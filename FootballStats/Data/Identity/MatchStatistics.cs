using System.ComponentModel.DataAnnotations;

namespace FootballStats.Data.Identity
{
    public class MatchStatistics
    {
        [Key]
        public int MatchStatID { get; set; }
        public int MatchID { get; set; }
        public int CornersHome { get; set; }
        public int CornersAway { get; set; }
        public int YellowCardsHome { get; set; }
        public int YellowCardsAway { get; set; }
        public int RedCardsHome { get; set; }
        public int RedCardsAway { get; set; }

        public Match? Match { get; set; }
    }
}
