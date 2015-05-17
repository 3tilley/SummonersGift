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
        
    let jsonFiles = [summonerJsonFile]

    let mutable summonerJson : string = null

    let SummonerJson() =
        match summonerJson with
        | null ->
            summonerJson <- readJsonFile summonerJsonFile
            summonerJson
        | x -> x

    
    [<TestFixture>]
    type JsonConverters() =

        [<Test>]
        member x.``Check example json files exist`` () =
            [summonerJsonFile]
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
        member x.``Deserialise summoners json into SummonerJson object`` () =
            JsonConvert.DeserializeObject<Map<string, Summoner_1_4>>(SummonerJson()) |> ignore

        [<Test>]
        member x.``Check members of SummonerJson object`` () =
            let d = JsonConvert.DeserializeObject<Map<string, Summoner_1_4>>(SummonerJson())
            d |> should haveCount 2
            d.ContainsKey "sambucatus" |> should be True
            d.ContainsKey "proheme" |> should be True