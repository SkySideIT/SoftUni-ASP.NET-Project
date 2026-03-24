using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechWorld.GCommon;
using TechWorld.Services.Core.Interfaces;
using TechWorld.Web.ViewModels;

namespace TechWorld.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = ApplicationConstants.AdminRole)]
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var viewModel = await _gameService.AllGamesDetailsViewModelAsync(userId);

            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        [HttpGet]
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

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ModelState.AddModelError(string.Empty, "An error occurred while editing the game. Please try again later.");

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            GameDetailsViewModel? game = await _gameService.GameDetailsViewModelAsync(id);

            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            try
            {
                await _gameService.DeleteGameAsync(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return Redirect(Url.Action("Delete", "Home") + "/" + id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
