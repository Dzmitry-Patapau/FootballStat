using FootballStats.Data;
using FootballStats.Data.Identity;

using FootballStats.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Core.Types;

namespace FootballStats.Services.Repository
{
    public class Repository : IRepository
    {
        public ApplicationDbContext _context { get; }
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Team>> GetTeamsForLeague(int leagueId)
        {
            var teams = await _context.TeamStatistics
                .Include(ts => ts.League)
                .Include(ts => ts.Team)
                .Where(x => x.LeagueID == leagueId)
                .Select(x => x.Team).ToListAsync();

            return teams;
        }

        public async Task<IEnumerable<Match>> GetMatchesForReferee(int refereeId)
        {
            var matchs = await _context.Matches
                .Include(m => m.AwayTeam)
                .Include(m => m.HomeTeam)
                .Include(m => m.League)
                .Include(m => m.Referee)
                .Where(x => x.RefereeID == refereeId).OrderBy(x => x.MatchDateTime).ToListAsync();

            return matchs;
        }

        public async Task<IEnumerable<League>> GetLeaguesForTeam(int teamId)
        {
            var leagues = await _context.TeamStatistics
                .Include(ts => ts.League)
                .Include(ts => ts.Team)
                .Where(x => x.TeamID == teamId)
                .Select(x => x.League).ToListAsync();

            return leagues;
        }

        public async Task<IEnumerable<Match>> GetMatchesForTeamInLeague(int teamId, int leagueId)
        {
            var matches = await _context.Matches
                .Include(m => m.AwayTeam)
                .Include(m => m.HomeTeam)
                .Include(m => m.League)
                .Where(m => m.LeagueID == leagueId && (m.AwayTeamID == teamId || m.HomeTeamID == teamId)).ToListAsync();
            return matches;
        }

        public MatchStatistics GetMatchStatisticsForMatch(int matchId)
        {
            var matchstatistics = _context.MatchStatistics
                .FirstOrDefault(ms => ms.MatchID == matchId);
            return matchstatistics;
        }

        public async Task<IEnumerable<Match>> GetMatchesForTeam(int teamId)
        {
            var matches = await _context.Matches
                .Include(m => m.AwayTeam)
                .Include(m => m.HomeTeam)
                .Include(m => m.League)
                .Include(m => m.Referee)
                .Where(x => (x.AwayTeamID == teamId || x.HomeTeamID == teamId)).OrderBy(x => x.MatchDateTime).ToListAsync();
            return matches;
        }

        public async Task<IEnumerable<Match>> GetMatchesForLeague(int leagueId)
        {
            var matches = await _context.Matches
                .Include(m => m.AwayTeam)
                .Include(m => m.HomeTeam)
                .Include(m => m.League)
                .Include(m => m.Referee)
                .Where(x => x.LeagueID == leagueId).ToListAsync();
            return matches;
        }

        public async Task<IEnumerable<object>> GetMatchesWithStaticticsApi(IEnumerable<Match> matches)
        {
            var matchesWithStatistics = matches.Select(m => new
            {
                LeagueName = m.League.Name,
                HomeTeam = m.HomeTeam.Name,
                AwayTeam = m.AwayTeam.Name,
                m.MatchRound,
                m.MatchDateTime,
                m.HomeTeamScore,
                m.AwayTeamScore,
                Referee = m.Referee.Name,
                Statistics = GetMatchStatisticsForMatchApi(m.MatchID)
            });
            return matchesWithStatistics;
        }

        public async Task<object> GetMatchWithStatisticsApi(int matchId)
        {
            var match = await _context.Matches
                .Include(m => m.AwayTeam)
                .Include(m => m.HomeTeam)
                .Include(m => m.League)
                .Include(m => m.Referee)
                .Where(x => x.MatchID == matchId)
                .FirstOrDefaultAsync();

            object matchWithStatistics = null;

            if (match != null)
            {
                var matchStatistics = GetMatchStatisticsForMatchApi(match.MatchID);

                matchWithStatistics = new
                {
                    LeagueName = match.League.Name,
                    HomeTeam = match.HomeTeam.Name,
                    AwayTeam = match.AwayTeam.Name,
                    match.MatchRound,
                    match.MatchDateTime,
                    match.HomeTeamScore,
                    match.AwayTeamScore,
                    Referee = match.Referee.Name,
                    Statistics = matchStatistics
                };
            }

            return matchWithStatistics;
        }

        public async Task<IEnumerable<TeamStatistics>> GetTeamsStatisticsForLeague(int leagueId)
        {
            var teamsStatistics =await _context.TeamStatistics
                .Include(m => m.League)
                .Include(m => m.Team)
                .Where(ts => ts.LeagueID == leagueId).OrderByDescending(x=>x.Points)
                .ToListAsync();
            return teamsStatistics;
        }
        public async Task<IEnumerable<object>> GetTeamsStatisticsForLeagueApi(int leagueId)
        {
            var stat = await _context.TeamStatistics
                .Include(m => m.League)
                .Include(m => m.Team)
                .Where(ts => ts.LeagueID == leagueId)
                .ToListAsync();
            var liga = _context.Leagues.Where(m=> m.LeagueID == leagueId).Select(m => m.Name ).FirstOrDefault();

            var teamsStatistics = stat.Select(s => new
                {
                    League = liga, 
                    TeamName = _context.Teams.Where(t => t.TeamID == s.TeamID).Select(t => t.Name).FirstOrDefault(),
                    s.MatchesPlayed,
                    s.Wins,
                    s.Draws,
                    s.Losses,
                    s.Points,
                    s.GoalsScored,
                    s.GoalsConceded
                }
            );
            return teamsStatistics;
        }

        public async Task<IEnumerable<Match>> GetAllMatches()
        {
            try
            {
                var matches = await _context.Matches
                    .Include(ts => ts.League)
                    .Include(ts => ts.AwayTeam)
                    .Include(ts => ts.HomeTeam)
                    .Include(ts => ts.Referee)
                    .OrderBy(x => x.MatchDateTime)
                    .ToListAsync();
                return matches;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred in GetAllMatches: {ex.Message}");
                throw; 
            }
        }
        private object GetMatchStatisticsForMatchApi(int matchId)
        {
            var matchstatistics =  _context.MatchStatistics
                        .Where(stat => stat.MatchID == matchId)
                        .Select(stat => new
                        {
                            stat.CornersHome,
                            stat.CornersAway,
                            stat.YellowCardsHome,
                            stat.YellowCardsAway,
                            stat.RedCardsHome,
                            stat.RedCardsAway
                        })
                        .FirstOrDefault();
            return matchstatistics;
        }

        public async Task<IEnumerable<MatchWithStatisticsViewModel>> GetMatchesWithStatictics(IEnumerable<Match> matches)
        {
            var matchesWithStatistics = matches.Select(m => new MatchWithStatisticsViewModel
            {
                LeagueName = m.League.Name,
                LogoAway = m.AwayTeam.Logo,
                LogoHome = m.HomeTeam.Logo,
                HomeTeam = m.HomeTeam.Name,
                AwayTeam = m.AwayTeam.Name,
                MatchRound = m.MatchRound,
                MatchDateTime = m.MatchDateTime,
                HomeTeamScore = m.HomeTeamScore,
                AwayTeamScore = m.AwayTeamScore,
                Referee = m.Referee.Name,
                Statistics = GetMatchStatisticsMatch(m.MatchID)
            });
            return matchesWithStatistics;
        }

        public MatchStatisticsViewModel GetMatchStatisticsMatch(int matchID)
        {
            var matchWithStatistics = _context.MatchStatistics
                .Where(m => m.MatchID == matchID)
                .Select(m => new MatchStatisticsViewModel
                {
                    CornersAway = m.CornersAway,
                    CornersHome = m.CornersHome,
                    YellowCardsAway = m.YellowCardsAway,
                    YellowCardsHome = m.YellowCardsHome,
                    RedCardsAway = m.RedCardsAway,
                    RedCardsHome = m.RedCardsHome
                }
                )
                .FirstOrDefault();

            return matchWithStatistics;
        }


    }
}
