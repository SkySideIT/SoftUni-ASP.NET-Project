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
        Task<bool> GameExists(Guid id);
        Task<IEnumerable<SelectGameGenreViewModel>> GetAllGenresAsync();
        Task<IEnumerable<SelectGamePlatformViewModel>> GetAllPlatformsAsync();
        Task CreateGameAsync(GameCreateEditInputModel model);
        Task EditGameAsync(Guid id, GameCreateEditInputModel model);
        Task<GameDetailsViewModel?> GameDetailsViewModelAsync(Guid id);
        Task<IEnumerable<GameDetailsViewModel?>> CreateAllGamesDetailsViewModelAsync();
        Task<GameCreateEditInputModel?> CreateGameViewModel();
        Task<GameCreateEditInputModel?> EditGameViewModel(Guid id);
        Task DeleteGameAsync(Guid id);
        Task<bool> ValidateGameInputAsync(GameCreateEditInputModel model, ModelStateDictionary modelState, bool isEdit);
    }
}
