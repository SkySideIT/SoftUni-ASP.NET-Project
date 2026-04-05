using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechWorld.Data.Common;
using TechWorld.Data.Common.Interfaces;
using TechWorld.Data.Models;
using TechWorld.GCommon.Exceptions;
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

        public async Task<IEnumerable<GameDetailsViewModel?>> AllGamesDetailsViewModelAsync(string? userId = null)
        {
            var allGames = await _repository
                .GetAllAsync<Game>
                (
                    g => true,
                    g => g.Genre,
                    g => g.Platform,
                    g => g.Publisher
                );

            HashSet<Guid> wishlistIds = new();

            if (!string.IsNullOrEmpty(userId))
            {
                var userGames = await _repository.GetAllAsync<UserGame>(x => x.UserId.ToString() == userId);

                wishlistIds = userGames
                    .Select(x => x.GameId)
                    .ToHashSet();
            }

            HashSet<Guid> cartIds = new();

            if (!string.IsNullOrEmpty(userId))
            {
                var userCart = await _repository.GetAllAsync<CartProduct>(x => x.UserId.ToString() == userId);

                cartIds = userCart
                    .Select(x => x.GameId)
                    .ToHashSet();
            }

            var viewModel = allGames.Select(g => new GameDetailsViewModel
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description,
                Price = g.Price,
                Genre = g.Genre.Name,
                Platform = g.Platform.Name,
                Publisher = g.Publisher.Name,
                ReleaseDate = g.ReleaseDate,
                ImageUrl = g.ImageUrl!,
                InWishlist = wishlistIds.Contains(g.Id),
                InCart = cartIds.Contains(g.Id)
            });

            return viewModel;
        }

        public async Task CreateGameAsync(GameCreateEditInputModel model)
        {
            Publisher newPublisher = new Publisher { Name = model.PublisherName.Trim() };
            await _repository.AddAsync(newPublisher);
            await _repository.SaveChangesAsync();
            int publisherId = newPublisher.Id;

            Game game = new Game
            {
                Id = Guid.NewGuid(),
                Title = model.Title.Trim(),
                Description = model.Description.Trim(),
                Price = model.Price,
                GenreId = model.GenreId,
                PlatformId = model.PlatformId,
                PublisherId = publisherId,
                ReleaseDate = model.ReleaseDate,
                ImageUrl = model.ImageUrl?.Trim() ?? "https://placehold.co/600x300?text=No+Image"
            };

            await _repository.AddAsync(game);
            await _repository.SaveChangesAsync();
        }

        public async Task<GameDetailsViewModel?> GameDetailsViewModelAsync(Guid id)
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
                throw new EntityNotFoundException("Game not found.");
            }

            var gameModel = new GameDetailsViewModel
            {
                Id = game.Id,
                Title = game.Title.Trim(),
                Description = game.Description.Trim(),
                Price = game.Price,
                Genre = game.Genre.Name.Trim(),
                Platform = game.Platform.Name.Trim(),
                Publisher = game.Publisher.Name.Trim(),
                ReleaseDate = game.ReleaseDate,
                ImageUrl = game.ImageUrl?.Trim() ?? "https://cdn.cloudflare.steamstatic.com/steam/apps/256658589/header.jpg"
            };

            return gameModel;
        }

        public async Task<GameCreateEditInputModel?> CreateGameViewModel()
        {
            GameCreateEditInputModel model = new GameCreateEditInputModel
            {
                Genres = await GetAllGenresAsync(),
                Platforms = await GetAllPlatformsAsync()
            };

            return model;
        }

        public async Task DeleteGameAsync(Guid id)
        {
            Game? game = await _repository.GetSingleAsync<Game>
                (
                    g => g.Id == id,
                    g => g.Genre,
                    g => g.Platform,
                    g => g.Publisher
                );

            if (game == null)
            {
                throw new EntityNotFoundException("Game not found.");
            }

            Publisher publisher = game.Publisher;

            _repository.Delete(game);
            await _repository.SaveChangesAsync();

            bool hasOtherGames = (await _repository
                .GetAllAsync<Game>(g => g.PublisherId == publisher.Id))
                .Any();

            if (!hasOtherGames)
            {
                _repository.Delete(publisher);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task EditGameAsync([FromRoute] Guid id, GameCreateEditInputModel model)
        {
            Game? game = await _repository.GetSingleAsync<Game>
                (
                    g => g.Id == id,
                    g => g.Genre,
                    g => g.Platform,
                    g => g.Publisher
                );

            if (game == null)
            {
                throw new EntityNotFoundException("Game not found.");
            }

            Publisher oldPublisher = game!.Publisher;

            if (oldPublisher.Name.Trim() != model.PublisherName.Trim())
            {
                var newPublisher = new Publisher { Name = model.PublisherName.Trim() };
                await _repository.AddAsync(newPublisher);
                await _repository.SaveChangesAsync();

                game.PublisherId = newPublisher.Id;
            }

            game.Title = model.Title.Trim();
            game.Description = model.Description.Trim();
            game.Price = model.Price;
            game.GenreId = model.GenreId;
            game.PlatformId = model.PlatformId;
            game.ReleaseDate = model.ReleaseDate;
            game.ImageUrl = model.ImageUrl?.Trim() ?? "https://placehold.co/600x300?text=No+Image";

            _repository.Update(game);
            await _repository.SaveChangesAsync();

            bool hasOtherGames = (await _repository
                .GetAllAsync<Game>(g => g.PublisherId == oldPublisher.Id))
                .Any();

            if (!hasOtherGames)
            {
                _repository.Delete(oldPublisher);
                await _repository.SaveChangesAsync();
            }
        }

        public async Task<GameCreateEditInputModel?> EditGameViewModel(Guid id)
        {
            Game? game = await _repository.GetSingleAsync<Game>
            (
                g => g.Id == id,
                g => g.Publisher
            );

            if (game == null)
            {
                throw new EntityNotFoundException("Game not found.");
            }

            GameCreateEditInputModel viewModel = new GameCreateEditInputModel
            {
                Title = game!.Title.Trim(),
                Description = game.Description.Trim(),
                Price = game.Price,
                PublisherName = game.Publisher.Name.Trim(),
                GenreId = game.GenreId,
                Genres = await GetAllGenresAsync(),
                PlatformId = game.PlatformId,
                Platforms = await GetAllPlatformsAsync(),
                ReleaseDate = game.ReleaseDate,
                ImageUrl = game.ImageUrl?.Trim() ?? "https://cdn.cloudflare.steamstatic.com/steam/apps/256658589/header.jpg",
            };

            return viewModel;
        }

        public async Task<bool> GameExists(Guid id)
        {
            Game? game = await _repository.GetByIdAsync<Game>(id);
            if (game == null)
            {
                return false;
            }
            else
            {
                return true;
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

        public async Task<GamesIndexViewModel> GetAllGamesAsync
            (
                int currentPage,
                int pageSize,
                string? userId = null, 
                string? searchTerm = null, 
                int? genreId = null, 
                int? platformId = null
            )
        {
            var games = await _repository
                .GetAllAsync<Game>
                (
                    g => true,
                    g => g.Genre,
                    g => g.Platform,
                    g => g.Publisher
                );

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                games = games.Where(g =>
                    g.Title.ToLower().Contains(searchTerm.ToLower()));
            }

            if (genreId.HasValue)
            {
                games = games.Where(g => g.GenreId == genreId.Value);
            }

            if (platformId.HasValue)
            {
                games = games.Where(g => g.PlatformId == platformId.Value);
            }

            int totalGames = games.Count();

            var pagedGames = games
                .OrderBy(g => g.Title)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);

            HashSet<Guid> wishlistIds = new();
            HashSet<Guid> cartIds = new();

            if (!string.IsNullOrEmpty(userId))
            {
                var userWishlist = await _repository.GetAllAsync<UserGame>(x => x.UserId.ToString() == userId);
                wishlistIds = userWishlist.Select(x => x.GameId).ToHashSet();

                var userCart = await _repository.GetAllAsync<CartProduct>(x => x.UserId.ToString() == userId);
                cartIds = userCart.Select(x => x.GameId).ToHashSet();
            }

            var gamesViewModel = pagedGames.Select(g => new GameDetailsViewModel
            {
                Id = g.Id,
                Title = g.Title,
                Description = g.Description,
                ImageUrl = g.ImageUrl,
                Price = g.Price,
                GenreId = g.GenreId,
                Genre = g.Genre.Name,
                PlatformId = g.PlatformId,
                Platform = g.Platform.Name,
                Publisher = g.Publisher.Name,
                ReleaseDate = g.ReleaseDate,
                InWishlist = wishlistIds.Contains(g.Id),
                InCart = cartIds.Contains(g.Id)
            });

            var genres = await _repository.GetAllAsync<Genre>();
            var genreViewModels = genres.Select(g => new SelectGameGenreViewModel
            {
                Id = g.Id,
                Name = g.Name
            });

            var platforms = await _repository.GetAllAsync<Platform>();
            var platformViewModels = platforms.Select(p => new SelectGamePlatformViewModel
            {
                Id = p.Id,
                Name = p.Name
            });

            return new GamesIndexViewModel
            {
                Games = gamesViewModel,
                Genres = genreViewModels,
                Platforms = platformViewModels,

                SearchTerm = searchTerm,
                GenreId = genreId,
                PlatformId = platformId,

                CurrentPage = currentPage,
                TotalPages = (int)Math.Ceiling((double)totalGames / pageSize)
            };
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

        public async Task<IEnumerable<LatestGamesCardViewModel?>> GetLatestGamesAsync(int count)
        {
            var allGames = (await _repository.GetAllAsync<Game>()).OrderByDescending(g => g.ReleaseDate).Take(count);

            var viewModel = allGames
                .Select(g => new LatestGamesCardViewModel
                {
                    Id = g.Id,
                    Title = g.Title,
                    Description = g.Description,
                    ImageUrl = g.ImageUrl!
                });

            return viewModel;
        }

        public async Task<bool> ValidateGameInputAsync(GameCreateEditInputModel model, ModelStateDictionary modelState, bool isEdit = false)
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

            if (!isEdit)
            {
                bool duplicate = (await _repository.FindAsync<Game>(g => g.Title.ToLower() == model.Title.ToLower())).Any();
                if (duplicate)
                {
                    modelState.AddModelError(nameof(model.Title), $"A game named '{model.Title}' already exists.");
                }
            }

            return modelState.IsValid;
        }
    }
}
