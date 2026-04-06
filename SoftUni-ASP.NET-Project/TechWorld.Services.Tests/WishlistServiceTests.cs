using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Moq;
using TechWorld.Data.Common.Interfaces;
using TechWorld.Data.Models;
using TechWorld.GCommon.Exceptions;
using TechWorld.Services.Core;

namespace TechWorld.Services.Tests
{
    [TestFixture]
    public class WishlistServiceTests
    {
        private Mock<IRepository> _repositoryMock;
        private WishlistService _wishlistService;

        private Guid _userId;
        private Guid _gameId;

        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IRepository>();

            _userId = Guid.Parse("001994e4-75a5-4717-88f9-95cf0009fbf2");
            _gameId = Guid.Parse("17845c0e-4e0d-4c2e-98de-a50759c70ef5");

            _wishlistService = new WishlistService(_repositoryMock.Object);
        }

        [Test]
        public async Task AddAsync_ValidData_ShouldAdd()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync(new Game());

            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync(new ApplicationUser());

            _repositoryMock.Setup(r => r.GetSingleAsync<UserGame>(
                    It.IsAny<Expression<Func<UserGame, bool>>>(),
                    It.IsAny<Expression<Func<UserGame, object>>[]>()))
                .ReturnsAsync((UserGame?)null);

            await _wishlistService.AddAsync(_userId, _gameId);

            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<UserGame>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void AddAsync_GameNotFound_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync((Game?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _wishlistService.AddAsync(_userId, _gameId));
        }

        [Test]
        public void AddAsync_UserNotFound_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync(new Game());

            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync((ApplicationUser?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _wishlistService.AddAsync(_userId, _gameId));
        }

        [Test]
        public void AddAsync_AlreadyExists_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync(new Game());

            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync(new ApplicationUser());

            _repositoryMock.Setup(r => r.GetSingleAsync<UserGame>(
                    It.IsAny<Expression<Func<UserGame, bool>>>(),
                    It.IsAny<Expression<Func<UserGame, object>>[]>()))
                .ReturnsAsync(new UserGame());

            Assert.ThrowsAsync<EntityAlreadyExistsException>(() =>
                _wishlistService.AddAsync(_userId, _gameId));
        }

        [Test]
        public async Task RemoveAsync_ValidData_ShouldRemove()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync(new Game());

            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync(new ApplicationUser());

            _repositoryMock.Setup(r => r.GetSingleAsync<UserGame>(
                    It.IsAny<Expression<Func<UserGame, bool>>>(),
                    It.IsAny<Expression<Func<UserGame, object>>[]>()))
                .ReturnsAsync(new UserGame());

            await _wishlistService.RemoveAsync(_userId, _gameId);

            _repositoryMock.Verify(r => r.Delete(It.IsAny<UserGame>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void RemoveAsync_GameNotFound_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync((Game?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _wishlistService.RemoveAsync(_userId, _gameId));
        }

        [Test]
        public void RemoveAsync_UserNotFound_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync(new Game());

            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync((ApplicationUser?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _wishlistService.RemoveAsync(_userId, _gameId));
        }

        [Test]
        public void RemoveAsync_NotInWishlist_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync(new Game());

            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync(new ApplicationUser());

            _repositoryMock.Setup(r => r.GetSingleAsync<UserGame>(
                    It.IsAny<Expression<Func<UserGame, bool>>>(),
                    It.IsAny<Expression<Func<UserGame, object>>[]>()))
                .ReturnsAsync((UserGame?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _wishlistService.RemoveAsync(_userId, _gameId));
        }

        [Test]
        public async Task ExistsAsync_EntityExists_ReturnsTrue()
        {
            _repositoryMock.Setup(r => r.GetSingleAsync<UserGame>(
                    It.IsAny<Expression<Func<UserGame, bool>>>(),
                    It.IsAny<Expression<Func<UserGame, object>>[]>()))
                .ReturnsAsync(new UserGame());

            var result = await _wishlistService.ExistsAsync(_userId, _gameId);

            Assert.IsTrue(result);
        }

        [Test]
        public async Task ExistsAsync_EntityNotFound_ReturnsFalse()
        {
            _repositoryMock.Setup(r => r.GetSingleAsync<UserGame>(
                    It.IsAny<Expression<Func<UserGame, bool>>>(),
                    It.IsAny<Expression<Func<UserGame, object>>[]>()))
                .ReturnsAsync((UserGame?)null);

            var result = await _wishlistService.ExistsAsync(_userId, _gameId);

            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetUserWishlistByIdAsync_ValidUser_ReturnsWishlist()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync(new ApplicationUser());

            _repositoryMock.Setup(r => r.GetAllAsync<UserGame>(
                    It.IsAny<Expression<Func<UserGame, bool>>>(),
                    It.IsAny<Expression<Func<UserGame, object>>[]>()))
                .ReturnsAsync(new List<UserGame>
                {
                    new UserGame
                    {
                        Game = new Game
                        {
                            Id = _gameId,
                            Title = "Test",
                            Description = "Desc",
                            Price = 10,
                            ImageUrl = "img",
                            Genre = new Genre { Name = "Action" },
                            Platform = new Platform { Name = "PC" },
                            Publisher = new Publisher { Name = "TestPub" }
                        }
                    }
                });

            _repositoryMock.Setup(r => r.GetAllAsync<CartProduct>(
                    It.IsAny<Expression<Func<CartProduct, bool>>>()))
                .ReturnsAsync(new List<CartProduct>());

            var result = await _wishlistService.GetUserWishlistByIdAsync(_userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void GetUserWishlistByIdAsync_UserNotFound_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync((ApplicationUser?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _wishlistService.GetUserWishlistByIdAsync(_userId));
        }
    }
}
