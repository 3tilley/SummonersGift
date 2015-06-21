using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonersGift.Models.Riot
{

    public class FellowPlayer
    {
        public int ChampionId { get; set; }
        public int TeamId { get; set; }
        public int SummonerId { get; set; }
    }

    public class RecentGameStats
    {
        public int TotalDamageDealtToChampions { get; set; }
        public int Item2 { get; set; }
        public int GoldEarned { get; set; }
        public int Item1 { get; set; }
        public int WardPlaced { get; set; }
        public int Item0 { get; set; }
        public int TotalDamageTaken { get; set; }
        public int TrueDamageDealtPlayer { get; set; }
        public int PhysicalDamageDealtPlayer { get; set; }
        public int TrueDamageDealtToChampions { get; set; }
        public int KillingSprees { get; set; }
        public int PlayerRole { get; set; }
        public int TotalUnitsHealed { get; set; }
        public int PlayerPosition { get; set; }
        public int Level { get; set; }
        public int DoubleKills { get; set; }
        public int TripleKills { get; set; }
        public int NeutralMinionsKilledYourJungle { get; set; }
        public int MagicDamageDealtToChampions { get; set; }
        public int TurretsKilled { get; set; }
        public int MagicDamageDealtPlayer { get; set; }
        public int NeutralMinionsKilledEnemyJungle { get; set; }
        public int Assists { get; set; }
        public int MagicDamageTaken { get; set; }
        public int NumDeaths { get; set; }
        public int TotalTimeCrowdControlDealt { get; set; }
        public int LargestMultiKill { get; set; }
        public int PhysicalDamageTaken { get; set; }
        public int Team { get; set; }
        public bool Win { get; set; }
        public int TotalDamageDealt { get; set; }
        public int LargestKillingSpree { get; set; }
        public int TotalHeal { get; set; }
        public int Item4 { get; set; }
        public int Item3 { get; set; }
        public int Item6 { get; set; }
        public int Item5 { get; set; }
        public int MinionsKilled { get; set; }
        public int TimePlayed { get; set; }
        public int PhysicalDamageDealtToChampions { get; set; }
        public int ChampionsKilled { get; set; }
        public int TrueDamageTaken { get; set; }
        public int QuadraKills { get; set; }
        public int GoldSpent { get; set; }
        public int NeutralMinionsKilled { get; set; }
        public int? LargestCriticalStrike { get; set; }
        public int? WardKilled { get; set; }
        public int? BarracksKilled { get; set; }
    }

    public class Game
    {
        public List<FellowPlayer> FellowPlayers { get; set; }
        public string GameType { get; set; }
        public RecentGameStats Stats { get; set; }
        public object GameId { get; set; }
        public int IpEarned { get; set; }
        public int Spell1 { get; set; }
        public int TeamId { get; set; }
        public int Spell2 { get; set; }
        public string GameMode { get; set; }
        public int MapId { get; set; }
        public int Level { get; set; }
        public bool Invalid { get; set; }
        public string SubType { get; set; }
        public object CreateDate { get; set; }
        public int ChampionId { get; set; }
    }

    public class Game_1_3
    {
        public List<Game> Games { get; set; }
        public int SummonerId { get; set; }
    }
}
