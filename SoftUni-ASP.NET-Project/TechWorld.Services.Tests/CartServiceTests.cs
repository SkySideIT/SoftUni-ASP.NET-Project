using System.Linq.Expressions;
using Moq;
using TechWorld.Data.Common.Interfaces;
using TechWorld.Data.Models;
using TechWorld.GCommon.Exceptions;
using TechWorld.Services.Core;

namespace TechWorld.Services.Tests
{
    [TestFixture]
    public class CartServiceTests
    {
        private Mock<IRepository> _repositoryMock;
        private CartService _cartService;

        private Guid _userId;
        private Guid _gameId;

        [SetUp]
        public void Setup()
        {
            _userId = Guid.Parse("001994e4-75a5-4717-88f9-95cf0009fbf2");
            _gameId = Guid.Parse("17845c0e-4e0d-4c2e-98de-a50759c70ef5");

            _repositoryMock = new Mock<IRepository>();
            _cartService = new CartService(_repositoryMock.Object);
        }

        [Test]
        public async Task AddAsync_ValidData_AddsCartProduct()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync(new Game());

            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync(new ApplicationUser());

            _repositoryMock.Setup(r => r.FindAsync<CartProduct>(
                    It.IsAny<Expression<Func<CartProduct, bool>>>()))
                .ReturnsAsync(new List<CartProduct>());

            await _cartService.AddAsync(_userId, _gameId);

            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<CartProduct>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void AddAsync_GameAlreadyInCart_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync(new Game());

            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync(new ApplicationUser());

            _repositoryMock.Setup(r => r.FindAsync<CartProduct>(
                    It.IsAny<Expression<Func<CartProduct, bool>>>()))
                .ReturnsAsync(new List<CartProduct> { new CartProduct() });

            Assert.ThrowsAsync<EntityAlreadyExistsException>(() =>
                _cartService.AddAsync(_userId, _gameId));
        }

        [Test]
        public void AddAsync_GameNotFound_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync((Game?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _cartService.AddAsync(_userId, _gameId));
        }

        [Test]
        public void AddAsync_UserNotFound_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync(new Game());

            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync((ApplicationUser?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _cartService.AddAsync(_userId, _gameId));
        }

        [Test]
        public async Task RemoveAsync_GameInCart_ShouldDelete()
        {
            _repositoryMock.Setup(r => r.GetSingleAsync<CartProduct>(
                    It.IsAny<Expression<Func<CartProduct, bool>>>(),
                    It.IsAny<Expression<Func<CartProduct, object>>[]>()))
                .ReturnsAsync(new CartProduct());

            _repositoryMock.Setup(r => r.GetByIdAsync<Game>(_gameId))
                .ReturnsAsync(new Game());

            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync(new ApplicationUser());

            await _cartService.RemoveAsync(_userId, _gameId);

            _repositoryMock.Verify(r => r.Delete(It.IsAny<CartProduct>()), Times.Once);
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void RemoveAsync_GameNotInCart_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetSingleAsync<CartProduct>(
                    It.IsAny<Expression<Func<CartProduct, bool>>>(),
                    It.IsAny<Expression<Func<CartProduct, object>>[]>()))
                .ReturnsAsync((CartProduct?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _cartService.RemoveAsync(_userId, _gameId));
        }

        [Test]
        public async Task GetUserCartAsync_UserFound_ShouldReturnItems()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync(new ApplicationUser());

            _repositoryMock.Setup(r => r.GetAllAsync<CartProduct>(
                    It.IsAny<Expression<Func<CartProduct, bool>>>(),
                    It.IsAny<Expression<Func<CartProduct, object>>[]>()))
                .ReturnsAsync(new List<CartProduct>
                {
                    new CartProduct
                    {
                        Game = new Game
                        {
                            Id = Guid.NewGuid(),
                            Title = "Test",
                            Price = 10,
                            ImageUrl = "img"
                        }
                    }
                });

            var result = await _cartService.GetUserCartAsync(_userId);

            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void GetUserCartAsync_UserNotFound_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync((ApplicationUser?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _cartService.GetUserCartAsync(_userId));
        }

        [Test]
        public async Task ClearCartAsync_UserFound_ShouldClearAllItems()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync(new ApplicationUser());

            _repositoryMock.Setup(r => r.GetAllAsync<CartProduct>(
                    It.IsAny<Expression<Func<CartProduct, bool>>>()))
                .ReturnsAsync(new List<CartProduct>
                {
                    new CartProduct(),
                    new CartProduct()
                });

            await _cartService.ClearCartAsync(_userId);

            _repositoryMock.Verify(r => r.Delete(It.IsAny<CartProduct>()), Times.Exactly(2));
            _repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public void ClearCartAsync_UserNotFound_ThrowsException()
        {
            _repositoryMock.Setup(r => r.GetByIdAsync<ApplicationUser>(_userId))
                .ReturnsAsync((ApplicationUser?)null);

            Assert.ThrowsAsync<EntityNotFoundException>(() =>
                _cartService.ClearCartAsync(_userId));
        }
    }
}