using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace SummonersGift.Models.View
{
    public class StatRowViewModel
    {
        public byte StatId { get; private set; }
        public string StatName { get; private set; }
        public double SampleMean { get; private set; }
        public double SampleError { get; private set; }
        public double SummonerMean { get; private set; }

        public StatRowViewModel(byte statId, string statName, double sampleMean, double sampleError, double summonerMean)
        {
            StatId = statId;
            StatName = statName;
            SampleMean = sampleMean;
            SummonerMean = summonerMean;
            SampleError = sampleError;
        }
    }

    public class StatTableViewModel
    {
        public IEnumerable<StatRowViewModel> Stats { get; set; }
        public IReadOnlyDictionary<string, string> Filters { get; private set; }
        public int SummonerGames { get; private set; }
        public int DatasetGames { get; private set; }
        public int SummonerWins { get; private set; }

        public StatTableViewModel(IEnumerable<StatRowViewModel> stats, int summonerGames,
                int summonerWins, int datasetGames, IEnumerable<Tuple<string, string>> filters)
        {
            Stats = stats;
            SummonerGames = summonerGames;
            SummonerWins = summonerWins;
            DatasetGames = datasetGames;
            Filters = new ReadOnlyDictionary<string, string>(filters.ToDictionary(x => x.Item1, x => x.Item2));
        }
    }
}
