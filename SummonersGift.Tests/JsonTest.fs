namespace SummonersGift.Tests

open System
open System.Collections.Generic

open NUnit.Framework
open FsUnit
open Newtonsoft.Json

open SummonersGift.Models.Riot
open SummonersGift.Data.RiotData

module Json =
    
    let prohemeName = "proheme"
    let prohemeId = 43518871
    
    let readJsonFile filename =
        filename
        |> System.IO.File.ReadAllText

    let summonerJsonFile = __SOURCE_DIRECTORY__ +  @"\..\ExampleJson\summoner-v1.4.json"
    let matchHistoryFile = __SOURCE_DIRECTORY__ +  @"\..\ExampleJson\matchhistory-v2.2.json"
    let leagueEntryFile = __SOURCE_DIRECTORY__ +  @"\..\ExampleJson\league-entry-v2.5.json"

    let jsonFiles = [summonerJsonFile; matchHistoryFile; leagueEntryFile]

    let mutable summonerJson : string = null
    let mutable matchHistoryJson : string = null
    let mutable leagueEntryJson : string = null

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

    let LeagueEntryJson() =
        match leagueEntryJson with
        | null ->
            leagueEntryJson <- readJsonFile leagueEntryFile
            leagueEntryJson
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
            let d = buildSummonerObject(SummonerJson())
            d |> should haveCount 2
            d.ContainsKey "sambucatus" |> should be True
            d.ContainsKey "proheme" |> should be True

        [<Test>]
        member x.``Deserialise match history json into generic list`` () =
            JsonConvert.DeserializeObject<MatchHistory_2_2>(MatchHistoryJson())
            |> ignore

        [<Test>]
        member x.``Check members of SummonerJson`` () =
            let d = buildMatchHistoryObject(MatchHistoryJson())
            d.Matches |> should haveCount 10
            d.Matches |> Seq.exists (fun i -> i.MatchId = 1792915017) |> should be True

        [<Test>]
        member x.``Deserialise summoner leagues into generic list`` () =
            let d = JsonConvert.DeserializeObject<Map<int, List<LeagueJson_2_5>>>(LeagueEntryJson())
            ()

        [<Test>]
        member x.``Check members of LeagueEntryJson`` () =
            let d = buildLeagueObject(LeagueEntryJson())
            d |> should haveCount 2
            d.ContainsKey (prohemeId) |> should be True
            d.[prohemeId] |> should haveLength 1
            d.[prohemeId].[0].Entries.[0].Division |> should equal "V"