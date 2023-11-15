using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballStats.Data;
using FootballStats.Data.Identity;
using FootballStats.Services.Repository;
using FootballStats.Models;
using Microsoft.AspNetCore.Authorization;

namespace FootballStats.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TeamStatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository _repository;

        public TeamStatisticsController(ApplicationDbContext context,IRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        // GET: TeamStatistics
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TeamStatistics.Include(t => t.League).Include(t => t.Team);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TeamStatistics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TeamStatistics == null)
            {
                return NotFound();
            }

            var teamStatistics = await _context.TeamStatistics
                .Include(t => t.League)
                .Include(t => t.Team)
                .FirstOrDefaultAsync(m => m.TeamStatID == id);
            if (teamStatistics == null)
            {
                return NotFound();
            }
            var modelView = new TeamStaticticsDetailsViewModel() { TeamStatistics = teamStatistics, Matches = await _repository.GetMatchesForTeamInLeague(teamStatistics.TeamID, teamStatistics.LeagueID) };
            return View(modelView);
        }

        // GET: TeamStatistics/Create
        public IActionResult Create()
        {
            ViewData["LeagueID"] = new SelectList(_context.Leagues, "LeagueID", "Name");
            ViewData["TeamID"] = new SelectList(_context.Teams, "TeamID", "Name");
            return View();
        }

        // POST: TeamStatistics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeamStatID,TeamID,LeagueID")] TeamStatistics teamStatistics)
        {
            if (ModelState.IsValid)
            {
                var isDuplicate = _context.TeamStatistics.FirstOrDefault(l => l.TeamID == teamStatistics.TeamID && l.LeagueID == teamStatistics.LeagueID);
                if (isDuplicate == null)
                {
                    _context.Add(teamStatistics);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["ErrorMessage"] = "The team is already associated with the selected league.";
                return View(teamStatistics);
            }
            ViewData["LeagueID"] = new SelectList(_context.Leagues, "LeagueID", "Name", teamStatistics.LeagueID);
            ViewData["TeamID"] = new SelectList(_context.Teams, "TeamID", "Name", teamStatistics.TeamID);
            return View(teamStatistics);
        }

        // GET: TeamStatistics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TeamStatistics == null)
            {
                return NotFound();
            }

            var teamStatistics = await _context.TeamStatistics.FindAsync(id);
            if (teamStatistics == null)
            {
                return NotFound();
            }
            ViewData["LeagueID"] = new SelectList(_context.Leagues, "LeagueID", "Name", teamStatistics.LeagueID);
            ViewData["TeamID"] = new SelectList(_context.Teams, "TeamID", "Name", teamStatistics.TeamID);
            return View(teamStatistics);
        }

        // POST: TeamStatistics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeamStatID,TeamID,LeagueID,MatchesPlayed,Wins,Draws,Losses,GoalsScored,GoalsConceded,Points")] TeamStatistics teamStatistics)
        {
            if (id != teamStatistics.TeamStatID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teamStatistics);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamStatisticsExists(teamStatistics.TeamStatID))
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
            ViewData["LeagueID"] = new SelectList(_context.Leagues, "LeagueID", "Name", teamStatistics.LeagueID);
            ViewData["TeamID"] = new SelectList(_context.Teams, "TeamID", "Name", teamStatistics.TeamID);
            return View(teamStatistics);
        }

        // GET: TeamStatistics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TeamStatistics == null)
            {
                return NotFound();
            }

            var teamStatistics = await _context.TeamStatistics
                .Include(t => t.League)
                .Include(t => t.Team)
                .FirstOrDefaultAsync(m => m.TeamStatID == id);
            if (teamStatistics == null)
            {
                return NotFound();
            }

            return View(teamStatistics);
        }

        // POST: TeamStatistics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TeamStatistics == null)
            {
                return Problem("Entity set 'ApplicationDbContext.TeamStatistics'  is null.");
            }
            var teamStatistics = await _context.TeamStatistics.FindAsync(id);
            if (teamStatistics != null)
            {
                _context.TeamStatistics.Remove(teamStatistics);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamStatisticsExists(int id)
        {
          return (_context.TeamStatistics?.Any(e => e.TeamStatID == id)).GetValueOrDefault();
        }
    }
}
