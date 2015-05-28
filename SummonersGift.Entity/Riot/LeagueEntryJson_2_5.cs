using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonersGift.Models.Riot
{
    public class MiniSeries
    {
        public string Progress { get; set; }
        public int Target { get; set; }
        public int Losses { get; set; }
        public int Wins { get; set; }
    }

    public class Entry
    {
        public int LeaguePoints { get; set; }
        public bool IsFreshBlood { get; set; }
        public bool IsHotStreak { get; set; }
        public string Division { get; set; }
        public bool IsInactive { get; set; }
        public bool IsVeteran { get; set; }
        public string PlayerOrTeamName { get; set; }
        public string PlayerOrTeamId { get; set; }
        public int Wins { get; set; }
        public MiniSeries MiniSeries { get; set; }
    }

    public class LeagueJson_2_5
    {
        public string Queue { get; set; }
        public string Name { get; set; }
        public string ParticipantId { get; set; }
        public List<Entry> Entries { get; set; }
        public string Tier { get; set; }
    }
}
