namespace SummonersGift.Data

open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Threading.Tasks

open Newtonsoft.Json

open SummonersGift.Data.Utils
open SummonersGift.Data.Endpoints
open SummonersGift.Models.Riot
open SummonersGift.Models.View

type DataFetchResult<'T> =
    {
        Success         : bool
        Result          : 'T option
        ErrorMessage    : string
        ApiCalls        : int
        DatabaseCalls   : int
        RedisCalls      : int
        TimeTaken       : TimeSpan
    }
        member x.GetResult =
            match x.Success with
            | true -> x.Result.Value
            | false -> failwith "No result available"

type internal Result private () =
    static member SuccessfulResult(result, start, apiCalls, ?dbCalls, ?redisCalls) =
        {   Success = true
            Result = Some result
            ErrorMessage = null
            ApiCalls = apiCalls
            DatabaseCalls = if dbCalls.IsNone then 0 else dbCalls.Value
            RedisCalls = if redisCalls.IsNone then 0 else redisCalls.Value
            TimeTaken = DateTime.UtcNow - start }

    static member ErrorResult(message, start, apiCalls, ?dbCalls, ?redisCalls) =
        {   Success = false
            Result = None
            ErrorMessage = message
            ApiCalls = apiCalls
            DatabaseCalls = if dbCalls.IsNone then 0 else dbCalls.Value
            RedisCalls = if redisCalls.IsNone then 0 else redisCalls.Value
            TimeTaken = DateTime.UtcNow - start }

module RiotRequest =

    [<Literal>]
    let RateLimitExceededError : HttpStatusCode = enum 429 

    type ErrorCode =
        | Http400 | Http401 | Http404 | Http429 | Http500 | Http503
            with static member fromStatusCode = 
                function
                | HttpStatusCode.BadRequest -> Http400
                | HttpStatusCode.Unauthorized -> Http401
                | HttpStatusCode.NotFound -> Http404
                | RateLimitExceededError -> Http429
                | HttpStatusCode.InternalServerError -> Http500
                | HttpStatusCode.ServiceUnavailable -> Http503

    type RiotResponse =
        | Data of string
        | Error of ErrorCode * string

    type InternalResult<'T> =
        | Success of 'T
        | Failure of string

    let asyncRiotCall (url : string) =
        async {
            use client = new HttpClient()
            client.DefaultRequestHeaders.Accept.Clear()
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"))
            let! response = client.GetAsync(url) |> Async.AwaitTask
            match response.IsSuccessStatusCode with
            | false ->
                return Error(ErrorCode.fromStatusCode(response.StatusCode), response.ReasonPhrase)
            | true ->
                let! responseData = response.Content.ReadAsStringAsync() |> Async.AwaitTask
                return Data(responseData)
                }

open RiotRequest


module RiotData =

    type Rank =
        {
            Tier : string
            Division : string
        }

    let buildSummonerObject summonerResponseString =
        JsonConvert.DeserializeObject<Map<string, Summoner_1_4>>(summonerResponseString)

    let buildMatchHistoryObject matchHistoryString =
        JsonConvert.DeserializeObject<MatchHistory_2_2>(matchHistoryString)

    let buildLeagueObject leagueResponseString =
        JsonConvert.DeserializeObject<Map<int, List<LeagueJson_2_5>>>(leagueResponseString)


    let asyncGetSummoner region escapedName key =
        async {
                let url = buildSummonerNamesUrl region [escapedName] key.Key
                let! riotData = asyncRiotCall url
                match riotData with
                | Data s ->
                    let jsonObj = buildSummonerObject s
                    match jsonObj.TryFind escapedName with
                    | Some x ->
                        return Success x
                    | None ->
                        return Failure "No summoner in object"
                | Error(ec, mes) ->
                    return Failure (ec.ToString() + mes)
            }
        
    let asyncGetMatchHistory region summonerId (condition : Match -> bool) (reduce : Match -> 'T) key delay =
        let matchList = new System.Collections.Generic.List<'T>()
        let rec getMatches index calls =
            async {
                let url = buildMatchHistoryUrl region (summonerId) index key
                let! hist = asyncRiotCall url
                match hist with
                | Data s ->
                    let histObj = buildMatchHistoryObject s
                    histObj.Matches.Reverse()
                    let filtered =
                        histObj.Matches
                        |> Seq.filter condition
                    filtered
                    |> Seq.iter (fun game -> matchList.Add(reduce game))
                    match filtered |> Seq.length with
                    | 15 ->
                        Async.Sleep(int(delay * 1000.0)) |> ignore
                        return! getMatches (index + 15) (calls + 1)
                    | _ -> 
                        return (Success matchList, calls + 1)
                | Error(ec, mes) ->
                    return (Failure (mes), calls + 1)
            }
        getMatches 0 0

    let asyncGetSummonerLeague region summonerId key =
        async {
                let url = buildSummonerLeagues region [summonerId] key
                let! riotData = asyncRiotCall url
                match riotData with
                | Data s ->
                    let jsonObj = buildLeagueObject s
                    match jsonObj.TryFind summonerId with
                    | Some x ->
                        return Success x
                    | None ->
                        return Failure "No summoner in object"
                | Error(ec, mes) ->
                    return Failure (ec.ToString() + mes)
                }

    let asyncGetSummonerRankedSoloLeague region summonerId key =
        async {
            let! maybeLeagues = asyncGetSummonerLeague region summonerId key
            match maybeLeagues with
            | Failure x ->
                return Failure x
            | Success leagues ->
                let soloQueue =
                    leagues
                    |> List.tryFind (fun i -> i.Queue = "RANKED_SOLO_5x5")
                match soloQueue with
                | None ->
                    return Success None
                | Some x ->
                    return Success (Some { Tier = x.Tier; Division = x.Entries.[0].Division })
            }

    type DataFetcher(keys : ApiKey seq) =

        let settings = new JsonSerializerSettings()
        do
            settings.ContractResolver <- new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()

        // Make sure we've only got one key (for now)
        let key = 
            match keys |> Seq.length with
                | 0 -> failwith "Need at least one key"
                | 1 -> keys |> Seq.head
                | x when x >= 2 -> failwith "Multiple keys not supported"

        member public x.GetSummonerLeagueAndMatchesThisSeasonAsync(region, escapedName) =
            async {
                let start = DateTime.UtcNow
                let! summoner = asyncGetSummoner region escapedName key
                match summoner with
                | Failure(mes) ->
                    return Result.ErrorResult("Summoner not found: " + mes, start, 1)
                | Success sumObj ->
                    let summonerId = sumObj.Id

                    Async.Sleep(int (1000.0 / key.ReqsPerSecond)) |> ignore

                    let! league =
                        asyncGetSummonerRankedSoloLeague region summonerId key.Key
                            
                    Async.Sleep(int (1000.0 / key.ReqsPerSecond)) |> ignore

                    let! hist =
                        asyncGetMatchHistory region summonerId (fun i -> i.Season = "SEASON2015")
                                id key.Key (1.0 / key.ReqsPerSecond)
                    match hist with
                    | (Failure s, x) ->
                        // Two extra calls for the summoner and league call above
                        return Result.ErrorResult("Match history failure: " + s, start, x + 2)
                    | (Success h, x) ->
                        // Two extra calls for the summoner and league call above
                        match league with
                        | Success rankResult ->
                            let (tier, div) = match rankResult with | Some r -> r.Tier, r.Division | None -> null, null
                            return Result.SuccessfulResult((SummonerBasicViewModel(sumObj, tier, div), h), start, x + 2)
                        | Failure x ->
                            return Result.ErrorResult("Could not find summoner leagues", start, 2)
                }
                |> Async.StartAsTask

        member public x.GetSummonerIdAsync(region, escapedName) =
            async {
                let start = DateTime.UtcNow
                let url = buildSummonerNamesUrl region [escapedName] key.Key
                let! riotData = asyncRiotCall url
                match riotData with
                | Data s ->
                    let jsonObj = buildSummonerObject s
                    match jsonObj.TryFind escapedName with
                    | Some x ->
                        let! rank = asyncGetSummonerRankedSoloLeague region x.Id key.Key
                        match rank with
                        | Success(Some r) ->
                            return Result.SuccessfulResult(SummonerBasicViewModel(x, r.Tier, r.Division), start, 2)
                        | Success None ->
                            return Result.SuccessfulResult(SummonerBasicViewModel(x, null, null), start, 2)
                        | Failure x ->
                            return Result.ErrorResult("Could not find summoner leagues", start, 2)
                    | None ->
                        return Result.ErrorResult("The summoner was not found", start, 1)
                | Error(ec, reason) ->
                    return Result.ErrorResult(reason, start, 1)
                }
                |> Async.StartAsTask
       

