using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechWorld.Data.Common;
using TechWorld.Data.Common.Interfaces;
using TechWorld.Data.Models;
using TechWorld.Services.Core.Interfaces;
using TechWorld.Web.ViewModels;

namespace TechWorld.Services.Core
{
    public class WishlistService : IWishlistService
    {
        private readonly IRepository _repository;

        public WishlistService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<GameDetailsViewModel?>> GetUserWishlistByIdAsync(string userId)
        {
            var userGames = await _repository
                .GetAllAsync<UserGame>
                (
                    ug => ug.UserId == userId,
                    ug => ug.Game,
                    ug => ug.Game.Genre,
                    ug => ug.Game.Platform,
                    ug => ug.Game.Publisher
                );

            if (userGames == null)
            {
                return null!;
            }

            var viewModel = userGames.Select(x => new GameDetailsViewModel
            {
                Id = x.Game.Id,
                Title = x.Game.Title,
                Description = x.Game.Description,
                Price = x.Game.Price,
                ImageUrl = x.Game.ImageUrl,
                Genre = x.Game.Genre.Name,
                Platform = x.Game.Platform.Name,
                Publisher = x.Game.Publisher.Name
            });

            return viewModel;
        }
    }
}
