#r @"C:\Users\Maximus\Documents\GitHub\SummonersGift\SummonersGift\packages\FSharp.Data.1.1.10\lib\net40\FSharp.Data.dll"

open System
open System.IO
open System.Net
open System.Net.Security
open System.Security.Cryptography.X509Certificates


let username = "myTestUsername"
let password = "test123"
let url = @"https://lq.eu.lol.riotgames.com/login-queue/rest/queue"

let certHack = System.Net.Security.RemoteCertificateValidationCallback(fun (a:obj) (b:X509Certificate) (c:X509Chain) (d:SslPolicyErrors) -> true)

//System.Net.ServicePointManager.ServerCertificateValidationCallback <- certHack

let request = HttpWebRequest.Create(url+"/authenticate")
request.Method <- "POST"

let innerPostBody = "user=" + System.Web.HttpUtility.UrlEncode(username) + ",password=" + System.Web.HttpUtility.UrlEncode(password)
//let postBody = "payload=" + System.Web.HttpUtility.UrlEncode(innerPostBody)

let postBody = "payload=" + innerPostBody


request.ContentLength <- int64 postBody.Length
let writer = new StreamWriter(request.GetRequestStream())
writer.Write(postBody)
writer.Close()

let response = request.GetResponse()
let streamReader = new StreamReader(response.GetResponseStream())
let json = streamReader.ReadLine()
streamReader.Close()

type login = FSharp.Data.JsonProvider<"""{"rate":140,"token":"7Fh6kXi8bB2fFSGmQ5A-hI6iibC3zAxlHWJRCVRq+y1ie7Y0LpJceA6p+03M-dcyRwyRV6EOc33ZihBlS-icNw==","reason":"login_rate","status":"LOGIN","delay":5000,"inGameCredentials":{"inGame":false,"summonerId":null,"serverIp":null,"serverPort":null,"encryptionKey":null,"handshakeToken":null},"user":"fiboflegends"}""">

let loginData = login.Parse(json)