

#r @"C:\Users\Andrew\Documents\Visual Studio 2012\Projects\lolapi\packages\FSharp.Data.1.1.10\lib\net40\FSharp.Data.dll"
let wc = new System.Net.WebClient()

let summonerNameString = "Proheme"
let summonerID = "35511005"
let key = "b4048e5a-060e-436f-af3e-a4e294995aa7"
type game = FSharp.Data.JsonProvider<"game12.JSON">
type statsRanked = FSharp.Data.JsonProvider<"stats12ranked.JSON">
type statsSummary = FSharp.Data.JsonProvider<"stats12summary.JSON">
type summonerName = FSharp.Data.JsonProvider<"summoner12byName.JSON">
type summonerMasteries = FSharp.Data.JsonProvider<"summoner12masteries.JSON">

let g = game.Load (@"http://prod.api.pvp.net/api/lol/euw/v1.2/game/by-summoner/" + summonerID + "/recent?api_key=" + key)
let sr = statsRanked.Load (@"http://prod.api.pvp.net/api/lol/euw/v1.2/stats/by-summoner/" + summonerID + "/ranked?season=SEASON3&api_key=" + key)
let ss = statsSummary.Load (@"http://prod.api.pvp.net/api/lol/euw/v1.2/stats/by-summoner/" + summonerID + "/summary?season=SEASON3&api_key=" + key)
let sn = summonerName.Load (@"http://prod.api.pvp.net/api/lol/euw/v1.2/summoner/by-name/" + summonerNameString + "?api_key=" + key )
let sm = summonerMasteries.Load (@"http://prod.api.pvp.net/api/lol/euw/v1.2/summoner/" + summonerID + "/masteries?api_key=" + key )


g.Games.[9].Statistics |> Array.map (fun s -> s.Name)

(g.Games.[9].Statistics |> Array.find (fun i -> i.Name="CHAMPIONS_KILLED")).Value
sr.Champions |> Array.map(fun c -> (c.Name,c.Stats.RankedSoloGamesPlayed))
sr.Champions.[1].Stats.TotalFirstBlood

ss.PlayerStatSummaries |> Array.map (fun i -> i.AggregatedStats.TotalChampionKills)
ss.PlayerStatSummaries |> Array.map (fun i -> i.PlayerStatSummaryType)

sn.ProfileIconId

sm.Pages