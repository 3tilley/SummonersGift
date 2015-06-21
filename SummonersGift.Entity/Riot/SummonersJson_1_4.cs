
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.FSharp.Collections;
using ProtoBuf;

namespace SummonersGift.Models.Riot
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
    public class Summoner_1_4
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProfileIconId { get; set; }
        public int SummonerLevel { get; set; }
        public long RevisionDate { get; set; }
    }
}
