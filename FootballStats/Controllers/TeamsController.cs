using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FootballStats.Data;
using FootballStats.Data.Identity;
using FootballStats.Services;
using FootballStats.Models;
using FootballStats.Services.Repository;
using Microsoft.AspNetCore.Authorization;
//using FootballStats.Data.Migrations;

namespace FootballStats.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly IRepository _repository;

        public TeamsController(ApplicationDbContext context, IImageService imageService, IRepository repository)
        {
            _context = context;
            _imageService = imageService;
            _repository = repository;
        }

        // GET: Teams
        public async Task<IActionResult> Index()
        {
            return _context.Teams != null ?
                        View(await _context.Teams.ToListAsync()) :
                        Problem("Entity set 'ApplicationDbContext.Teams'  is null.");
        }
        [AllowAnonymous]
        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.TeamID == id);
            if (team == null)
            {
                return NotFound();
            }
            var modelView = new TeamDetailsViewModel() { Team = team, Leagues = await _repository.GetLeaguesForTeam(team.TeamID), Matches = await _repository.GetMatchesForTeam(team.TeamID) };
            return View(modelView);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeamID,Name,Stadium")] Team team, IFormFile Logo, IFormFile LogoStadium)
        {
            if (ModelState.IsValid)
            {
                if (Logo != null)
                {
                    try
                    {
                        team.Logo = await _imageService.SaveImageAsync(Logo, "image/team");
                    }
                    catch (Exception ex)
                    {
                    ModelState.AddModelError(string.Empty, "An error occurred while saving the image.");
                    return View(team); // Вернуть представление с сообщением об ошибке
                    }
                }

                if (LogoStadium != null)
                {
                    try
                    {
                        team.LogoStadium = await _imageService.SaveImageAsync(LogoStadium, "image/stadium");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while saving the image.");
                        return View(team); // Вернуть представление с сообщением об ошибке
                    }
                }

                _context.Add(team);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(team);
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            
            if (team == null)
            {
                return NotFound();
            }
            HttpContext.Session.SetString("logo", team.Logo);
            HttpContext.Session.SetString("logostadium", team.LogoStadium);
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeamID,Name,Stadium")] Team team, IFormFile Logo, IFormFile LogoStadium)
        {
            if (id != team.TeamID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (Logo != null)
                {
                    try
                    {
                        team.Logo = await _imageService.SaveImageAsync(Logo, "image/team");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while saving the image.");
                        return View(team); // Вернуть представление с сообщением об ошибке
                    }
                }
                if (LogoStadium != null)
                {
                    try
                    {
                        team.LogoStadium = await _imageService.SaveImageAsync(LogoStadium, "image/stadium");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "An error occurred while saving the image.");
                        return View(team); // Вернуть представление с сообщением об ошибке
                    }
                }
                try
                {
                    _context.Update(team);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeamExists(team.TeamID))
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
            return View(team);
        }

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.TeamID == id);
            if (team == null)
            {
                return NotFound();
            }
            var viewModel = new TeamDeleteViewModel() { Team = team };
            return View(viewModel);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teams == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Teams'  is null.");
            }
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                try
                {
                    _context.Teams.Remove(team);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // В случае ошибки, устанавливаем сообщение в модель представления
                    return View(new TeamDeleteViewModel { Team = team, Error = ex.Message });
                }
            }
            return NotFound();
        }

        private bool TeamExists(int id)
        {
          return (_context.Teams?.Any(e => e.TeamID == id)).GetValueOrDefault();
        }
    }
}
