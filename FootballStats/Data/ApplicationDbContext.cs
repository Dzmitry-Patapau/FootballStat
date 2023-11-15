using FootballStats.Data.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FootballStats.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Referee> Referees { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<MatchStatistics> MatchStatistics { get; set; }
        public DbSet<TeamStatistics> TeamStatistics { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Match>()
                .HasOne(m => m.Referee)
                .WithMany()
                .HasForeignKey(m => m.RefereeID);

            builder.Entity<Match>()
                .HasOne(m => m.League)
                .WithMany()
                .HasForeignKey(m => m.LeagueID);

            builder.Entity<Match>()
                .HasOne(m => m.HomeTeam)
                .WithMany()
                .HasForeignKey(m => m.HomeTeamID);

            builder.Entity<Match>()
                .HasOne(m => m.AwayTeam)
                .WithMany()
                .HasForeignKey(m => m.AwayTeamID);

            builder.Entity<MatchStatistics>()
                .HasOne(ms => ms.Match)
                .WithOne()
                .HasForeignKey<MatchStatistics>(ms => ms.MatchID);

            builder.Entity<TeamStatistics>()
                .HasOne(ts => ts.Team)
                .WithMany()
                .HasForeignKey(ts => ts.TeamID);

            builder.Entity<TeamStatistics>()
                .HasOne(ts => ts.League)
                .WithMany()
                .HasForeignKey(ts => ts.LeagueID);
            
            base.OnModelCreating(builder);
        }
    }
}