namespace SummonersGift.Data

open SummonersGift.Models.Riot

module StatFunctions =
    
    let csAt10 (rMatch : Match) =
        rMatch.Participants.[0].Timeline.CreepsPerMinDeltas.ZeroToTen * 10.0

    let damageTakenAt10 (rMatch : Match) =
        rMatch.Participants.[0].Timeline.DamageTakenPerMinDeltas.ZeroToTen * 10.0

    let goldAt10 (rMatch : Match) =
        rMatch.Participants.[0].Timeline.GoldPerMinDeltas.ZeroToTen * 10.0

    let xpAt10 (rMatch : Match) =
        rMatch.Participants.[0].Timeline.XpPerMinDeltas.ZeroToTen * 10.0

    let statFuncMap : Map<int16, Match -> float> =
        [ (1, csAt10); (2, damageTakenAt10); (3, goldAt10); (4, xpAt10) ]
        |> List.map (fun (i,v) -> (int16 i, v))
        |> Map.ofList