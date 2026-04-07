using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Moq;
using TechWorld.Data.Common.Interfaces;
using TechWorld.Data.Models;
using TechWorld.GCommon.Exceptions;
using TechWorld.Services.Core;
using TechWorld.Web.ViewModels;

namespace TechWorld.Services.Tests
{
    [TestFixture]
    public class GameServiceTests
    {
        private Mock<IRepository> _repositoryMock;
        private GameService _gameService;

        private Guid _gameId;

        [SetUp]
        public void Setup()
        {
            _gameId = Guid.Parse("08d4aebe-59f1-4630-8c8d-6163d596d294");

            _repositoryMock = new Mock<IRepository>();
            _gameService = new GameService(_repositoryMock.Object);
        }

        [Test]
        public async Task GameExists_GameExists_ReturnsTrue()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync(new Game());

            var result = await _gameService.GameExists(_gameId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task GameExists_GameNotFound_ReturnsFalse()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync((Game?)null);

            var result = await _gameService.GameExists(_gameId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task CreateGameAsync_ValidData_ShouldAddGameAndPublisher()
        {
            var model = new GameCreateEditInputModel
            {
                Title = "Test",
                Description = "Desc",
                Price = 10,
                GenreId = 1,
                PlatformId = 1,
                PublisherName = "Pub",
                ReleaseDate = DateOnly.FromDateTime(DateTime.Today)
            };

            await _gameService.CreateGameAsync(model);

            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Publisher>()), Times.Once);
            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Game>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Exactly(2));
        }

        [Test]
        public async Task GetGameByIdAsync_GameExists_ReturnsGame()
        {
            _repositoryMock.Setup(r => r.GetSingleAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Expression<Func<Game, object>>[]>()))
                .ReturnsAsync(new Game());

            var result = await _gameService.GetGameByIdAsync(_gameId);

            Assert.IsNotNull(result);
        }

        [Test]
        public async Task DeleteGameAsync_GameExists_ShouldDeleteGame()
        {
            var publisher = new Publisher { Id = 1 };

            _repositoryMock.Setup(r => r.GetSingleAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Expression<Func<Game, object>>[]>()))
                .ReturnsAsync(new Game { Publisher = publisher });

            _repositoryMock.Setup(r => r.GetAllAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(new List<Game>());

            await _gameService.DeleteGameAsync(_gameId);

            _repositoryMock.Verify(r => r.Delete(It.IsAny<Game>()), Times.Once);
            _repositoryMock.Verify(r => r.Delete(It.IsAny<Publisher>()), Times.Once);
        }

        [Test]
        public void DeleteGameAsync_GameNotFound_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetSingleAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Expression<Func<Game, object>>[]>()))
                .ReturnsAsync((Game?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _gameService.DeleteGameAsync(_gameId));
        }

        [Test]
        public async Task GetLatestGamesAsync_ReturnsOrderedGames()
        {
            _repositoryMock.Setup(r => r.GetAllAsync<Game>())
                .ReturnsAsync(new List<Game>
                {
                    new Game { Id = Guid.NewGuid(), ReleaseDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1)) },
                    new Game { Id = Guid.NewGuid(), ReleaseDate = DateOnly.FromDateTime(DateTime.Today) }
                });

            var result = await _gameService.GetLatestGamesAsync(2);

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task ValidateGameInputAsync_ValidData_ReturnsTrue()
        {
            var model = new GameCreateEditInputModel
            {
                Title = "Test",
                Price = 10,
                GenreId = 1,
                PlatformId = 1,
                ReleaseDate = DateOnly.FromDateTime(DateTime.Today)
            };

            var modelState = new ModelStateDictionary();

            _repositoryMock.Setup(r => r.FindAsync<Genre>(It.IsAny<Expression<Func<Genre, bool>>>()))
                .ReturnsAsync(new List<Genre> { new Genre() });

            _repositoryMock.Setup(r => r.FindAsync<Platform>(It.IsAny<Expression<Func<Platform, bool>>>()))
                .ReturnsAsync(new List<Platform> { new Platform() });

            _repositoryMock.Setup(r => r.FindAsync<Game>(It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(new List<Game>());

            var result = await _gameService.ValidateGameInputAsync(model, modelState, false);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task ValidateGameInputAsync_InvalidPrice_ReturnsFalse()
        {
            var model = new GameCreateEditInputModel
            {
                Title = "Test",
                Price = -1,
                GenreId = 1,
                PlatformId = 1,
                ReleaseDate = DateOnly.FromDateTime(DateTime.Today)
            };

            var modelState = new ModelStateDictionary();

            _repositoryMock.Setup(r => r.FindAsync<Genre>(It.IsAny<Expression<Func<Genre, bool>>>()))
                .ReturnsAsync(new List<Genre> { new Genre() });

            _repositoryMock.Setup(r => r.FindAsync<Platform>(It.IsAny<Expression<Func<Platform, bool>>>()))
                .ReturnsAsync(new List<Platform> { new Platform() });

            var result = await _gameService.ValidateGameInputAsync(model, modelState, false);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task ValidateGameInputAsync_FutureDate_ReturnsFalse()
        {
            var model = new GameCreateEditInputModel
            {
                Title = "Test",
                Price = 10,
                GenreId = 1,
                PlatformId = 1,
                ReleaseDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
            };

            var modelState = new ModelStateDictionary();

            _repositoryMock.Setup(r => r.FindAsync<Genre>(It.IsAny<Expression<Func<Genre, bool>>>()))
                .ReturnsAsync(new List<Genre> { new Genre() });

            _repositoryMock.Setup(r => r.FindAsync<Platform>(It.IsAny<Expression<Func<Platform, bool>>>()))
                .ReturnsAsync(new List<Platform> { new Platform() });

            var result = await _gameService.ValidateGameInputAsync(model, modelState, false);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task EditGameAsync_ValidData_ShouldUpdateGame()
        {
            var publisher = new Publisher { Id = 1, Name = "Old" };

            var game = new Game
            {
                Id = _gameId,
                Publisher = publisher
            };

            var model = new GameCreateEditInputModel
            {
                Title = "New",
                Description = "Desc",
                Price = 20,
                GenreId = 1,
                PlatformId = 1,
                PublisherName = "Old",
                ReleaseDate = DateOnly.FromDateTime(DateTime.Today)
            };

            _repositoryMock.Setup(r => r.GetSingleAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Expression<Func<Game, object>>[]>()))
                .ReturnsAsync(game);

            _repositoryMock.Setup(r => r.GetAllAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(new List<Game> { game });

            await _gameService.EditGameAsync(_gameId, model);

            _repositoryMock.Verify(r => r.Update(It.IsAny<Game>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.AtLeastOnce);
        }

        [Test]
        public async Task EditGameAsync_PublisherChanged_ShouldCreateNewPublisher()
        {
            var oldPublisher = new Publisher { Id = 1, Name = "Old" };

            var game = new Game
            {
                Id = _gameId,
                Publisher = oldPublisher
            };

            var model = new GameCreateEditInputModel
            {
                Title = "Test",
                Description = "Desc",
                Price = 10,
                GenreId = 1,
                PlatformId = 1,
                PublisherName = "NewPublisher",
                ReleaseDate = DateOnly.FromDateTime(DateTime.Today)
            };

            _repositoryMock.Setup(r => r.GetSingleAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Expression<Func<Game, object>>[]>()))
                .ReturnsAsync(game);

            _repositoryMock.Setup(r => r.GetAllAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>()))
                .ReturnsAsync(new List<Game>());

            await _gameService.EditGameAsync(_gameId, model);

            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Publisher>()), Times.Once);
        }

        [Test]
        public void EditGameAsync_GameNotFound_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetSingleAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Expression<Func<Game, object>>[]>()))
                .ReturnsAsync((Game?)null);

            var model = new GameCreateEditInputModel();

            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _gameService.EditGameAsync(_gameId, model));
        }

        [Test]
        public async Task GetAllGamesAsync_NoFilters_ReturnsAll()
        {
            var games = new List<Game>
            {
                CreateGame("Game1"),
                CreateGame("Game2")
            };

            _repositoryMock.Setup(r => r.GetAllAsync<Game>(
                 It.IsAny<Expression<Func<Game, bool>>>(),
                 It.IsAny<Expression<Func<Game, object>>[]>()))
                .ReturnsAsync(games);

            _repositoryMock.Setup(r => r.GetAllAsync<Genre>())
                .ReturnsAsync(new List<Genre>());

            _repositoryMock.Setup(r => r.GetAllAsync<Platform>())
                .ReturnsAsync(new List<Platform>());

            var result = await _gameService.GetAllGamesAsync(1, 10);

            Assert.AreEqual(2, result.Games.Count());
        }

        [Test]
        public async Task GetAllGamesAsync_WithSearchTerm_ShouldFilter()
        {
            var games = new List<Game>
            {
                CreateGame("FIFA"),
                CreateGame("CSGO")
            };

            _repositoryMock.Setup(r => r.GetAllAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Expression<Func<Game, object>>[]>()))
                .ReturnsAsync(games);

            _repositoryMock.Setup(r => r.GetAllAsync<Genre>())
                .ReturnsAsync(new List<Genre>());

            _repositoryMock.Setup(r => r.GetAllAsync<Platform>())
                .ReturnsAsync(new List<Platform>());

            var result = await _gameService.GetAllGamesAsync(1, 10, null, "fifa");

            Assert.AreEqual(1, result.Games.Count());
        }

        [Test]
        public async Task GetAllGamesAsync_WithGenreFilter_ShouldFilter()
        {
            var games = new List<Game>
            {
                CreateGame("Game1", genreId: 1),
                CreateGame("Game2", genreId: 2)
            };

            _repositoryMock.Setup(r => r.GetAllAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Expression<Func<Game, object>>[]>()))
                .ReturnsAsync(games);

            _repositoryMock.Setup(r => r.GetAllAsync<Genre>())
                .ReturnsAsync(new List<Genre>());

            _repositoryMock.Setup(r => r.GetAllAsync<Platform>())
                .ReturnsAsync(new List<Platform>());

            var result = await _gameService.GetAllGamesAsync(1, 10, null, null, 1);

            Assert.AreEqual(1, result.Games.Count());
        }

        [Test]
        public async Task GetAllGamesAsync_WithPlatformFilter_ShouldFilter()
        {
            var games = new List<Game>
            {
                CreateGame("Game1", platformId: 1),
                CreateGame("Game2", platformId: 2)
            };

            _repositoryMock.Setup(r => r.GetAllAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Expression<Func<Game, object>>[]>()))
                .ReturnsAsync(games);

            _repositoryMock.Setup(r => r.GetAllAsync<Genre>())
                .ReturnsAsync(new List<Genre>());

            _repositoryMock.Setup(r => r.GetAllAsync<Platform>())
                .ReturnsAsync(new List<Platform>());

            var result = await _gameService.GetAllGamesAsync(1, 10, null, null, null, 1);

            Assert.AreEqual(1, result.Games.Count());
        }

        [Test]
        public async Task GetAllGamesAsync_ShouldApplyPaging()
        {
            var games = Enumerable.Range(1, 20)
                .Select(i => CreateGame($"Game{i}"))
                .ToList();

            _repositoryMock.Setup(r => r.GetAllAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Expression<Func<Game, object>>[]>()))
                .ReturnsAsync(games);

            _repositoryMock.Setup(r => r.GetAllAsync<Genre>())
                .ReturnsAsync(new List<Genre>());

            _repositoryMock.Setup(r => r.GetAllAsync<Platform>())
                .ReturnsAsync(new List<Platform>());

            var result = await _gameService.GetAllGamesAsync(2, 5);

            Assert.AreEqual(5, result.Games.Count());
        }

        [Test]
        public async Task GameDetailsViewModelAsync_GameExists_ReturnsViewModel()
        {
            _repositoryMock.Setup(r => r.GetSingleAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Expression<Func<Game, object>>[]>()))
                .ReturnsAsync(new Game
                {
                    Id = _gameId,
                    Title = "Test",
                    Description = "Desc",
                    Price = 10,
                    Genre = new Genre { Name = "Action" },
                    Platform = new Platform { Name = "PC" },
                    Publisher = new Publisher { Name = "Pub" },
                    ImageUrl = "img"
                });

            var result = await _gameService.GameDetailsViewModelAsync(_gameId);

            Assert.IsNotNull(result);
            Assert.AreEqual("Test", result.Title);
        }

        [Test]
        public void GameDetailsViewModelAsync_GameNotFound_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetSingleAsync<Game>(
                It.IsAny<Expression<Func<Game, bool>>>(),
                It.IsAny<Expression<Func<Game, object>>[]>()))
                .ReturnsAsync((Game?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _gameService.GameDetailsViewModelAsync(_gameId));
        }

        private Game CreateGame(string title, int genreId = 1, int platformId = 1)
        {
            return new Game
            {
                Id = Guid.NewGuid(),
                Title = title,
                GenreId = genreId,
                PlatformId = platformId,
                Genre = new Genre { Id = genreId, Name = $"Genre{genreId}" },
                Platform = new Platform { Id = platformId, Name = $"Platform{platformId}" },
                Publisher = new Publisher { Id = 1, Name = "TestPublisher" },
                ReleaseDate = DateOnly.FromDateTime(DateTime.Today),
                Price = 10,
                Description = "Desc",
                ImageUrl = "img"
            };
        }
    }
}
