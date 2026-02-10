using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TechWorld.Data;
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

        public IActionResult Index()
        {
            var latestGames = _gameService.GetLatestGamesAsync(3)
                .Result
                .Select(g => new Latest3GamesCardViewModel
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description,
                    ImageUrl = g.ImageUrl!
                });
            
            return View(latestGames);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
