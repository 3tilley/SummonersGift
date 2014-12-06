
#r @"C:\Users\Andrew\Documents\GitHub\SummonersGift\packages\FSharp.Data.2.1.0\lib\net40\FSharp.Data.dll"
open FSharp.Data 

type SomeProvider = JsonProvider<"..\ExampleJSON\game-v1.3.JSON">

let game = SomeProvider.Load(@"C:\Users\Andrew\Documents\GitHub\SummonersGift\ExampleJSON\game-v1.3.json")

game.Games.[0].ChampionId