// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.

open SummonersGift

[<EntryPoint>]
let main argv = 
    let rate = 500.0 / (60.0 * 10.0)
    let waitTime = 1.0 / rate

    let startMatch = 2066999976

    let wc = new System.Net.WebClient()

    let baseUrlMatch = @"https://euw.api.pvp.net/api/lol/euw/v2.2/match/"
    let baseUrlSummoner = @"https://euw.api.pvp.net/api/lol/euw/v2.5/league/by-summoner/"

    let matchesSaveFolder = System.IO.Path.Combine(__SOURCE_DIRECTORY__, """..""", "Data", "Matches","AprilData")
    let summonersSaveFolder = System.IO.Path.Combine(__SOURCE_DIRECTORY__, """..""", "Data", "Summoners","AprilData")

    let getJsonMatch saveFolder matchId key =
        let saveFile = System.IO.Path.Combine(saveFolder, string(matchId) + ".json")
        let url = baseUrlMatch + string(matchId) + "?includeTimeline=True&api_key=" + key
        printfn "%s" url
        try 
            let data = wc.DownloadString(url)
            System.IO.File.WriteAllText(saveFile, data)
        with
        | e -> printfn "%A" e

    let getJsonSummoners saveFolder summonerIds key =
        let summonerString = summonerIds |> String.concat ","
        let saveFile = System.IO.Path.Combine(saveFolder, string(summonerString) + ".json")
        let url = baseUrlSummoner + string(summonerString) + "/entry?api_key=" + key
        printfn "%s" url
        try 
            let data = wc.DownloadString(url)
            System.IO.File.WriteAllText(saveFile, data)
        with
        | e -> printfn "%A" e

    let key1 = Keys.keys.[0].Key
    let key2 = Keys.keys.[1].Key

    // get summoners
    let summoners =
        System.IO.File.ReadAllLines(@"C:\Users\Max\Documents\GitHub\SummonersGift\Data\Summoners\summoners.csv")
        |> Array.map (fun s -> s.Split(',').[1])
    
    let lst = System.Collections.Generic.List<string[]>()
    let stride = 9
    for i in 0..stride..summoners.Length do
        if (i + stride) < summoners.Length then
            lst.Add(summoners.[i..(i+stride-1)])
        else
            lst.Add(summoners.[i..])
    
    lst |> Seq.iter (fun i ->
        getJsonSummoners summonersSaveFolder i key1
        System.Threading.Thread.Sleep((int(waitTime * 1000.0))))

    // get matches
//    for i in 0..1000000 do
//        getJsonMatch matchesSaveFolder (startMatch+(2*i)) key1
//        getJsonMatch matchesSaveFolder (startMatch+(2*i)+1) key2
//        System.Threading.Thread.Sleep(int(waitTime * 1000.0))
    
    0

   
