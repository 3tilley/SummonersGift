using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SummonersGift.Models.Riot;

namespace SummonersGift.Models.View
{
    public class SummonerFullViewModel
    {
        public SummonerBasicViewModel SummonerBasics { get; private set; }
        public IEnumerable<StatTableViewModel> StatTables { get; private set; }
        public IEnumerable<Match> Matches { get; private set; }
        public int NumberOfRecentGames { get; private set; }
        public int WinsInRecentGames { get; private set; }
        public int GamesInLastFortnight { get; private set; }
        public double AverageHour { get; private set; }
        public double PercentageWeekendGames { get; private set; }

        public SummonerFullViewModel(SummonerBasicViewModel basics, IEnumerable<StatTableViewModel> stats,
            IEnumerable<Match> matches, int recentGames, int recentWins, int gamesInLastFortnight,
                double averageHour, double weekendGameRate)
        {
            SummonerBasics = basics;
            StatTables = stats;
            Matches = matches;
            NumberOfRecentGames = recentGames;
            WinsInRecentGames = recentWins;
            GamesInLastFortnight = gamesInLastFortnight;
            AverageHour = averageHour;
            PercentageWeekendGames = weekendGameRate;
        }
    }

}
