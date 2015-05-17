namespace SummonersGift.Data

open SummonersGift.Models.Entity

type Champions(version) =
    let buildChampionMap version =
        use db = new SgdbContext()

        let query = query { for champ in db.Champions do
                            where (champ.Version = version)
                            select champ }
        query
        |> Seq.map (fun c -> (int c.ChampionId, c.Name))
        |> Map.ofSeq

    let champMap = buildChampionMap version
    
    member x.ChampNameOrDefault(id, valueIfDefault) =
            match champMap.TryFind id with
            | Some x -> x
            | None -> valueIfDefault

type Stat() =
    let buildStatMap version =
        use db = new SgdbContext()

        let query =
            query { for stat in db.Stats do
                    select stat}

        query
        |> Seq.map (fun s -> s.StatId, s.StatName)
        |> Map.ofSeq