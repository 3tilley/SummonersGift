namespace SummonersGift.Tests

open System
open System.Collections.Generic

open NUnit.Framework
open FsUnit
open Newtonsoft.Json

open SummonersGift.Models.Riot

module Json =

    
    let readJsonFile filename =
        filename
        |> System.IO.File.ReadAllText

    let summonerJsonFile = __SOURCE_DIRECTORY__ +  @"\..\ExampleJson\summoner-v1.4.json"
    let matchHistoryFile = __SOURCE_DIRECTORY__ +  @"\..\ExampleJson\matchhistory-v2.2.json"

    let jsonFiles = [summonerJsonFile; matchHistoryFile]

    let mutable summonerJson : string = null
    let mutable matchHistoryJson : string = null

    let SummonerJson() =
        match summonerJson with
        | null ->
            summonerJson <- readJsonFile summonerJsonFile
            summonerJson
        | x -> x

    let MatchHistoryJson() =
        match matchHistoryJson with
        | null ->
            matchHistoryJson <- readJsonFile matchHistoryFile
            matchHistoryJson
        | x -> x

    
    [<TestFixture>]
    type JsonConverters() =

        [<Test>]
        member x.``Check example json files exist`` () =
            jsonFiles
            |> List.iter (fun i ->
                printfn "%A" (System.IO.Path.GetFullPath(i))
                System.IO.File.Exists(i) |> should be True)

        [<Test>]
        member x.``Deserialise summoners json into generic dict`` () =
            JsonConvert.DeserializeObject<Dictionary<string, Summoner_1_4>>(SummonerJson()) |> ignore

        [<Test>]
        member x.``Check members of json dict`` () =
            let d = JsonConvert.DeserializeObject<Dictionary<string, Summoner_1_4>>(SummonerJson())
            d |> should haveCount 2
            d.Keys |> should contain "sambucatus"
            d.Keys |> should contain "proheme"

        [<Test>]
        member x.``Deserialise summoners json into F# Map`` () =
            JsonConvert.DeserializeObject<Map<string, Summoner_1_4>>(SummonerJson()) |> ignore

        [<Test>]
        member x.``Check members of SummonerJson object`` () =
            let d = JsonConvert.DeserializeObject<Map<string, Summoner_1_4>>(SummonerJson())
            d |> should haveCount 2
            d.ContainsKey "sambucatus" |> should be True
            d.ContainsKey "proheme" |> should be True

        [<Test>]
        member x.``Deserialise match history json into generic list`` () =
            JsonConvert.DeserializeObject<MatchHistory_2_2>(MatchHistoryJson())
            |> ignore

        [<Test>]
        member x.``Check members of SummonerJson`` () =
            let d = JsonConvert.DeserializeObject<MatchHistory_2_2>(MatchHistoryJson())
            d.Matches |> should haveCount 10
            d.Matches |> Seq.exists (fun i -> i.MatchId = 1792915017) |> should be True