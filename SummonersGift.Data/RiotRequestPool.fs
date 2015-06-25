namespace SummonersGift.Data

module RiotRequestPool =
    
    type IRequestPool =
        abstract member GetRiotRequest : unit -> Async<unit>

    type BasicPool(reqsPerSecond) =
        let delay = int(1000.0 / reqsPerSecond)
        interface IRequestPool with
            member this.GetRiotRequest() =
                Async.Sleep(delay)

    type Message =
        | Request of AsyncReplyChannel<unit>
        | Update

    type MailboxPool(reqsInTenSeconds) =
        let delay = int(10.5 * 1000.0)

        let mb = new MailboxProcessor<Message>(fun inbox ->

            let waitAndRepopulate () =
                async {
                    do! Async.Sleep delay
                    inbox.Post(Update)
                    }

            let rec loop reqs awaitingReqs =
                async {
                    let a = sprintf "%i requests available, %i jobs in queue" reqs (awaitingReqs |> List.length)
                    System.Diagnostics.Trace.TraceInformation(a)
                    let! message = inbox.Receive()
                    match message with
                    | Request replyChannel ->
                        match reqs with
                        | x when x > 0 ->
                            replyChannel.Reply()
                            Async.Start (waitAndRepopulate())
                            return! loop (reqs-1) awaitingReqs
                        | 0 ->
                            return! loop 0 (replyChannel::awaitingReqs)
                        | x -> failwith "Request pool has dropped to: %i" x
                    | Update ->
                        match awaitingReqs with
                        | [] ->
                            return! loop (reqs + 1) []
                        | hd::tl ->
                            hd.Reply()
                            Async.Start (waitAndRepopulate())
                            return! loop reqs tl
                }
            loop reqsInTenSeconds [])

        do
            mb.Start()

        interface IRequestPool with
            member this.GetRiotRequest() =
                mb.PostAndAsyncReply(fun i -> Request i)

