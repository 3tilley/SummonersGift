
#r @"C:\Users\Maximus\Documents\GitHub\SummonersGift\SummonersGift\packages\FSharp.Data.1.1.10\lib\net40\FSharp.Data.dll"

open System.Web
open System.Net 
open System.IO

let h = HttpWebRequest.Create("https://lq.eu.lol.riotgames.com/login-queue/rest/queue/authenticate")
h.Method <- "POST"
let user = "myTestUsername"
let pass = "test123"
let body = "payload=user="+user+",password="+pass

let sw = new StreamWriter(h.GetRequestStream())
sw.Write(body)
sw.Close()

let r = h.GetResponse()
let streamReader = new StreamReader(r.GetResponseStream())
let json = streamReader.ReadLine()
streamReader.Close()

type login = FSharp.Data.JsonProvider<"""{"rate":140,"token":"7Fh6kXi8bB2fFSGmQ5A-hI6iibC3zAxlHWJRCVRq+y1ie7Y0LpJceA6p+03M-dcyRwyRV6EOc33ZihBlS-icNw==","reason":"login_rate","status":"LOGIN","delay":5000,"inGameCredentials":{"inGame":false,"summonerId":null,"serverIp":null,"serverPort":null,"encryptionKey":null,"handshakeToken":null},"user":"fiboflegends"}""">

let loginData = login.Parse(json)
loginData.Token
