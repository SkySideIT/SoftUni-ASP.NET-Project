using System;
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
            GameDetailsViewModel? gameModel = await _gameService.CreateGameDetailsViewModelAsync(id);

            if (gameModel == null)
            {
                return NotFound();
            }

            return View(gameModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Create()
        {
            var model = new GameCreateInputModel
            {
                Genres = await _gameService.GetAllGenresAsync(),
                Platforms = await _gameService.GetAllPlatformsAsync()
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(GameCreateInputModel model)
        {
            bool isValid = await _gameService.ValidateGameInputAsync(model, ModelState);
            if (!ModelState.IsValid || !isValid)
            {
                model.Genres = await _gameService.GetAllGenresAsync();
                model.Platforms = await _gameService.GetAllPlatformsAsync();

                return View(model);
            }

            try
            {
                await _gameService.CreateGameAsync(model);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ModelState.AddModelError(string.Empty, "An error occurred while creating the game. Please try again later.");

                return View(model);
            }
        }
    }
}
