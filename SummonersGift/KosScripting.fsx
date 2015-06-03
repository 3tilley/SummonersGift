#r @"..\packages\FSharp.Data.2.1.0\lib\net40\FSharp.Data.dll"
#r @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.dll"
#r @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Core.dll"
#r @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Numerics.dll"
#r @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Web.dll"
#r @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.5\System.Xml.Linq.dll"
#r @"..\packages\Newtonsoft.Json.6.0.6\lib\net45\Newtonsoft.Json.dll"

#load "Utils.fs"
#load "ApiKeys.fs"
#load "FetchFunctions.fs"
#load "RiotData.fs"

open SummonersGift.Data

let fetch = DataFetcher([ApiKeys.keys.[0]])

let a = fetch.GetSummonerId("EUW", "Proheme")


let myList = System.Collections.Generic.List<_>(["a"; "b"; "c"])


myList.Reverse()
myList

let myList2 = System.Collections.Generic.List<string>([])

myList2.Reverse()