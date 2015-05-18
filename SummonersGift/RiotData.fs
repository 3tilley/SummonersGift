﻿namespace SummonersGift.Data

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
        DatabaseCalls    : int
        RedisCalls       : int
    }
        member x.GetResult =
            match x.Success with
            | true -> x.Result.Value
            | false -> failwith "No result available"

type internal Result private () =
    static member SuccessfulResult(result, apiCalls, ?dbCalls, ?redisCalls) =
        {   Success = true
            Result = Some result
            ErrorMessage = null
            ApiCalls = apiCalls
            DatabaseCalls = if dbCalls.IsNone then 0 else dbCalls.Value
            RedisCalls = if redisCalls.IsNone then 0 else redisCalls.Value }

    static member ErrorResult(message, apiCalls, ?dbCalls, ?redisCalls) =
        {   Success = false
            Result = None
            ErrorMessage = message
            ApiCalls = apiCalls
            DatabaseCalls = if dbCalls.IsNone then 0 else dbCalls.Value
            RedisCalls = if redisCalls.IsNone then 0 else redisCalls.Value }

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

    let buildSummonerObject summonerResponseString =
        JsonConvert.DeserializeObject<Map<string, Summoner_1_4>>(summonerResponseString)

    let buildMatchHistoryObject matchHistoryString =
        JsonConvert.DeserializeObject<MatchHistory_2_2>(matchHistoryString)

    let asyncTryGetSummoner region escapedName key =
        async {
                let url = buildSummonerNamesUrl region [escapedName] key.Key
                let! riotData = asyncRiotCall url
                match riotData with
                | Data s ->
                    let jsonObj = buildSummonerObject s
                    match jsonObj.TryFind escapedName with
                    | Some x ->
                        return Some jsonObj
                    | None ->
                        return None
            }
        
    let asyncTryGetMatchHistory region id (condition : Match -> bool) (reduce : Match -> 'T) key delay =
        let matchList = new System.Collections.Generic.List<'T>()
        let rec getMatches index calls =
            async {
                let url = buildMatchHistoryUrl region (string(id)) 0 key
                let! hist = asyncRiotCall url
                match hist with
                | Data s ->
                    let histObj = buildMatchHistoryObject s
                
                    let filtered =
                        histObj.Matches
                        |> Seq.filter condition
                    filtered
                    |> Seq.iter (fun game -> matchList.Add(reduce game))
                    match filtered |> Seq.length with
                    | 15 ->
                        Async.Sleep(int(delay * 1000.0)) |> ignore
                        return! getMatches (index + 15) (calls + 1)
                    | _ -> return (Some matchList, calls)
                | Error(ec, mes) ->
                    return (None, calls)
            }
        getMatches 0 0

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

        member public x.GetSummonerIdAndMatchesThisSeasonAsync(region, escapedName) =
            async {
                let! summoner = asyncTryGetSummoner region escapedName key
                match summoner with
                | None ->
                    return Result.ErrorResult("Summoner not found", 1)
                | Some sumMap ->
                    match escapedName |> sumMap.TryFind with
                    | None ->
                        return Result.ErrorResult("Summoner not found", 1)
                    | Some n ->
                        let summonerId = n.Id

                        let! hist =
                            asyncTryGetMatchHistory region id (fun i -> i.Season = "SEASON2015")
                                 id key.Key (1.0 / key.ReqsPerSecond)
                        match hist with
                        | (None, x) ->
                            // One extra call for the summoner call above
                            return Result.ErrorResult("Match history failure", x + 1)
                        | (Some h, x) ->
                            // One extra call for the summoner call above
                            return Result.SuccessfulResult(h, x + 1)
                    }
                    |> Async.StartAsTask

        member public x.GetSummonerIdAsync(region, escapedName) =
            async {
                let url = buildSummonerNamesUrl region [escapedName] key.Key
                let! riotData = asyncRiotCall url
                match riotData with
                | Data s ->
                    let jsonObj = buildSummonerObject s
                    match jsonObj.TryFind escapedName with
                    | Some x ->
                        return Result.SuccessfulResult(SummonerBasicViewModel x, 1)
                    | None ->
                        return Result.ErrorResult("The summoner was not found", 1)
                | Error(ec, reason) ->
                    return Result.ErrorResult(reason, 1)
                }
                |> Async.StartAsTask
       

