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
using FootballStats.Services.Repository;
using Microsoft.AspNetCore.Authorization;

namespace FootballStats.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class RefereesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository _repository;

        public RefereesController(ApplicationDbContext context, IRepository repository)
        {
            _context = context;
            _repository = repository;
        }
        public IActionResult Index1()
        {
            return View();
                        
        }
        // GET: Referees
        public async Task<IActionResult> Index()
        {
              return _context.Referees != null ? 
                          View(await _context.Referees.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Referees'  is null.");
        }
        [AllowAnonymous]
        // GET: Referees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Referees == null)
            {
                return NotFound();
            }

            var referee = await _context.Referees
                .FirstOrDefaultAsync(m => m.RefereeID == id);
            if (referee == null)
            {
                return NotFound();
            }
            var modelView = new RefereeDetailsViewModel() { Referee = referee, Matches = await _repository.GetMatchesForReferee(referee.RefereeID) };
            return View(modelView);
        }

        // GET: Referees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Referees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RefereeID,Name,Nationality")] Referee referee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(referee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(referee);
        }

        // GET: Referees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Referees == null)
            {
                return NotFound();
            }

            var referee = await _context.Referees.FindAsync(id);
            if (referee == null)
            {
                return NotFound();
            }
            return View(referee);
        }

        // POST: Referees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RefereeID,Name,Nationality")] Referee referee)
        {
            if (id != referee.RefereeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(referee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefereeExists(referee.RefereeID))
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
            return View(referee);
        }

        // GET: Referees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Referees == null)
            {
                return NotFound();
            }

            var referee = await _context.Referees
                .FirstOrDefaultAsync(m => m.RefereeID == id);
            if (referee == null)
            {
                return NotFound();
            }
            var viewModel = new RefereeDeleteViewModel() { Referee = referee};
            
            return View(viewModel);
        }

        // POST: Referees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Referees == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Referees' is null.");
            }
            var referee = await _context.Referees.FindAsync(id);
            if (referee != null)
            {
                try
                {
                    _context.Referees.Remove(referee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // В случае ошибки, устанавливаем сообщение в модель представления
                    return View(new RefereeDeleteViewModel { Referee = referee, Error = ex.Message });
                }
            }
            return NotFound();
        }

        private bool RefereeExists(int id)
        {
          return (_context.Referees?.Any(e => e.RefereeID == id)).GetValueOrDefault();
        }
    }
}
