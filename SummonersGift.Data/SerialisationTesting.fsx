

#load "Scripts\load-references.fsx"

open SummonersGift.Models.Riot

let s = new StackExchange.Redis.Extensions.Protobuf.ProtobufSerializer()

type MyRec = { Name: string; Age: int }

let m = { Name = "Max"; Age = 25 }
let m2 = { Name = "Andrew"; Age = 25 }

let t = s.Serialize(m)
let t2 = s.Serialize(m2)

//let b : MyRec = s.Deserialize(t)

s.Deserialize<MyRec>(t)

let exampleDir = __SOURCE_DIRECTORY__ + @"..\..\ExampleJson\"
let exampleFile = exampleDir + "game-v1.3.json"

let json = System.IO.File.ReadAllText(exampleFile)

let g = Newtonsoft.Json.JsonConvert.DeserializeObject<Game_1_3>(json)

let t3 = s.Serialize(g)

let a = g.Games.[0].Stats
s.Serialize(a)

t3.Length