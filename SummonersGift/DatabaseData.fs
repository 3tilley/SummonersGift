namespace SummonersGift.Data

open SummonersGift.Models.Entity
open SummonersGift.Models.View
open SummonersGift.Models.Riot

module DatabaseData =

    use db = new SgdbContext()

    let rankMap = Rank().RankMap

    let statMap = SummonersGift.Data.Stat().StatMap
    
    let query tId dId =
        query {
            for stat in db.AggStats do
            where ((stat.TierId = tId)
                && (stat.DivisionId = dId)
                && (stat.ChampionId = 0s )
                && (stat.RoleId = 0uy)
                && (stat.Winner = System.Nullable())
                && (stat.IsBlue = System.Nullable())
                && (stat.StatId <= 4s))
            select stat }

    let stats(tier, division, matches) =
        
        // TODO: Fix these conversions
        let (tierId, divId) = rankMap.[(tier, division)]
        query tierId divId
        |> Seq.toArray
        |> Array.map (fun i ->
            let sumStats =
                matches
                |> Seq.map StatFunctions.statFuncMap.[i.StatId]
                |> Seq.average
            StatBasicViewModel(byte(i.StatId), statMap.[i.StatId], i.mean.Value, sumStats) )
