﻿namespace SummonersGift.Data

open System
open System.Net
open System.Net.Http
open System.Net.Http.Headers
open System.Threading.Tasks
open System.Diagnostics

open Newtonsoft.Json
open StackExchange.Redis

open SummonersGift.Data.Utils
open SummonersGift.Data.Endpoints
open SummonersGift.Data.RiotRequestPool
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

//    [<Literal>]
//    let RateLimitExceededError : HttpStatusCode = enum 429 

    type ErrorCode =
        | Http400 | Http401 | Http404 | Http429 | Http500 | Http503 | Other
        static member fromStatusCode =
            function
                | HttpStatusCode.BadRequest -> Http400
                | HttpStatusCode.Unauthorized -> Http401
                | HttpStatusCode.NotFound -> Http404
//                | RateLimitExceededError -> Http429
                | HttpStatusCode.InternalServerError -> Http500
                | HttpStatusCode.ServiceUnavailable -> Http503
                | _ -> Other


    type RiotResponse =
        | Data of string
        | Error of ErrorCode * string

    type InternalResult<'T> =
        | Success of 'T
        | Failure of string

    let rec asyncRiotCallFull firstCall (pool : IRequestPool) (url : string) =
        async {
            use client = new HttpClient()
            client.DefaultRequestHeaders.Accept.Clear()
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"))
            do! pool.GetRiotRequest()
            let! response = client.GetAsync(url) |> Async.AwaitTask
            match response.IsSuccessStatusCode with
            | false ->
                match (int(response.StatusCode), firstCall, response.Headers.RetryAfter.Delta.HasValue) with
                | (429, true, true) ->
                    let retryAfter = response.Headers.RetryAfter.Delta.Value.Milliseconds
                    Trace.TraceInformation("429: Attempting retry header")
                    do! Async.Sleep(retryAfter + 200)
                    return! asyncRiotCallFull false pool url
                | _ -> 
                    Trace.TraceError(url + " returned " + (response.StatusCode.ToString()))
                    return Error(ErrorCode.fromStatusCode(response.StatusCode), response.ReasonPhrase)
            | true ->
                let! responseData = response.Content.ReadAsStringAsync() |> Async.AwaitTask
                return Data(responseData)
                }

    let asyncRiotCall (pool : IRequestPool) (url : string) =
        asyncRiotCallFull true pool url

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


    let asyncGetSummoner region escapedName key pool =
        async {
                let url = buildSummonerNamesUrl region [escapedName] key.Key
                let! riotData = asyncRiotCall pool url
                match riotData with
                | Data s ->
                    let jsonObj = buildSummonerObject s
                    let nameKey = escapedName.ToLower().Replace(" ", "")
                    match jsonObj.TryFind nameKey with
                    | Some x ->
                        return Success x
                    | None ->
                        return Failure "No summoner in object"
                | Error(ec, mes) ->
                    return Failure (ec.ToString() + mes)
            }
        
    let asyncGetMatchHistory region summonerId (condition : Match -> bool) (reduce : Match -> 'T) key pool =
        let matchList = new System.Collections.Generic.List<'T>()
        let rec getMatches index calls =
            async {
                let url = buildMatchHistoryUrl region (summonerId) index key
                let! hist = asyncRiotCall pool url
                match hist with
                | Data "{}" ->
                    return (Success matchList, calls + 1)
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
                        return! getMatches (index + 15) (calls + 1)
                    | _ -> 
                        return (Success matchList, calls + 1)
                | Error(ec, mes) ->
                    return (Failure (mes), calls + 1)
            }
        getMatches 0 0

    let asyncGetSummonerLeague region summonerId key pool =
        async {
                let url = buildSummonerLeagues region [summonerId] key
                let! riotData = asyncRiotCall pool url
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

    let asyncGetSummonerRankedSoloLeague region summonerId key pool =
        async {
            let! maybeLeagues = asyncGetSummonerLeague region summonerId key pool
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

    type DataFetcher(keys : ApiKey seq, pool, redis : ConnectionMultiplexer) =

        let settings = new JsonSerializerSettings()
        do
            settings.ContractResolver <- new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            //redis.

        // Make sure we've only got one key (for now)
        let key = 
            match keys |> Seq.length with
                | 0 -> failwith "Need at least one key"
                | 1 -> keys |> Seq.head
                | x when x >= 2 -> failwith "Multiple keys not supported"

        member public x.GetSummonerLeagueAndMatchesThisSeasonAsync(region2 : string, escapedName2 : string) =
            async {
                let lowerRegion = region2.ToLower()
                let lowerName = escapedName2.ToLower()
                let start = DateTime.UtcNow
                let! summoner = asyncGetSummoner lowerRegion lowerName key pool
                match summoner with
                | Failure(mes) ->
                    return Result.ErrorResult("Summoner not found: " + mes, start, 1)
                | Success sumObj ->
                    let summonerId = sumObj.Id

                    let! league =
                        asyncGetSummonerRankedSoloLeague lowerRegion summonerId key.Key pool

                    let! hist =
                        asyncGetMatchHistory lowerRegion summonerId (fun i -> i.Season = "SEASON2015")
                                id key.Key pool
                    match hist with
                    | (Failure s, x) ->
                        // Two extra calls for the summoner and league call above
                        return Result.ErrorResult("Match history failure: " + s, start, x + 2)
                    | (Success h, x) ->
                        // Two extra calls for the summoner and league call above
                        match league with
                        | Success rankResult ->
                            match rankResult with
                            | Some r ->
                                let stats = DatabaseData.stats(r.Tier, r.Division, h)
                                let basicSummoner = SummonerBasicViewModel(sumObj, r.Tier, r.Division)
                                return Result.SuccessfulResult(Summoner.buildFullViewModel(basicSummoner, stats, h), start, x + 2)
                            | None ->
                                let basicSummoner = SummonerBasicViewModel(sumObj, null, null)
                                return Result.SuccessfulResult(SummonerFullViewModel.CreateSummonerWithoutStats(basicSummoner), start, x + 2)
                        | Failure x ->
                            return Result.ErrorResult("Could not find summoner leagues", start, 2)
                }
                |> Async.StartAsTask

        member public x.GetSummonerIdAsync(region2 : string, escapedName2 : string) =
            async {
                let lowerRegion = region2.ToLower()
                let lowerName = escapedName2.ToLower()
                let start = DateTime.UtcNow
                let url = buildSummonerNamesUrl lowerRegion [lowerName] key.Key
                let! riotData = asyncRiotCall pool url
                match riotData with
                | Data s ->
                    let jsonObj = buildSummonerObject s
                    match jsonObj.TryFind lowerName with
                    | Some x ->
                        let! rank = asyncGetSummonerRankedSoloLeague lowerRegion x.Id key.Key pool
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
       

