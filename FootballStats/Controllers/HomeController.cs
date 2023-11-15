using FootballStats.Data;
using FootballStats.Models;
using FootballStats.Services.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace FootballStats.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IRepository _repository;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context, IRepository repository)
        {
            _logger = logger;
            _context = context;
            _repository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> TeamsAsync()
        {
            return _context.Teams != null ?
              View(await _context.Teams.OrderBy(x => x.Name).ToListAsync()) :
              Problem("Entity set 'ApplicationDbContext.Teams'  is null.");
        }
        public async Task<IActionResult> LeaguesAsync()
        {
            return _context.Leagues != null ?
                          View(await _context.Leagues.OrderBy(x => x.Name).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Leagues'  is null.");
        }
        public async Task<IActionResult> RefereesAsync()
        {
            return _context.Referees != null ?
                          View(await _context.Referees.OrderBy(x => x.Name).ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Referees'  is null.");
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PrivacyAsync()
        {
            return View();
        }
        [Authorize(Roles = "Administrator")]
        public IActionResult AdminPanel()
        {
            return View();
        }

        public async Task<IActionResult> DetailsLeague(int? id)
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
            var modelView = await _repository.GetTeamsStatisticsForLeague(league.LeagueID);
            return View(modelView);
        }
        public async Task<IActionResult> TestApi()
        {
            var apiUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/api/league/4/Statistics";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(apiUrl);

                var leagueStatistics = JsonConvert.DeserializeObject<List<LeagueStatistics>>(response);
                leagueStatistics = leagueStatistics.OrderByDescending(stat => stat.Points).ToList();
                return View(leagueStatistics);
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}