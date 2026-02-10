using TechWorld.Data.Common.Interfaces;
using TechWorld.Data.Models;
using TechWorld.Services.Core.Interfaces;

namespace TechWorld.Services.Core
{
    public class GameService : IGameService
    {
        private readonly IRepository<Game> _repository;

        public GameService(IRepository<Game> repository)
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
            var game = await _repository.GetByIdAsync(id);
            if (game != null)
            {
                _repository.Delete(game);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Game>> GetAllGamesAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Game?> GetGameByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Game>> GetLatestGamesAsync(int count)
        {
            var allGames = await _repository.GetAllAsync();
            return allGames.OrderByDescending(g => g.ReleaseDate).Take(count);
        }

        public async Task UpdateGameAsync(Game game)
        {
            _repository.Update(game);
            await _repository.SaveChangesAsync();
        }
    }
}
