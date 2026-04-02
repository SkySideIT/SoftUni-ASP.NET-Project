using System;
using System.Security.Claims;
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
        public async Task<IActionResult> Index(string? searchTerm, int? genreId, int? platformId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var games = await _gameService.GetAllGamesAsync(userId, searchTerm, genreId, platformId);

            return View(games);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(Guid id)
        {
            GameDetailsViewModel? gameModel = await _gameService.GameDetailsViewModelAsync(id);

            if (gameModel == null)
            {
                return NotFound();
            }

            return View(gameModel);
        }
    }
}
