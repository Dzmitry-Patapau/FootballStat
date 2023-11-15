using FootballStats.Data.Identity;

namespace FootballStats.Models
{
    public class RefereeDetailsViewModel
    {
        public Referee Referee { get; set; }
        public IEnumerable<Match> Matches { get; set; }
    }
}
