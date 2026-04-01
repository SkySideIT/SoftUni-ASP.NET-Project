using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechWorld.GCommon.Exceptions;
using TechWorld.Services.Core.Interfaces;

namespace TechWorld.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId(User);

            var model = await _cartService.GetUserCartAsync(userId);

            ViewBag.Total = model.Sum(x => x.Price);

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(Guid gameId)
        {
            var userId = GetUserId(User);
            
            try
            {
                await _cartService.AddAsync(userId, gameId);
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
                TempData["ErrorMessage"] = "An error occurred while adding the game to the cart. Please try again later.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Remove(Guid gameId)
        {
            var userId = GetUserId(User);

            try
            {
                await _cartService.RemoveAsync(userId, gameId);
            }
            catch (EntityNotFoundException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while removing the game from the cart. Please try again later.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase()
        {
            var userId = GetUserId(User);

            try
            {
                await _cartService.ClearCartAsync(userId);
            }
            catch (Exception)
            {
                TempData["ErrorMessage"] = "An error occurred while finalizing the deal. Please try again later.";
            }

            TempData["SuccessMessage"] = "Congratulations! Your order was successful 🎉";

            return RedirectToAction(nameof(Index));
        }

        public static Guid GetUserId(ClaimsPrincipal user)
        {
            return Guid.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }
    }
}
