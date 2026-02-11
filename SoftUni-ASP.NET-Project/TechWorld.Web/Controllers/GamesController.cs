using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechWorld.Data.Models;
using TechWorld.Services.Core.Interfaces;
using TechWorld.Web.ViewModels;

namespace TechWorld.Web.Controllers
{
    public class GamesController : Controller
    {
        private readonly IGameService _gameService;

        public GamesController(IGameService gameService)
        {
            _gameService = gameService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var allGames = await _gameService.GetAllGamesAsync();

            var viewModel = allGames
                .Select(g => new GameDetailsViewModel
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description,
                    Price = g.Price,
                    Genre = g.Genre.Name,
                    Platform = g.Platform.Name,
                    Publisher = g.Publisher.Name,
                    ReleaseDate = g.ReleaseDate,
                    ImageUrl = g.ImageUrl!
                });

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(Guid id)
        {
            Game? game = await _gameService.GetGameByIdAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            var gameModel = new GameDetailsViewModel
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price,
                Genre = game.Genre.Name,
                Platform = game.Platform.Name,
                Publisher = game.Publisher.Name,
                ReleaseDate = game.ReleaseDate,
                ImageUrl = game.ImageUrl!
            };

            return View(gameModel);
        }
    }
}
