using FootballStats.Data.Identity;

namespace FootballStats.Models
{
    public class TeamDetailsViewModel
    {
        public Team Team { get; set; }
        public IEnumerable<League> Leagues { get; set; }
        public IEnumerable<Match> Matches { get; set; }
    }
}
