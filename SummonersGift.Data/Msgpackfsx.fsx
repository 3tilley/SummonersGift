
#r @"C:\Users\Max\Documents\GitHub\SummonersGift\packages\MsgPack.0.1.0.2011042300\lib\net40\MsgPack.dll"

let matchesPath = @"C:\Users\Max\Documents\GitHub\SummonersGift\Data\Matches\AprilData\Serialised\"

use st = new System.IO.StreamReader(matchesPath + "lightMatches.msgpack")

let m = MsgPack.MsgPackReader(st.BaseStream)

m.Read()
m.Length

