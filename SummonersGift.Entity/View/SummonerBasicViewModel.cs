
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

        public SummonerBasicViewModel(int id, string name, long revisionDate, int profileIconId, int summonerLevel)
        {
            Id = id;
            Name = name;
            RevisionDate = Utils.ConvertFromEpochMilliseconds(revisionDate);
            ProfileIconLocation = "";
            SummonerLevel = summonerLevel;
        }

        public SummonerBasicViewModel(Summoner_1_4 summoner) :
            this(summoner.Id, summoner.Name, summoner.RevisionDate, summoner.ProfileIconId, summoner.SummonerLevel)
        {
        }
    }
}
