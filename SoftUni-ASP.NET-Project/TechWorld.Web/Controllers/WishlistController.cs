using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechWorld.GCommon.Exceptions;
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
            Guid userId = GetUserId(User);
            IEnumerable<WishlistViewModel?> viewModel = await _wishlistService.GetUserWishlistByIdAsync(userId);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Guid gameId)
        {
            Guid userId = GetUserId(User);

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

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Guid gameId)
        {
            Guid userId = GetUserId(User);

            try
            {
                await _wishlistService.RemoveAsync(userId, gameId);
            }
            catch (EntityNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while removing the game from the wishlist. Please try again later.";
            }

            return RedirectToAction(nameof(Index));
        }

        public static Guid GetUserId(ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }
    }
}
