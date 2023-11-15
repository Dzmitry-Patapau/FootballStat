using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballStats.Data;
using FootballStats.Data.Identity;
using Microsoft.AspNetCore.Authorization;

namespace FootballStats.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class MatchStatisticsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MatchStatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: MatchStatistics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MatchStatistics == null)
            {
                return NotFound();
            }

            var matchStatistics = await _context.MatchStatistics
                .Include (ms => ms.Match)
                .Where(ms => ms.Match.MatchID == id).FirstOrDefaultAsync();
            if (matchStatistics == null)
            {
                return NotFound();
            }
            //ViewData["MatchID"] = new SelectList(_context.Matches, "MatchID", "MatchID", matchStatistics.MatchID);
            return View(matchStatistics);
        }

        // POST: MatchStatistics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MatchStatID,MatchID,CornersHome,CornersAway,YellowCardsHome,YellowCardsAway,RedCardsHome,RedCardsAway")] MatchStatistics matchStatistics)
        {
            if (id != matchStatistics.MatchID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(matchStatistics);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchStatisticsExists(matchStatistics.MatchStatID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Matches");
            }
            ViewData["MatchID"] = new SelectList(_context.Matches, "MatchID", "MatchID", matchStatistics.MatchID);
            return View(matchStatistics);
        }

        private bool MatchStatisticsExists(int id)
        {
          return (_context.MatchStatistics?.Any(e => e.MatchStatID == id)).GetValueOrDefault();
        }
    }
}
