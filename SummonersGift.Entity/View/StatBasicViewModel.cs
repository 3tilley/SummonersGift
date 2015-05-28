using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonersGift.Models.View
{
    public class StatBasicViewModel
    {
        public byte StatId { get; set; }
        public string StatName { get; set; }
        public double SampleMean { get; set; }
        public double SummonerMean { get; set; }

        public StatBasicViewModel(byte statId, string statName, double sampleMean, double summonerMean)
        {
            StatId = statId;
            StatName = statName;
            SampleMean = sampleMean;
            SummonerMean = summonerMean;
        }
    }
}
