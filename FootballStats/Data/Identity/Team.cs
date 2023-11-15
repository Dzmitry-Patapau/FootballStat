namespace FootballStats.Data.Identity
{
    public class Team
    {
        public int TeamID { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
        public string Stadium { get; set; }
        public string? LogoStadium {  get; set; }
    }
}
