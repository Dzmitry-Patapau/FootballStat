using FootballStats.Data.Identity;

namespace FootballStats.Models
{
    public class TeamStaticticsDetailsViewModel
    {
        public TeamStatistics TeamStatistics { get; set; }
        public IEnumerable<Match> Matches { get; set; }
    }
}
