
namespace SummonersGift.Data

module Redis =
    
    let normaliseSummonerName (name : string) = 
        name.ToLower().Replace(" ","")

    let summonerKey name = "summoner:" + (normaliseSummonerName name)
    let followSummonersKey = "followSummoners"
    let normalGamesKey summonerId = "normals:" + summonerId
    //let normalsStoredKey summonerId = "normalsStored:" + summonerId