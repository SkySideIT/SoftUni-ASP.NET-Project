using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechWorld.Data.Common;
using TechWorld.Data.Common.Interfaces;
using TechWorld.Data.Models;
using TechWorld.GCommon.Exceptions;
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

        public async Task<IEnumerable<WishlistViewModel?>> GetUserWishlistByIdAsync(string userId)
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

            var viewModel = userGames.Select(x => new WishlistViewModel
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

        public async Task AddAsync(string userId, Guid gameId)
        {
            bool exists = await ExistsAsync(userId, gameId);

            if (exists)
            {
                throw new EntityAlreadyExistsException("Game is already in wishlist.");
            }

            bool gameExists = await _repository.GetByIdAsync<Game>(gameId) != null;

            if (!gameExists)
            {
                throw new EntityNotFoundException("Game not found.");
            }

            UserGame entity = new UserGame
            {
                UserId = userId,
                GameId = gameId
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task RemoveAsync(string userId, Guid gameId)
        {
            bool gameExists = await _repository.GetByIdAsync<Game>(gameId) != null;

            if (!gameExists)
            {
                throw new EntityNotFoundException("Game not found.");
            }

            var userGame = await _repository.GetSingleAsync<UserGame>
            (
                ug => ug.UserId == userId && ug.GameId == gameId
            );

            if (userGame == null)
            {
                throw new EntityNotFoundException("Game is not in wishlist.");
            }

            _repository.Delete(userGame);
            await _repository.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(string userId, Guid gameId)
        {
            var entity = await _repository.GetSingleAsync<UserGame>
            (
                x => x.UserId.ToLower() == userId.ToLower() && x.GameId == gameId
            );

            return entity != null;
        }
    }
}
