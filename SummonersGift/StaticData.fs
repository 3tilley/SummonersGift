namespace SummonersGift.Data

type StaticData(version) =

    let champMap = Champions(version)
    member x.ChampNameOrDefault(id, defaultValue) =
        champMap.ChampNameOrDefault(id, defaultValue)
    

