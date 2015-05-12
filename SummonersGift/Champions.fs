namespace SummonersGift.Data

module Champions =


    let internal buildChampionMap version =
        let db = new SummonersGift.Entity.SgdbContext()

        let query = query { for champ in db.Champions do
                            where (champ.Version = version)
                            select champ }
        query
        |> Seq.map (fun c -> (int c.ChampionId, c.Name))
        |> Map.ofSeq

    let internal champMap = buildChampionMap "5.8.1"
    
    let ChampNameOrDefault id valueIfDefault =
            match champMap.TryFind id with
            | Some x -> x
            | None -> valueIfDefault