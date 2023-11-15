namespace FootballStats.Models
{
    public class MatchWithStatisticsViewModel
    {
        public string LeagueName { get; set; }
        public string HomeTeam { get; set; }
        public string? LogoHome { get; set; }
        public string? LogoAway { get; set; }
        public string AwayTeam { get; set; }
        public int MatchRound { get; set; }
        public DateTime MatchDateTime { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public string Referee { get; set; }
        public MatchStatisticsViewModel Statistics { get; set; }
    }

    public class MatchStatisticsViewModel
    {
        public int CornersHome { get; set; }
        public int CornersAway { get; set; }
        public int YellowCardsHome { get; set; }
        public int YellowCardsAway { get; set; }
        public int RedCardsHome { get; set; }
        public int RedCardsAway { get; set; }
    }

    public enum StatisticType
    {
        Referee,
        League,
        Team,
        Match
    }
}
