using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechWorld.Data.Common;
using TechWorld.Data.Common.Interfaces;
using TechWorld.Data.Models;
using TechWorld.Services.Core.Interfaces;
using TechWorld.Web.ViewModels;

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

        public async Task CreateGameAsync(GameCreateInputModel model)
        {
            Publisher newPublisher = new Publisher { Name = model.PublisherName.Trim() };
            await _repository.AddAsync(newPublisher);
            await _repository.SaveChangesAsync();
            int publisherId = newPublisher.Id;

            Game game = new Game
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Description = model.Description,
                Price = model.Price,
                GenreId = model.GenreId,
                PlatformId = model.PlatformId,
                PublisherId = publisherId,
                ReleaseDate = model.ReleaseDate,
                ImageUrl = model.ImageUrl
            };

            await _repository.AddAsync(game);
            await _repository.SaveChangesAsync();
        }

        public async Task<GameDetailsViewModel?> CreateGameDetailsViewModelAsync(Guid id)
        {
            var game = await _repository
                .GetSingleAsync<Game>
                (
                    g => g.Id == id,
                    g => g.Genre,
                    g => g.Platform,
                    g => g.Publisher
                );

            if (game == null)
            {
                return null!;
            }

            var gameModel = new GameDetailsViewModel
            {
                Id = game.Id,
                Title = game.Title,
                Description = game.Description,
                Price = game.Price,
                Genre = game.Genre.Name,
                Platform = game.Platform.Name,
                Publisher = game.Publisher.Name,
                ReleaseDate = game.ReleaseDate,
                ImageUrl = game.ImageUrl!
            };

            return gameModel;
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

        public async Task<IEnumerable<SelectGameGenreViewModel>> GetAllGenresAsync()
        {
            var genres = await _repository.GetAllAsync<Genre>();

            return genres
                .Select(g => new SelectGameGenreViewModel
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToArray();
        }

        public async Task<IEnumerable<SelectGamePlatformViewModel>> GetAllPlatformsAsync()
        {
            var platforms = await _repository.GetAllAsync<Platform>();

            return platforms
                .Select(g => new SelectGamePlatformViewModel
                {
                    Id = g.Id,
                    Name = g.Name
                })
                .ToArray();
        }

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

        public async Task<bool> ValidateGameInputAsync(GameCreateInputModel model, ModelStateDictionary modelState)
        {
            if (model.Price < 0 || model.Price > 500)
            {
                modelState.AddModelError(nameof(model.Price), "Price must be greater than 0.");
            }

            if (model.ReleaseDate > DateOnly.FromDateTime(DateTime.Today))
            {
                modelState.AddModelError(nameof(model.ReleaseDate), "Release date cannot be in the future.");
            }

            bool genreExists = (await _repository.FindAsync<Genre>(g => g.Id == model.GenreId)).Any();
            if (!genreExists)
            {
                modelState.AddModelError(nameof(model.GenreId), "Selected genre does not exist.");
            }

            bool platformExists = (await _repository.FindAsync<Platform>(g => g.Id == model.PlatformId)).Any();
            if (!genreExists)
            {
                modelState.AddModelError(nameof(model.PlatformId), "Selected platform does not exist.");
            }

            bool duplicate = (await _repository.FindAsync<Game>(g => g.Title.ToLower() == model.Title.ToLower())).Any();
            if (duplicate)
            {
                modelState.AddModelError(nameof(model.Title), $"A game named '{model.Title}' already exists.");
            }

            return modelState.IsValid;
        }
    }
}
