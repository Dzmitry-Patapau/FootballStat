using FootballStats.Data.Identity;
using FootballStats.Models;

namespace FootballStats.Services.Repository
{
    public interface IRepository
    {
        Task<IEnumerable<Team>> GetTeamsForLeague(int leagueId);
        Task<IEnumerable<Match>> GetMatchesForReferee(int refereeId);
        Task<IEnumerable<League>> GetLeaguesForTeam(int teamId);
        Task<IEnumerable<Match>> GetMatchesForTeamInLeague(int teamId, int leagueId);
        MatchStatistics GetMatchStatisticsForMatch(int matchId);
        Task<IEnumerable<Match>> GetMatchesForTeam(int teamId);
        Task<IEnumerable<Match>> GetMatchesForLeague(int leagueId);
        Task<IEnumerable<object>> GetMatchesWithStaticticsApi(IEnumerable<Match> matches);
        Task<IEnumerable<TeamStatistics>> GetTeamsStatisticsForLeague(int leagueId);
        Task<IEnumerable<object>> GetTeamsStatisticsForLeagueApi(int leagueId);
        Task<IEnumerable<Match>> GetAllMatches();
        Task<object> GetMatchWithStatisticsApi(int matchId);
        Task<IEnumerable<MatchWithStatisticsViewModel>> GetMatchesWithStatictics(IEnumerable<Match> matches);
        MatchStatisticsViewModel GetMatchStatisticsMatch(int matchID);
    }
}
