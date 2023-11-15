using FootballStats.Data;
using FootballStats.Services.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace FootballStats.Controllers
{
    [Route("api")]
    [ApiController]
    public class FootballApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository _repository;

        public FootballApiController(ApplicationDbContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        // Вывод всех игр команды по идентификатору команды
        [HttpGet("team/{teamId}/matches")]
        public async Task<IActionResult> GetTeamMatchesAsync(int teamId)
        {
            try
            {
                var matches = await _repository.GetMatchesForTeam(teamId);

                if (matches == null || !matches.Any())
                {
                    return NotFound($"No matches found for team with ID {teamId}");
                }

                var matchesWithStatistics = await _repository.GetMatchesWithStaticticsApi(matches);

                return Ok(matchesWithStatistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Вывод всех игр судьи по идентификатору судьи
        [HttpGet("referee/{refereeId}/matches")]
        public async Task<IActionResult> GetRefereeMatchesAsync(int refereeId)
        {
            try
            {
                var matches = await _repository.GetMatchesForReferee(refereeId);

                if (matches == null || !matches.Any())
                {
                    return NotFound($"No matches found for team with ID {refereeId}");
                }

                var matchesWithStatistics = await _repository.GetMatchesWithStaticticsApi(matches);

                return Ok(matchesWithStatistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Вывод всех игр лиги по идентификатору лиги
        [HttpGet("league/{leagueId}/matches")]
        public async Task<IActionResult> GetLeagueMatchesAsync(int leagueId)
        {
            try
            {
                var matches = await _repository.GetMatchesForLeague(leagueId);

                if (matches == null || !matches.Any())
                {
                    return NotFound($"No matches found for team with ID {leagueId}");
                }

                var matchesWithStatistics = await _repository.GetMatchesWithStaticticsApi(matches);

                return Ok(matchesWithStatistics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Вывод матча и его статистики
        [HttpGet("match/{matchId}/matchStatistics")]
        public async Task<IActionResult> GetMatchMatchStatisticsAsync(int matchId)
        {
            try
            {
                var match = await _repository.GetMatchWithStatisticsApi(matchId);

                if (match == null)
                {
                    return NotFound($"No matches found for team with ID {matchId}");
                }
                return Ok(match);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

        // Вывод статистики выступлений всех команд выбранной лиги
        // https://localhost:7001/api/league/4/Statistics 
        [HttpGet("league/{leagueId}/Statistics")]
        public async Task<IActionResult> GetLeagueStatisticsAsync(int leagueId)
        {
            try
            {
                var matches = await _repository.GetTeamsStatisticsForLeagueApi(leagueId);

                if (matches == null || !matches.Any())
                {
                    return NotFound($"No matches found for team with ID {leagueId}");
                }

                return Ok(matches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }

    }
}
