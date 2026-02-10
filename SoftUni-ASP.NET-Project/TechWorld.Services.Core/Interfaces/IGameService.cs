using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechWorld.Data.Models;

namespace TechWorld.Services.Core.Interfaces
{
    public interface IGameService
    {
        Task<IEnumerable<Game>> GetAllGamesAsync();
        Task<IEnumerable<Game>> GetLatestGamesAsync(int count);
        Task<Game?> GetGameByIdAsync(Guid id);
        Task AddGameAsync(Game game);
        Task UpdateGameAsync(Game game);
        Task DeleteGameAsync(Guid id);
    }
}
