namespace SummonersGift.Data

open System.Net
open Newtonsoft.Json

type DataFetchResult<'T> =
    {
        Success : bool
        Result  : 'T option
        ErrorMessage : string
    }
    with member x.GetResult =
        match x.Success with
        | true -> x.Result.Value
        | false -> failwith "No result available"

module internal ResultHelpers =
    let successfulResult result =
        { Success = true; Result = Some result; ErrorMessage = null }

    let errorResult message =
        { Success = false; Result = None; ErrorMessage = message }

open ResultHelpers
open SummonersGift.Data.Utils
open SummonersGift.Data.Endpoints
open SummonersGift.Data.RiotTypes

type DataFetcher(keys : ApiKey seq) =

    let settings = new JsonSerializerSettings()
    do
        settings.ContractResolver <- new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()

    // Make sure we've only got one key (for now)
    let key = 
        match keys |> Seq.length with
            | 0 -> failwith "Need at least one key"
            | 1 -> keys |> Seq.head
            | x when x >= 2 -> failwith "Multiple keys not supported"

    member public x.GetSummonerId(region, escapedName) =
        let url = buildSummonerNamesUrl region [escapedName] key.Key
        let wc = new System.Net.WebClient()
        let uri = System.Uri(url)
        let cont (resp : System.Threading.Tasks.Task<string>) =
            let jsonObj = JsonConvert.DeserializeObject<SummonersJson_1_4>(resp.Result)
            match jsonObj.TryFind escapedName with
            | Some x -> successfulResult x
            | None -> errorResult "The summoner was not found"
        wc.DownloadStringTaskAsync(uri).ContinueWith(System.Func<_,_>(cont))

