using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechWorld.GCommon.Exceptions;
using TechWorld.Services.Core;
using TechWorld.Services.Core.Interfaces;
using TechWorld.Web.ViewModels;

namespace TechWorld.Web.Controllers
{
    public class WishlistController : Controller
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;
            IEnumerable<GameDetailsViewModel?> viewModel = await _wishlistService.GetUserWishlistByIdAsync(userId);

            return View(viewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Add([FromRoute(Name = "id")]Guid gameId)
        {
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier)!;

            try
            {
                await _wishlistService.AddAsync(userId, gameId);
            }
            catch (EntityNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (EntityAlreadyExistsException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while adding the game to the wishlist. Please try again later.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index", "Wishlist");
        }
    }
}
