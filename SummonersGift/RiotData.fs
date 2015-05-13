namespace SummonersGift.Data

open System.Net

type DataFetcher(keys : Utils.ApiKey seq) =
    
    // Make sure we've only got one key (for now)
    let key = 
        match keys |> Seq.length with
            | 0 -> failwith "Need at least one key"
            | 1 -> keys |> Seq.head
            | x when x > 2 -> failwith "Multiple keys not supported"



    member public x.GetSummonerId(region, name) =
        let url = Endpoints.buildSummonerNamesUrl region [name] key.Key
        let wc = new System.Net.WebClient()
        let resp = wc.DownloadString(url)
        let j = JsonProviders.Summoner_1_4.Parse(resp)
        j.JsonValue.ToString()

