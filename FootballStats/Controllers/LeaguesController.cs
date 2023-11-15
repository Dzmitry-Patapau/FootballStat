using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballStats.Data;
using FootballStats.Data.Identity;
using FootballStats.Models;
using FootballStats.Data.Migrations;
using FootballStats.Services.Repository;
using Microsoft.AspNetCore.Authorization;

namespace FootballStats.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LeaguesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository _repository;

        public LeaguesController(ApplicationDbContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        // GET: Leagues
        public async Task<IActionResult> Index()
        {
              return _context.Leagues != null ? 
                          View(await _context.Leagues.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Leagues'  is null.");
        }

        // GET: Leagues/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Leagues == null)
            {
                return NotFound();
            }

            var league = await _context.Leagues
                .FirstOrDefaultAsync(m => m.LeagueID == id);
            if (league == null)
            {
                return NotFound();
            }
            var modelView = new LeagueDetailsViewModel() { League =  league, Teams = await _repository.GetTeamsForLeague(league.LeagueID), Matches = await _repository.GetMatchesForLeague(league.LeagueID) };
            return View(modelView);
        }

        // GET: Leagues/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Leagues/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeagueID,Name")] League league)
        {
            if (ModelState.IsValid)
            {
                _context.Add(league);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(league);
        }

        // GET: Leagues/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Leagues == null)
            {
                return NotFound();
            }

            var league = await _context.Leagues.FindAsync(id);
            if (league == null)
            {
                return NotFound();
            }
            return View(league);
        }

        // POST: Leagues/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeagueID,Name")] League league)
        {
            if (id != league.LeagueID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(league);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeagueExists(league.LeagueID))
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
            return View(league);
        }

        // GET: Leagues/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Leagues == null)
            {
                return NotFound();
            }

            var league = await _context.Leagues
                .FirstOrDefaultAsync(m => m.LeagueID == id);
            if (league == null)
            {
                return NotFound();
            }
            var viewModel = new LeagueDeleteViewModel() { League = league };
            return View(viewModel);
        }

        // POST: Leagues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Leagues == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Leagues'  is null.");
            }
            var league = await _context.Leagues.FindAsync(id);
            if (league != null)
            {
                try
                {
                    _context.Leagues.Remove(league);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // В случае ошибки, устанавливаем сообщение в модель представления
                    return View(new LeagueDeleteViewModel { League = league, Error = ex.Message });
                }
            }
            return NotFound();
        }

        private bool LeagueExists(int id)
        {
          return (_context.Leagues?.Any(e => e.LeagueID == id)).GetValueOrDefault();
        }
    }
}
