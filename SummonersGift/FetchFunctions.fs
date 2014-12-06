namespace SummonersGift

open FSharp.Data 
open SummonersGift.Keys

module FetchFunctions =

    type Game1_3 = JsonProvider<"..\ExampleJSON\game-v1.3.JSON">
    type League2_5 = JsonProvider<"..\ExampleJSON\league-v2.5.JSON">
    type MatchHistory2_2 = JsonProvider<"..\ExampleJSON\matchhistory-v2.2.JSON">
    type Match2_2 = JsonProvider<"..\ExampleJSON\match-v2.2.JSON">
    type Stats1_3_Ranked = JsonProvider<"..\ExampleJSON\stats-v1.3_ranked.JSON">
    type Stats1_3_Summary = JsonProvider<"..\ExampleJSON\stats-v1.3_summary.JSON">
    type Summoner1_4 = JsonProvider<"..\ExampleJSON\summoner-v1.4.JSON">


    let getSummoner (region:string) (summonerName:string) =

        "https://" + region + ".api.pvp.net/api/lol/" + region + "/v1.4/summoner/by-name/" + summonerName.Replace(" ","%20") + "?api_key=" + lolApiKey
            |> Summoner1_4.Load