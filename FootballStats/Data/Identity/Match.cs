namespace FootballStats.Data.Identity
{
    public class Match
    {
        public int MatchID { get; set; }
        public int LeagueID { get; set; }
        public int HomeTeamID { get; set; }
        public int AwayTeamID { get; set; }
        public int MatchRound { get; set; }
        public DateTime MatchDateTime { get; set; }
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public int RefereeID { get; set; }


        public Referee? Referee { get; set; }
        public League? League { get; set; }
        public Team? HomeTeam { get; set; }
        public Team? AwayTeam { get; set; }
    }
}
