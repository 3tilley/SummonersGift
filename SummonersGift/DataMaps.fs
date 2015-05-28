namespace SummonersGift.Data

open SummonersGift.Models.Entity

type ChampionStatic =
    {
        Id : int16
        Name : string
        FullImage : string
    }

type Champions(version) =
    let buildChampionMap version =
        use db = new SgdbContext()

        let query = query { for champ in db.Champions do
                            where (champ.Version = version)
                            select champ }
        query
        |> Seq.map (fun c -> (int c.Id, c))
        |> Map.ofSeq

    let champMap = buildChampionMap version
    
    member x.ChampNameOrDefault(id, valueIfDefault) =
            match champMap.TryFind id with
            | Some x -> x.Name
            | None -> valueIfDefault

    member x.ChampOrNull(id) =
        match champMap.TryFind id with
        | Some x -> x
        | None -> null

    member val ProfileBase =
        "http://ddragon.leagueoflegends.com/cdn/" + version + "/img/profileicon/"
        with get

    member val SplashBase =
        "http://ddragon.leagueoflegends.com/cdn/img/champion/splash/"
        with get

    member val LoadScreenBase =
        "http://ddragon.leagueoflegends.com/cdn/img/champion/loading/"
        with get

    member val SquareBase =
        "http://ddragon.leagueoflegends.com/cdn/" + version + "/img/champion/"
        with get

type Summoners(version) =
    let buildSpellMap version =
        use db = new SgdbContext()

        let query = query { for spell in db.Summoners do
                            where (spell.Version = version)
                            select spell }
        query
        |> Seq.map (fun s -> (int s.Id, s))
        |> Map.ofSeq

    let spellMap = buildSpellMap version
    
    member x.SpellNameOrDefault(id, valueIfDefault) =
            match spellMap.TryFind id with
            | Some x -> x.Name
            | None -> valueIfDefault

    member x.SpellOrNull(id) =
        match spellMap.TryFind id with
        | Some x -> x
        | None -> null

    member val SquareBase =
        "http://ddragon.leagueoflegends.com/cdn/" + version + "/img/spell/"
        with get


type Stat() =
    let buildStatMap =
        use db = new SgdbContext()

        let query =
            query { for stat in db.Stats do
                    select stat}

        query
        |> Seq.map (fun s -> s.StatId, s.StatName)
        |> Map.ofSeq

    let map = buildStatMap

    member x.StatMap = map

type Rank() =
    let buildRankMap =
        use db = new SgdbContext()

        let queryT =
            query { for tier in db.Tiers do
                    select tier }
        
        let queryD =
            query { for div in db.Divisions do
                    select div }

        let cross =
            seq { for i in queryT do
                    for j in queryD -> (i,j)}

        cross
        |> Seq.map (fun (i, j) -> ((i.TierJson, j.DivisionJson), (i.TierId, j.DivisionId)))
        |> Map.ofSeq

    let map = buildRankMap

    member x.RankMap = map

type RoleAndLane() =
    let (laneQuery, roleQuery) =
        use db = new SgdbContext()

        let lanes =
            query { for lane in db.Lanes do
                        select lane }
            |> Seq.toList

        let roles =
            query { for role in db.Roles do
                        select role }
            |> Seq.toList
        (lanes, roles)
    
    let lanesById =
        laneQuery
        |> List.map (fun lane -> lane.LaneId, lane)
        |> Map.ofList

    let lanesByJson =
        laneQuery
        |> List.map (fun lane -> lane.LaneJson, lane)
        |> Map.ofList

    let rolesByJson =
        roleQuery
        |> List.map (fun role -> role.RoleJson, role)
        |> Map.ofList

    let rolesById =
        roleQuery
        |> List.map (fun role -> role.RoleId, role)
        |> Map.ofList

    member x.LaneOrDefault(laneJson, valueOrDefault) =
        match lanesByJson.TryFind laneJson with
        | Some x -> x.LaneName
        | None -> valueOrDefault

    member x.RoleOrDefault(laneJson, valueOrDefault) =
        match rolesByJson.TryFind laneJson with
        | Some x -> x.Description
        | None -> valueOrDefault

    member x.LaneOrNull(laneId) =
        match lanesById.TryFind laneId with
        | Some x -> x
        | None -> null

    member x.RoleOrNull(roleId) =
        match rolesById.TryFind roleId with
        | Some x -> x
        | None -> null