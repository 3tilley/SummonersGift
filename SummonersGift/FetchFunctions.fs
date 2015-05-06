namespace SummonersGift

open FSharp.Data 
open SummonersGift.Keys

module DataSource =

    let wc = new System.Net.WebClient()

module FetchFunctions =

    open DataSource

    type Game1_3 = JsonProvider<"..\ExampleJSON\game-v1.3.JSON">
    type League2_5 = JsonProvider<"..\ExampleJSON\league-v2.5.JSON">
    type MatchHistory2_2 = JsonProvider<"..\ExampleJSON\matchhistory-v2.2.JSON">
    type Match2_2 = JsonProvider<"..\ExampleJSON\match-v2.2.JSON">
    type Stats1_3_Ranked = JsonProvider<"..\ExampleJSON\stats-v1.3_ranked.JSON">
    type Stats1_3_Summary = JsonProvider<"..\ExampleJSON\stats-v1.3_summary.JSON">
    type Summoner1_4 = JsonProvider<"..\ExampleJSON\summoner-v1.4.JSON">

    type Summoner = {id:int;name:string;profileIconId:int;summonerLevel:int;revisionDate:int64}

//    This function isn't great.
//    let getSummoner (region:string) (summonerName:string) =
//
//        "https://" + region + ".api.pvp.net/api/lol/" + region + "/v1.4/summoner/by-name/" + summonerName.Replace(" ","%20") + "?api_key=" + lolApiKey
//            |> Summoner1_4.Load


    let getSummoner (region:string) (summonerNames:string[]) lolApiKey = 

        let summoners = (String.concat "," summonerNames).Replace(" ","%20") 
        let query = "https://" + region + ".api.pvp.net/api/lol/" + region + "/v1.4/summoner/by-name/" + summoners + "?api_key=" + lolApiKey
        let result = wc.DownloadString query
        let dictType = System.Collections.Generic.Dictionary<string,Summoner>().GetType()
        let deserialised = Newtonsoft.Json.JsonConvert.DeserializeObject(result,dictType)
        deserialised :?> System.Collections.Generic.Dictionary<string,Summoner>
        