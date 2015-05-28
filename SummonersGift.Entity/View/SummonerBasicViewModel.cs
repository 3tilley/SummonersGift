
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SummonersGift.Models.Riot;

namespace SummonersGift.Models.View
{
    public class SummonerBasicViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime RevisionDate { get; set; }
        public string ProfileIconLocation { get; set; }
        public int SummonerLevel { get; set; }
        public bool IsUnranked { get; set; }
        public string Tier { get; set; }
        public string Division { get; set; }
        public string DisplayRank { get; set; }

        public SummonerBasicViewModel(int id, string name, long revisionDate, int profileIconId, int summonerLevel, string tier, string division)
        {
            var isUnranked = false;
            var displayRank = "UNRANKED";
            if((tier==null) | (division==null))
            {
                tier = null;
                division = null;
            }
            else
            {
                displayRank = tier + (((tier == "CHALLENGER") | (tier == "MASTER")) ? "" : (" " + division));
            }
            Id = id;
            Name = name;
            RevisionDate = Utils.ConvertFromEpochMilliseconds(revisionDate);
            ProfileIconLocation = "";
            SummonerLevel = summonerLevel;
            IsUnranked = isUnranked;
            Tier = tier;
            Division = division;
            DisplayRank = displayRank;
        }

        public SummonerBasicViewModel(Summoner_1_4 summoner, string tier, string division) :
            this(summoner.Id, summoner.Name, summoner.RevisionDate, summoner.ProfileIconId, summoner.SummonerLevel, tier, division)
        {
        }
    }
}
