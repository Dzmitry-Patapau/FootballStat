using FootballStats.Data.Identity;

namespace FootballStats.Models
{
    public class LeagueDetailsViewModel
    {
        public League League { get; set; }
        public IEnumerable<Team> Teams { get; set; }
        public IEnumerable<Match> Matches { get; set; }
    }
}
