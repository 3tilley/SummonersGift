
import os
import json

import pandas as pd
import numpy as np
import matplotlib
import matplotlib.pyplot as plt

cats = ["region",
    "matchType",
    "platformId",
    "matchMode",
    "matchVersion",
    "season",
    "queueType"]
    
toPop = ["matchType",
    "matchMode",
    "matchDuration",
    "season",
    "participants",
    "participantIdentities",
    "teams",
    "timeline"]
    
summonerColumnsToMakeCategories = [("division", ["I","II","III","IV","V"][::-1]),
                               ("tier", ["BRONZE", "SILVER","GOLD","PLATINUM","DIAMOND","MASTER","CHALLENGER"])]

def sumCats(df, cats=summonerColumnsToMakeCategories):
    for c,o in cats:
        df[c] = df[c].astype("category", categories=o, ordered=True)

### Need to optimise this, close files and delete json?
def loadMatches(matchIds, matchFolder, columnsToMakeCategories=cats, fileExtension="",
                    dropTimeline=True, lightMatches=False, popCats=None, showProgress=True):
    counter = False
    idsLen = len(matchIds)
    counts = None
    if (idsLen >= 1000) and showProgress:
        counter = True
        counts = [matchIds.iloc[int((i / 10.0) * idsLen)] for i in range(1,10) ]
    
    assert (not ((not lightMatches) and (popCats is not None))), "If popCats is specified, lightMatches must be True"
    if popCats is None and lightMatches:
        popCats = toPop
    dataDict = []
    
    nextId = None
    percentage = None
    if counter:
        nextId = counts[0]
        progress = 1
    for f in matchIds:
        with open(matchFolder + "\\" + str(f) + fileExtension) as fp:
            jsonObj = json.load(fp)
            if dropTimeline and not lightMatches:
                jsonObj.pop("timeline")
            if lightMatches:
                for j in popCats: jsonObj.pop(j)
            dataDict.append(jsonObj)
        if counter:
            if f == nextId:
                print("{0}% complete".format(progress * 10.0))
                if progress != 9:
                    nextId = counts[progress]
                progress += 1
    df = pd.DataFrame(dataDict)
    if lightMatches:
        lightCats = set(columnsToMakeCategories).difference(set(popCats))
        columnsToMakeCategories = lightCats
    for c in columnsToMakeCategories:
        df[c] = df[c].astype("category")
    return df

def loadSummoners(summonerFolder, dropMiniSeries=True):
    fileNames = os.listdir(summonerFolder)
    dataDict = []
    for f in fileNames:
                with open(summonerFolder + "\\" + f) as fp:
                    jsonObj = json.load(fp)
                    for i,v in jsonObj.items():
                        for j in v:
                            if j["queue"] == "RANKED_SOLO_5x5":
                                lst = {"tier":j["tier"], "name":j["name"]}
                                lst.update(j["entries"][0])
                                dataDict.append(lst)
                    fp.close()
    df = pd.DataFrame(dataDict).convert_objects(convert_numeric=True)
    for c,o in summonerColumnsToMakeCategories:
        df[c] = df[c].astype("category", categories=o, ordered=True)
    if dropMiniSeries:
        return df.drop("miniSeries", axis=1)
    else:
        return df
        
def pullSummonersFromGames(matchData):
    summonerInfo = {"matchId":[], "highestAchievedSeasonTier":[], "participantId":[], "data":[]}
    summonerIds = {"matchId":[], "participantId":[], "summonerId":[]}
    def addToSummonerInfo(sx):
        s = sx["participants"]
        ids = sx["participantIdentities"]
        for i in ids:
            summonerIds["matchId"].append(sx["matchId"])
            
            summonerIds["participantId"].append(i["participantId"])
            summonerIds["summonerId"].append(i["player"]["summonerId"])
        for j in s:
            summonerInfo["matchId"].append(sx["matchId"])
            summonerInfo["highestAchievedSeasonTier"].append(j["highestAchievedSeasonTier"])
            summonerInfo["participantId"].append(j["participantId"])
            summonerInfo["data"].append(j)
            
    for i, r in matchData.iterrows():
        addToSummonerInfo(r)
    dfs = pd.DataFrame(summonerInfo)
    dfs2 = pd.DataFrame(summonerIds)
    return dfs.merge(dfs2, how="left",on=["matchId","participantId"])        

def plotTimeCreation(plotCustoms=True, defaultColour="blue", colourMap=None):
    gs = df.groupby("queueType")
    fig = plt.figure(figsize=(20,20))
    ax = fig.add_subplot(1,1,1)

    b = 0.0
    cmap=plt.get_cmap("Accent")

    for i,j in gs:
        if colourMap:
            col = colourMap[i]
        else:
            col = "red" if i=="RANKED_SOLO_5x5" else defaultColour
        if plotCustoms or (i!="CUSTOM"):
            plt.scatter(j["matchCreation"], j["matchId"], s=5, c=col, alpha=1.0, edgecolor="", label=i)
        b += 1

    xt = ax.get_xticks()
    labels = ax.set_xticklabels([datetime.datetime.fromtimestamp(i / 1000.0).strftime("%H:%M") for i in xt], rotation=90)
    ax.legend()
    
waves = range(1,101)
creepCount = [ 7 if i % 3 == 0 else 6 for i in waves ]
spawnedCreeps = np.array(creepCount).cumsum()

def laneCsAtMinute(min, mid=False):
    offset = 0.5 if mid else (55.0 / 60.0)
    return spawnedCreeps[int((min - 1.5 - offset) * 2.0)]
    
def prepareCleanRanked(lightMatchesFile, matchesFolder, summonersFolder):
    # Pick up light matches to see which ones are ranked
    df = None
    if type(lightMatchesFile) == str:
        df = pd.read_pickle(lightMatchesFile)
    elif type(lightMatchesFile) == pd.core.frame.DataFrame:
        df = lightMatchesFile
    # Load full ranked matches
    df = loadMatches(df[(df.queueType == "RANKED_SOLO_5x5")].matchId, matchesFolder, fileExtension=".json")
    # Get the summoner names
    dfs = pullSummonersFromGames(df)
    # Get the summoners for the games we have
    dfs2 = loadSummoners(summonersFolder)
    
    d = dfs["data"].apply(pd.Series)
    df = pd.concat([dfs, d], axis=1, copy=False).drop(["data", "masteries", "runes", "highestAchievedSeasonTier"], axis=1)
    
    df = pd.concat([df, df.timeline.apply(pd.Series)], axis=1).merge(dfs2, "inner", left_on="summonerId", right_on="playerOrTeamId",copy=False)
    
    # Work out if lanes are standard
    a = df[["matchId", "teamId","lane", "role"]].copy()
    a["laneAndRole"] = a.lane + "_" + a.role
    gs = a.groupby(["matchId", "teamId"])
    rs = gs.aggregate(lambda s: " ".join(list(np.sort(s))))["laneAndRole"].reset_index()
    rs["isStandardLanesTeam"] = np.equal(rs.laneAndRole, "BOTTOM_DUO_CARRY BOTTOM_DUO_SUPPORT JUNGLE_NONE MIDDLE_SOLO TOP_SOLO")
    rs["isStandardLanesGame"] = rs.groupby("matchId")["isStandardLanesTeam"].transform(np.alltrue)
    rs = rs.drop("laneAndRole", axis=1)
    
    # Recombine data
    df = df.merge(rs, "inner", left_on=["matchId", "teamId"], right_on=["matchId", "teamId"], copy=False)
    
    sumCats(df)
    
    return df
    