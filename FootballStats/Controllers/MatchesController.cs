using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballStats.Data;
using FootballStats.Data.Identity;
using FootballStats.Data.Migrations;
using FootballStats.Models;
using System.Diagnostics;
using FootballStats.Services.Repository;
using Microsoft.AspNetCore.Authorization;

namespace FootballStats.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class MatchesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository _repository;

        public MatchesController(ApplicationDbContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        // GET: Matches
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Matches.Include(m => m.AwayTeam).Include(m => m.HomeTeam).Include(m => m.League).Include(m => m.Referee);
            return View(await applicationDbContext.ToListAsync());
        }
        [AllowAnonymous]
        // GET: Matches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Matches == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.AwayTeam)
                .Include(m => m.HomeTeam)
                .Include(m => m.League)
                .Include(m => m.Referee)
                .FirstOrDefaultAsync(m => m.MatchID == id);
            if (match == null)
            {
                return NotFound();
            }
            var modelView = new MatchDetailsViewModel() { Match = match, MatchStatistics = _repository.GetMatchStatisticsForMatch(match.MatchID) };
            return View(modelView);
        }

        // GET: Matches/Create
        public IActionResult Create()
        {
            ViewData["AwayTeamID"] = new SelectList(_context.Teams, "TeamID", "Name");
            ViewData["HomeTeamID"] = new SelectList(_context.Teams, "TeamID", "Name");
            ViewData["LeagueID"] = new SelectList(_context.Leagues, "LeagueID", "Name");
            ViewData["RefereeID"] = new SelectList(_context.Referees, "RefereeID", "Name");
            ViewBag.model = new Match();
            return View();
        }

        // POST: Matches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MatchID,LeagueID,HomeTeamID,AwayTeamID,MatchRound,MatchDateTime,RefereeID")] Match match)
        {
            if (ModelState.IsValid)
            {
                _context.Add(match);
                await _context.SaveChangesAsync();
                var matchstats = new MatchStatistics();
                matchstats.MatchID = match.MatchID;
                _context.Add(matchstats);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AwayTeamID"] = new SelectList(_context.Teams, "TeamID", "Name", match.AwayTeamID);
            ViewData["HomeTeamID"] = new SelectList(_context.Teams, "TeamID", "Name", match.HomeTeamID);
            ViewData["LeagueID"] = new SelectList(_context.Leagues, "LeagueID", "Name", match.LeagueID);
            ViewData["RefereeID"] = new SelectList(_context.Referees, "RefereeID", "Name", match.RefereeID);
            return View(match);
        }

        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Matches == null)
            {
                return NotFound();
            }

            var match = await _context.Matches.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }
            ViewData["AwayTeamID"] = new SelectList(_context.Teams, "TeamID", "Name", match.AwayTeamID);
            ViewData["HomeTeamID"] = new SelectList(_context.Teams, "TeamID", "Name", match.HomeTeamID);
            ViewData["LeagueID"] = new SelectList(_context.Leagues, "LeagueID", "Name", match.LeagueID);
            ViewData["RefereeID"] = new SelectList(_context.Referees, "RefereeID", "Name", match.RefereeID);
            return View(match);
        }

        // POST: Matches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MatchID,LeagueID,HomeTeamID,AwayTeamID,MatchRound,MatchDateTime,HomeTeamScore,AwayTeamScore,RefereeID")] Match match)
        {
            if (id != match.MatchID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.MatchID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AwayTeamID"] = new SelectList(_context.Teams, "TeamID", "Name", match.AwayTeamID);
            ViewData["HomeTeamID"] = new SelectList(_context.Teams, "TeamID", "Name", match.HomeTeamID);
            ViewData["LeagueID"] = new SelectList(_context.Leagues, "LeagueID", "Name", match.LeagueID);
            ViewData["RefereeID"] = new SelectList(_context.Referees, "RefereeID", "Name", match.RefereeID);
            return View(match);
        }

        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Matches == null)
            {
                return NotFound();
            }

            var match = await _context.Matches
                .Include(m => m.AwayTeam)
                .Include(m => m.HomeTeam)
                .Include(m => m.League)
                .Include(m => m.Referee)
                .FirstOrDefaultAsync(m => m.MatchID == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Matches == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Matches'  is null.");
            }
            var match = await _context.Matches.FindAsync(id);
            if (match != null)
            {
                try { 
                var matchstat = await _context.MatchStatistics.FirstOrDefaultAsync(ms => ms.MatchID == id);
                if (matchstat != null)
                {
                    _context.MatchStatistics.Remove(matchstat);
                }
                    await DeleteTeamStatisticsAsync(match.HomeTeamID, match.LeagueID, match.HomeTeamScore, match.AwayTeamScore);
                    await DeleteTeamStatisticsAsync(match.AwayTeamID, match.LeagueID, match.AwayTeamScore, match.HomeTeamScore);
                    _context.Matches.Remove(match);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.MatchID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MatchExists(int id)
        {
            return (_context.Matches?.Any(e => e.MatchID == id)).GetValueOrDefault();
        }


        private async Task AddTeamStatisticsAsync(int teamID, int leagueID, int? teamScore, int? enemyScore)
        {
            var teamStatistics = _context.TeamStatistics
                .FirstOrDefault(ts => ts.TeamID == teamID && ts.LeagueID == leagueID);

            if (teamStatistics != null)
            {
                teamStatistics.MatchesPlayed++;
                if (teamScore > enemyScore)
                {
                    teamStatistics.Wins++;
                    teamStatistics.Points += 3;
                }
                else if (teamScore < enemyScore)
                {
                    teamStatistics.Losses++;
                }
                else
                {
                    teamStatistics.Draws++;
                    teamStatistics.Points++;
                }
                teamStatistics.GoalsScored += teamScore ?? 0;
                teamStatistics.GoalsConceded += enemyScore ?? 0;

                // Сохранение изменений
                await _context.SaveChangesAsync();
            }
        }
        private async Task DeleteTeamStatisticsAsync(int teamID, int leagueID, int? teamScore, int? enemyScore)
        {
            var teamStatistics = _context.TeamStatistics
                .FirstOrDefault(ts => ts.TeamID == teamID && ts.LeagueID == leagueID);

            if (teamStatistics != null)
            {
                teamStatistics.MatchesPlayed--;
                if (teamScore > enemyScore)
                {
                    teamStatistics.Wins--;
                    teamStatistics.Points -= 3;
                }
                else if (teamScore < enemyScore)
                {
                    teamStatistics.Losses--;
                }
                else
                {
                    teamStatistics.Draws--;
                    teamStatistics.Points--;
                }
                teamStatistics.GoalsScored -= teamScore ?? 0;
                teamStatistics.GoalsConceded -= enemyScore ?? 0;

                // Сохранение изменений
                await _context.SaveChangesAsync();
            }
        }
        // GET: Matches/Edit/5
        public async Task<IActionResult> AddScore(int? id)
        {
            if (id == null || _context.Matches == null)
            {
                return View("~/Views/Shared/Error.cshtml", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }

            var match = await _context.Matches
                .Include(m => m.AwayTeam)
                .Include(m => m.HomeTeam)
                .Include(m => m.League)
                .Include(m => m.Referee)
                .SingleOrDefaultAsync(m => m.MatchID == id);
            if (match == null)
            {
                return NotFound();
            }
            return View(match);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddScore(int matchId, int? homeTeamScore, int? awayTeamScore)
        {
            var match = await _context.Matches.SingleOrDefaultAsync(m => m.MatchID == matchId);
            if (match == null)
            {
                return NotFound();
            }

            try
            {
                if (match.AwayTeamScore != null && match.HomeTeamScore != null)
                {
                    await DeleteTeamStatisticsAsync(match.HomeTeamID, match.LeagueID, match.HomeTeamScore, match.AwayTeamScore);
                    await DeleteTeamStatisticsAsync(match.AwayTeamID, match.LeagueID, match.AwayTeamScore, match.HomeTeamScore);
                }
                await AddTeamStatisticsAsync(match.HomeTeamID, match.LeagueID, homeTeamScore, awayTeamScore);
                await AddTeamStatisticsAsync(match.AwayTeamID, match.LeagueID, awayTeamScore, homeTeamScore);
                match.HomeTeamScore = homeTeamScore;
                match.AwayTeamScore = awayTeamScore;
                _context.Update(match);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MatchExists(match.MatchID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));

        }

        public JsonResult GetTeamsByLeague(int leagueId)
        {
            var teams = _context.TeamStatistics
                .Where(t => t.LeagueID == leagueId)
                .Select(t => new { value = t.TeamID, text = t.Team.Name })
                .ToList();

            return Json(teams);
        }
    }
}
