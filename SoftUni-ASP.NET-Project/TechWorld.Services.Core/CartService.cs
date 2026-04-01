using TechWorld.Data.Common.Interfaces;
using TechWorld.Data.Models;
using TechWorld.GCommon.Exceptions;
using TechWorld.Services.Core.Interfaces;
using TechWorld.Web.ViewModels;

namespace TechWorld.Services.Core
{
    public class CartService : ICartService
    {
        private readonly IRepository _repository;

        public CartService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task AddAsync(Guid userId, Guid gameId)
        {
            bool gameExists = (await _repository.GetByIdAsync<Game>(gameId)) != null;

            if (!gameExists)
            {
                throw new EntityNotFoundException("Game not found.");
            }

            bool userExists = (await _repository.GetByIdAsync<ApplicationUser>(userId)) != null;

            if (!userExists)
            {
                throw new EntityNotFoundException("User not found.");
            }

            bool exists = (await _repository
                .FindAsync<CartProduct>(x => x.UserId == userId && x.GameId == gameId))
                .Any();

            if (exists)
            {
                throw new EntityAlreadyExistsException("Game already in cart.");
            }

            var entity = new CartProduct
            {
                UserId = userId,
                GameId = gameId
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
        }

        public async Task ClearCartAsync(Guid userId)
        {
            bool userExists = (await _repository.GetByIdAsync<ApplicationUser>(userId)) != null;

            if (!userExists)
            {
                throw new EntityNotFoundException("User not found.");
            }

            var items = await _repository
                .GetAllAsync<CartProduct>
                (
                    x => x.UserId == userId
                );

            foreach (var item in items)
            {
                _repository.Delete(item);
            }

            await _repository.SaveChangesAsync();
        }

        public async Task<IEnumerable<CartViewModel?>> GetUserCartAsync(Guid userId)
        {
            bool userExists = (await _repository.GetByIdAsync<ApplicationUser>(userId)) != null;

            if (!userExists)
            {
                throw new EntityNotFoundException("User not found.");
            }

            var items = await _repository
                .GetAllAsync<CartProduct>
                (
                    x => x.UserId == userId,
                    x => x.Game
                );

            return items.Select(x => new CartViewModel
            {
                Id = x.Game.Id,
                Title = x.Game.Title,
                Price = x.Game.Price,
                ImageUrl = x.Game.ImageUrl!
            });
        }

        public async Task RemoveAsync(Guid userId, Guid gameId)
        {
            var item = await _repository
                .GetSingleAsync<CartProduct>
                (
                    x => x.UserId == userId && x.GameId == gameId
                );

            if (item == null)
            {
                throw new EntityNotFoundException("Cart product not found.");
            }

            bool gameExists = (await _repository.GetByIdAsync<Game>(gameId)) != null;

            if (!gameExists)
            {
                throw new EntityNotFoundException("Game not found.");
            }

            bool userExists = (await _repository.GetByIdAsync<ApplicationUser>(userId)) != null;

            if (!userExists)
            {
                throw new EntityNotFoundException("User not found.");
            }

            _repository.Delete(item);
            await _repository.SaveChangesAsync();
        }
    }
}
