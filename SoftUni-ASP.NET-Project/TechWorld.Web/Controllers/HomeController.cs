using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechWorld.Data;
using TechWorld.Data.Models;
using TechWorld.Services.Core.Interfaces;
using TechWorld.Web.ViewModels;

namespace TechWorld.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGameService _gameService;

        public HomeController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var latestGames = await _gameService.GetLatestGamesAsync(3);

            var viewModel = latestGames
                .Select(g => new LatestGamesCardViewModel
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description,
                    ImageUrl = g.ImageUrl!
                });
            
            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
