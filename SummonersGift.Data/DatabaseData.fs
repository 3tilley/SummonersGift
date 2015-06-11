namespace SummonersGift.Data

open SummonersGift.Models.Entity
open SummonersGift.Models.View
open SummonersGift.Models.Riot

module DatabaseData =

    use db = new SgdbContext()

    let rankMap = Rank().RankMap

    let roleLane = RoleAndLane()

    let statMap = SummonersGift.Data.Stat().StatMap
    
    let queryNoRole tId dId =
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

    let queryByRole tId dId rId =
        query {
                for stat in db.AggStats do
                where ((stat.TierId = tId)
                    && (stat.DivisionId = dId)
                    && (stat.ChampionId = 0s )
                    && (stat.RoleId = rId)
                    && (stat.Winner = System.Nullable())
                    && (stat.IsBlue = System.Nullable())
                    && (stat.StatId <= 4s))
                select stat }        

    let matchesToStatRow  matches (stat : AggStat) =
        let avgStats =
            matches
            |> Seq.map StatFunctions.statFuncMap.[stat.StatId]
            |> Seq.average
        StatRowViewModel(byte(stat.StatId), statMap.[stat.StatId], stat.mean.Value, stat.sem.Value, avgStats)

    let matchWins (matches : Match seq) =
        matches
        |> Seq.fold (fun acc i ->
            match i.Participants.[0].Stats.Winner with
            | true -> acc + 1
            | false -> acc + 0
            ) 0

    let stats(tier, division, matches) =
        
        // TODO: Fix these conversions
        let (tierId, divId) = rankMap.[(tier, division)]
        let queryResult = 
            queryNoRole tierId divId
            |> Seq.toArray
        
        let roleResults =
            matches
            |> Seq.groupBy (fun (i : Match) -> i.Participants.[0].Timeline.Role)
            |> Seq.map(fun (k, group) ->
                let role = roleLane.RoleOrNullByJson(k)
                let res =
                    queryByRole tierId divId role.RoleId
                    |> Seq.toArray
                res
                |> Array.map (matchesToStatRow group)
                |> fun i ->
                    let count = if res |> Array.length = 0 then 0 else res.[0].count
                    let winCount = group |> matchWins
                    StatTableViewModel(i, group |> Seq.length, winCount, count, ["lane", role.Description])
                    
            )
            |> Seq.toList
            |> List.filter (fun i -> i.DatasetGames > 0)

        let wins = matches |> matchWins
        let baseResults =
            queryResult
            |> Array.map (matchesToStatRow matches)
            |> fun i -> StatTableViewModel(i, matches |> Seq.length, wins, queryResult.[0].count, [])

        baseResults::roleResults
