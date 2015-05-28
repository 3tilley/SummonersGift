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
            where ((stat.TierId = tId) && (stat.DivisionId = dId))
            select stat }

    let stats(tier, division, matches : Match seq) =
        
        let (tierId, divId) = rankMap.[(tier, division)]
        query tierId divId
        |> Seq.toArray
        |> Array.map (fun i -> StatBasicViewModel(i.StatId, statMap[i], i.mean )
