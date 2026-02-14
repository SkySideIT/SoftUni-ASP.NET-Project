using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechWorld.Data.Models;
using TechWorld.Web.ViewModels;

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
        Task<IEnumerable<SelectGameGenreViewModel>> GetAllGenresAsync();
        Task<IEnumerable<SelectGamePlatformViewModel>> GetAllPlatformsAsync();
        Task CreateGameAsync(GameCreateInputModel model);
        Task<GameDetailsViewModel?> CreateGameDetailsViewModelAsync(Guid id);
        Task<bool> ValidateGameInputAsync(GameCreateInputModel model, ModelStateDictionary modelState);
    }
}
