namespace SummonersGift.Data

open System

open SummonersGift.Models
open SummonersGift.Models.Riot

module Summoner =

    let buildFullViewModel(basics, stats, matches : Match seq) =

        let matchLength = matches |> Seq.length

        let recentGames = min matchLength 20
        let recentWins =
            matches
            |> Seq.take recentGames
            |> Seq.filter (fun i -> i.Participants.[0].Stats.Winner)
            |> Seq.length
            

        let matchTimes =
            matches
            |> Seq.map (fun i -> Utils.ConvertFromEpochMilliseconds i.MatchCreation)
            |> Seq.toList
        
        let averageHour =
            matchTimes
            |> List.averageBy (fun i -> float(i.Hour))

        let weekendRate =
            matchTimes
            |> List.filter (fun i -> i.DayOfWeek = DayOfWeek.Saturday || i.DayOfWeek = DayOfWeek.Sunday)
            |> List.length
            |> fun i -> (float i) / (float matchLength)

        let twoWeeksAgo = System.DateTime.UtcNow.AddDays(-14.0)

        let gamesInFortnight =
            matchTimes
            |> List.filter (fun i -> i > twoWeeksAgo)
            |> List.length

        SummonersGift.Models.View.SummonerFullViewModel(basics, stats, matches, recentGames, recentWins,
            gamesInFortnight, averageHour, weekendRate)