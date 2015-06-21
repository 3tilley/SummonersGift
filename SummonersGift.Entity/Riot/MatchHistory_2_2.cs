
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SummonersGift.Models.Riot
{
    public class MatchHistory_2_2
    {
        public List<Match> Matches { get; set; }
    }

    public class CreepsPerMinDeltas
    {
        public double ZeroToTen { get; set; }
        public double TenToTwenty { get; set; }
        public double TwentyToThirty { get; set; }
        public double ThirtyToEnd { get; set; }
    }

    public class XpPerMinDeltas
    {
        public double ZeroToTen { get; set; }
        public double TenToTwenty { get; set; }
        public double TwentyToThirty { get; set; }
        public double ThirtyToEnd { get; set; }
    }

    public class GoldPerMinDeltas
    {
        public double ZeroToTen { get; set; }
        public double TenToTwenty { get; set; }
        public double TwentyToThirty { get; set; }
        public double ThirtyToEnd { get; set; }
    }

    public class CsDiffPerMinDeltas
    {
        public double ZeroToTen { get; set; }
        public double TenToTwenty { get; set; }
        public double TwentyToThirty { get; set; }
        public double ThirtyToEnd { get; set; }
    }

    public class XpDiffPerMinDeltas
    {
        public double ZeroToTen { get; set; }
        public double TenToTwenty { get; set; }
        public double TwentyToThirty { get; set; }
        public double ThirtyToEnd { get; set; }
    }

    public class DamageTakenPerMinDeltas
    {
        public double ZeroToTen { get; set; }
        public double TenToTwenty { get; set; }
        public double TwentyToThirty { get; set; }
        public double ThirtyToEnd { get; set; }
    }

    public class DamageTakenDiffPerMinDeltas
    {
        public double ZeroToTen { get; set; }
        public double TenToTwenty { get; set; }
        public double TwentyToThirty { get; set; }
        public double ThirtyToEnd { get; set; }
    }

    public class Timeline
    {
        public CreepsPerMinDeltas CreepsPerMinDeltas { get; set; }
        public XpPerMinDeltas XpPerMinDeltas { get; set; }
        public GoldPerMinDeltas GoldPerMinDeltas { get; set; }
        public CsDiffPerMinDeltas CsDiffPerMinDeltas { get; set; }
        public XpDiffPerMinDeltas XpDiffPerMinDeltas { get; set; }
        public DamageTakenPerMinDeltas DamageTakenPerMinDeltas { get; set; }
        public DamageTakenDiffPerMinDeltas DamageTakenDiffPerMinDeltas { get; set; }
        public string Role { get; set; }
        public string Lane { get; set; }
    }

    public class Mastery
    {
        public int MasteryId { get; set; }
        public int Rank { get; set; }
    }

    public class MatchHistoryStats
    {
        public bool Winner { get; set; }
        public int ChampLevel { get; set; }
        public int Item0 { get; set; }
        public int Item1 { get; set; }
        public int Item2 { get; set; }
        public int Item3 { get; set; }
        public int Item4 { get; set; }
        public int Item5 { get; set; }
        public int Item6 { get; set; }
        public int Kills { get; set; }
        public int DoubleKills { get; set; }
        public int TripleKills { get; set; }
        public int QuadraKills { get; set; }
        public int PentaKills { get; set; }
        public int UnrealKills { get; set; }
        public int LargestKillingSpree { get; set; }
        public int Deaths { get; set; }
        public int Assists { get; set; }
        public int TotalDamageDealt { get; set; }
        public int TotalDamageDealtToChampions { get; set; }
        public int TotalDamageTaken { get; set; }
        public int LargestCriticalStrike { get; set; }
        public int TotalHeal { get; set; }
        public int MinionsKilled { get; set; }
        public int NeutralMinionsKilled { get; set; }
        public int NeutralMinionsKilledTeamJungle { get; set; }
        public int NeutralMinionsKilledEnemyJungle { get; set; }
        public int GoldEarned { get; set; }
        public int GoldSpent { get; set; }
        public int CombatPlayerScore { get; set; }
        public int ObjectivePlayerScore { get; set; }
        public int TotalPlayerScore { get; set; }
        public int TotalScoreRank { get; set; }
        public int MagicDamageDealtToChampions { get; set; }
        public int PhysicalDamageDealtToChampions { get; set; }
        public int TrueDamageDealtToChampions { get; set; }
        public int VisionWardsBoughtInGame { get; set; }
        public int SightWardsBoughtInGame { get; set; }
        public int MagicDamageDealt { get; set; }
        public int PhysicalDamageDealt { get; set; }
        public int TrueDamageDealt { get; set; }
        public int MagicDamageTaken { get; set; }
        public int PhysicalDamageTaken { get; set; }
        public int TrueDamageTaken { get; set; }
        public bool FirstBloodKill { get; set; }
        public bool FirstBloodAssist { get; set; }
        public bool FirstTowerKill { get; set; }
        public bool FirstTowerAssist { get; set; }
        public bool FirstInhibitorKill { get; set; }
        public bool FirstInhibitorAssist { get; set; }
        public int InhibitorKills { get; set; }
        public int TowerKills { get; set; }
        public int WardsPlaced { get; set; }
        public int WardsKilled { get; set; }
        public int LargestMultiKill { get; set; }
        public int KillingSprees { get; set; }
        public int TotalUnitsHealed { get; set; }
        public int TotalTimeCrowdControlDealt { get; set; }
    }

    public class Rune
    {
        public int RuneId { get; set; }
        public int Rank { get; set; }
    }

    public class Participant
    {
        public int TeamId { get; set; }
        public int Spell1Id { get; set; }
        public int Spell2Id { get; set; }
        public int ChampionId { get; set; }
        public string HighestAchievedSeasonTier { get; set; }
        public Timeline Timeline { get; set; }
        public List<Mastery> Masteries { get; set; }
        public MatchHistoryStats Stats { get; set; }
        public int ParticipantId { get; set; }
        public List<Rune> Runes { get; set; }
    }

    public class Player
    {
        public int SummonerId { get; set; }
        public string SummonerName { get; set; }
        public string MatchHistoryUri { get; set; }
        public int ProfileIcon { get; set; }
    }

    public class ParticipantIdentity
    {
        public int ParticipantId { get; set; }
        public Player Player { get; set; }
    }

    public class Match
    {
        public Int64 MatchId { get; set; }
        public string Region { get; set; }
        public string PlatformId { get; set; }
        public string MatchMode { get; set; }
        public string MatchType { get; set; }
        public long MatchCreation { get; set; }
        public int MatchDuration { get; set; }
        public string QueueType { get; set; }
        public int MapId { get; set; }
        public string Season { get; set; }
        public string MatchVersion { get; set; }
        public List<Participant> Participants { get; set; }
        public List<ParticipantIdentity> ParticipantIdentities { get; set; }
    }
}
