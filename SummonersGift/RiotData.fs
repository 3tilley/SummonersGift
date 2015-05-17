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
        Success : bool
        Result  : 'T option
        ErrorMessage : string
    }
        member x.GetResult =
            match x.Success with
            | true -> x.Result.Value
            | false -> failwith "No result available"

module internal ResultHelpers =
    let successfulResult result =
        { Success = true; Result = Some result; ErrorMessage = null }

    let errorResult message =
        { Success = false; Result = None; ErrorMessage = message }

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

    let await (task : Task<'T>) =
        task.ContinueWith(Func<Task<'T>,_>(fun i -> i.Result)).Result

    let internal responseContinuation (response : HttpResponseMessage) =
        match response.IsSuccessStatusCode with
        | true ->
            response.Content.ReadAsStringAsync() |> await |> Data
        | false ->
            Error(ErrorCode.fromStatusCode(response.StatusCode), response.ReasonPhrase)
            

    let riotCallAsync (url : string) =
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

    let riotCall (url : string) =
        use client = new HttpClient()
        client.DefaultRequestHeaders.Accept.Clear()
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"))
        let response = client.GetAsync(url)
        response |> await |> responseContinuation

open ResultHelpers
open RiotRequest


module RiotData =

    let buildSummonerObject summonerResponseString =
        JsonConvert.DeserializeObject<Map<string, Summoner_1_4>>(summonerResponseString)

    let buildMatchHistoryObject matchHistoryString =
        JsonConvert.DeserializeObject<MatchHistory_2_2>(matchHistoryString)

    let tryGetSummonerAsync region escapedName key =
        async {
                let url = buildSummonerNamesUrl region [escapedName] key.Key
                let! riotData = riotCallAsync url
                match riotData with
                | Data s ->
                    let jsonObj = buildSummonerObject s
                    match jsonObj.TryFind escapedName with
                    | Some x ->
                        return Some jsonObj
                    | None ->
                        return None
            }
        
    let tryGetMatchHistoryAsync region id (condition : Match -> bool) (reduce : Match -> 'T) key delay =
        let matchList = new System.Collections.Generic.List<'T>()
        let rec getMatches index =
            async {
                let url = buildMatchHistoryUrl region (string(id)) 0 key
                let! hist = riotCallAsync url
                match hist with
                | Data s ->
                    let histObj = buildMatchHistoryObject s
                
                    let filtered =
                        histObj.matches
                        |> Seq.filter condition
                    filtered
                    |> Seq.iter (fun game -> matchList.Add(reduce game))
                    match filtered |> Seq.length with
                    | 15 ->
                        Async.Sleep(int(delay * 1000.0)) |> ignore
                        return! getMatches (index + 15)
                    | _ -> return Some matchList
                | Error(ec, mes) ->
                    return None
            }
        getMatches 0

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

        member public x.AsyncGetSummonerIdAndMatches(region, escapedName) =
            async {
                let! summoner = tryGetSummonerAsync region escapedName key
                // Todo: FIX THIS
                let summonerId = summoner.Value.Item(escapedName).Id

                let! hist =
                    tryGetMatchHistoryAsync region id (fun i -> i.season = "SEASON2015")
                         id key.Key (1.0 / key.ReqsPerSecond)
                // Todo: FIX THIS
                return hist.Value
                }
                |> Async.StartAsTask

        member public x.AsyncGetSummonerId(region, escapedName) =
            async {
                let url = buildSummonerNamesUrl region [escapedName] key.Key
                let! riotData = riotCallAsync url
                match riotData with
                | Data s ->
                    let jsonObj = buildSummonerObject s
                    match jsonObj.TryFind escapedName with
                    | Some x ->
                        return successfulResult (SummonerBasicViewModel x)
                    | None ->
                        return errorResult "The summoner was not found"
                | Error(ec, reason) ->
                    return errorResult reason
                }
                |> Async.StartAsTask

        member public x.GetSummonerId(region, escapedName) =
            let url = buildSummonerNamesUrl region [escapedName] key.Key
            match riotCall url with
            | Data s ->
                let jsonObj = JsonConvert.DeserializeObject<Map<string, Summoner_1_4>>(s)
                match jsonObj.TryFind escapedName with
                | Some x -> successfulResult (SummonerBasicViewModel x)
                | None -> errorResult "The summoner was not found"
            | Error(ec, reason) -> errorResult reason
            |> Task.FromResult
       

