namespace SummonersGift.Data

open FSharp.Data 

module JsonProviders =

    type Game_1_3 = JsonProvider<"..\ExampleJSON\game-v1.3.JSON">
    type League_2_5 = JsonProvider<"..\ExampleJSON\league-v2.5.JSON">
    type MatchHistory_2_2 = JsonProvider<"..\ExampleJSON\matchhistory-v2.2.JSON">
    type Matc_h2_2 = JsonProvider<"..\ExampleJSON\match-v2.2.JSON">
    type Stats_1_3_Ranked = JsonProvider<"..\ExampleJSON\stats-v1.3_ranked.JSON">
    type Stats_1_3_Summary = JsonProvider<"..\ExampleJSON\stats-v1.3_summary.JSON">
    type Summoner_1_4 = JsonProvider<"..\ExampleJSON\summoner-v1.4.JSON">
    type League_Entry_2_5 = JsonProvider<"..\ExampleJSON\league-entry-v2.5.json">

module Endpoints =

    let baseUrl = @"https://euw.api.pvp.net"

    let matchHistory = 
        """/api/lol/{region}/v2.2/matchhistory?{summonerId}/rankedQueues="RANKED_SOLO_5x5"%beginIndex={begin}&endIndex={end}"""

    let summonerNames =
        @"/api/lol/{region}/v1.4/summoner/by-name/{summonerNames}"

    let summonerLeagues =
        @"/api/lol/{region}/v2.5/league/by-summoner/{summonerIds}/entry"


        