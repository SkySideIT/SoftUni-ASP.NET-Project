using Microsoft.AspNetCore.Mvc.Rendering;
using TechWorld.Data.Common;
using TechWorld.Data.Common.Interfaces;
using TechWorld.Data.Models;
using TechWorld.Services.Core.Interfaces;

namespace TechWorld.Services.Core
{
    public class GameService : IGameService
    {
        private readonly IRepository _repository;

        public GameService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task AddGameAsync(Game game)
        {
            await _repository.AddAsync(game);
            await _repository.SaveChangesAsync();
        }

        public async Task DeleteGameAsync(Guid id)
        {
            var game = await _repository.GetByIdAsync<Game>(id);
            if (game != null)
            {
                _repository.Delete(game);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            return await _repository
                .GetAllAsync<Game>
                (
                    g => true,
                    g => g.Genre,
                    g => g.Platform,
                    g => g.Publisher
                );
        }

        public async Task<IEnumerable<Genre>> GetAllGenresAsync()
            => await _repository.GetAllAsync<Genre>();

        public async Task<IEnumerable<Platform>> GetAllPlatformsAsync()
            => await _repository.GetAllAsync<Platform>();

        public async Task<IEnumerable<Publisher>> GetAllPublishersAsync()
            => await _repository.GetAllAsync<Publisher>();

        public async Task<Game?> GetGameByIdAsync(Guid id)
        {
            return await _repository
                .GetSingleAsync<Game>
                (
                    g => g.Id == id,
                    g => g.Genre,
                    g => g.Platform,
                    g => g.Publisher
                );
        }

        public async Task<IEnumerable<Game>> GetLatestGamesAsync(int count)
        {
            var allGames = await _repository.GetAllAsync<Game>();
            return allGames.OrderByDescending(g => g.ReleaseDate).Take(count);
        }

        public async Task UpdateGameAsync(Game game)
        {
            _repository.Update(game);
            await _repository.SaveChangesAsync();
        }
    }
}
