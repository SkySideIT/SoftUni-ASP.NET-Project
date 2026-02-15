using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            IEnumerable<GameDetailsViewModel?> viewModel = await _gameService.CreateAllGamesDetailsViewModelAsync();

            if (viewModel == null)
            {
                return NotFound();
            }

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
            GameCreateEditInputModel? model = await _gameService.CreateGameViewModel();

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(GameCreateEditInputModel model)
        {
            bool isValid = await _gameService.ValidateGameInputAsync(model, ModelState, false);
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

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(Guid id)
        {
            GameCreateEditInputModel? model = await _gameService.EditGameViewModel(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit([FromRoute] Guid id, GameCreateEditInputModel model)
        {
            bool gameExists = await _gameService.GameExists(id);

            if (!gameExists)
            {
                return NotFound();
            }

            bool isValid = await _gameService.ValidateGameInputAsync(model, ModelState, true);
            if (!ModelState.IsValid || !isValid)
            {
                model.Genres = await _gameService.GetAllGenresAsync();
                model.Platforms = await _gameService.GetAllPlatformsAsync();

                return View(model);
            }

            try
            {
                await _gameService.EditGameAsync(id, model);

                return Redirect(Url.Action("Details", "Games") + "/" + id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ModelState.AddModelError(string.Empty, "An error occurred while editing the game. Please try again later.");

                return View(model);
            }
        }
    }
}
