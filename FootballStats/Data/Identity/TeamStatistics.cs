using System.ComponentModel.DataAnnotations;

namespace FootballStats.Data.Identity
{
    public class TeamStatistics
    {
        [Key]
        public int TeamStatID { get; set; }
        public int TeamID { get; set; }
        public int LeagueID { get; set; }
        public int MatchesPlayed { get; set; }
        public int Wins { get; set; }
        public int Draws { get; set; }
        public int Losses { get; set; }
        public int GoalsScored { get; set; }
        public int GoalsConceded { get; set; }
        public int Points { get; set; }

        public Team? Team { get; set; }
        public League? League { get; set; }
    }
}
