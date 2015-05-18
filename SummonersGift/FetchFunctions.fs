namespace SummonersGift.Data

open FSharp.Data 

//    type Game_1_3 = JsonProvider<"..\ExampleJSON\game-v1.3.JSON">
//    type League_2_5 = JsonProvider<"..\ExampleJSON\league-v2.5.JSON">
//    type MatchHistory_2_2 = JsonProvider<"..\ExampleJSON\matchhistory-v2.2.JSON">
//    type Matc_h2_2 = JsonProvider<"..\ExampleJSON\match-v2.2.JSON">
//    type Stats_1_3_Ranked = JsonProvider<"..\ExampleJSON\stats-v1.3_ranked.JSON">
//    type Stats_1_3_Summary = JsonProvider<"..\ExampleJSON\stats-v1.3_summary.JSON">
//    type Summoner_1_4 = JsonProvider<"..\ExampleJSON\summoner-v1.4.JSON", SampleIsList=true>
//    type League_Entry_2_5 = JsonProvider<"..\ExampleJSON\league-entry-v2.5.json">



module Endpoints =

    let baseUrl = @"https://euw.api.pvp.net"

    let matchHistoryEndpoint = 
        "/api/lol/{region}/v2.2/matchhistory/{summonerId}?rankedQueues=RANKED_SOLO_5x5&beginIndex={begin}&endIndex={end}"

    let summonerNamesEndpoint =
        @"/api/lol/{region}/v1.4/summoner/by-name/{summonerNames}"

    let summonerLeaguesEndpoint =
        @"/api/lol/{region}/v2.5/league/by-summoner/{summonerIds}/entry"

    let addApiKey key (request : string) =
        if request.Contains("?") then
            request + "&api_key=" + key
        else
            request + "?api_key=" + key

    let buildMatchHistoryUrl region summonerId index key =
        matchHistoryEndpoint.Replace("{summonerId}",summonerId).Replace("{region}", region)
            .Replace("{begin}",string(index)).Replace("{end}",string(index + 14))
        |> addApiKey key
        |> (+) baseUrl

    let buildSummonerNamesUrl region (summonerNames : string seq) key =
        let escapedNames =
            summonerNames
            |> Seq.map System.Web.HttpUtility.HtmlEncode
            |> String.concat ","
        summonerNamesEndpoint.Replace("{summonerNames}", escapedNames).Replace("{region}", region)
        |> addApiKey key
        |> (+) baseUrl

    let buildSummonerLeagues region summonerIds key =
        summonerLeaguesEndpoint.Replace("{region}", region)
            .Replace("{summonerIds}", summonerIds |> String.concat ",")
        |> addApiKey key
        |> (+) baseUrl