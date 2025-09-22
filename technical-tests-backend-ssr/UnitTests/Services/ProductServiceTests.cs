using Moq;
using Xunit;
using System.ComponentModel.DataAnnotations;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;
using technical_tests_backend_ssr.Services;

namespace technical_tests_backend_ssr.UnitTests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repoMock = new();
        private readonly IProductService _service;

        public ProductServiceTests()
        {
            _service = new ProductService(_repoMock.Object);
        }

        [Fact]
        public async Task CreateAsync_Should_Validate_And_Call_Repository()
        {
            var input = new Product { Name = "  Mouse  ", Price = 25.5m };
            _repoMock.Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Product p, CancellationToken _) => p);

            var result = await _service.CreateAsync(input);

            Assert.Equal("Mouse", result.Name); // trimmed
            Assert.Equal(25.5m, result.Price);
            _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Should_Throw_When_Name_Is_Empty()
        {
            var input = new Product { Name = "  ", Price = 10m };
            await Assert.ThrowsAsync<ValidationException>(() => _service.CreateAsync(input));
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Existing_Product()
        {
            var id = 1;
            var existing = new Product { Id = id, Name = "Old", Price = 10m, CreatedAt = DateTime.UtcNow };
            _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(existing);

            var update = new Product { Name = "New", Price = 20m };

            await _service.UpdateAsync(id, update);

            _repoMock.Verify(r => r.UpdateAsync(It.Is<Product>(p => p.Name == "New" && p.Price == 20m), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Should_Throw_When_NotFound()
        {
            var id = 999;
            _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                     .ReturnsAsync((Product?)null);

            var update = new Product { Name = "New", Price = 20m };

            await Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(id, update));
        }

        [Fact]
        public async Task DeleteAsync_Should_Validate_Id()
        {
            await Assert.ThrowsAsync<ValidationException>(() => _service.DeleteAsync(0));
        }

        [Fact]
        public async Task GetAllAsync_Should_Call_Repository()
        {
            _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new List<Product>());

            var result = await _service.GetAllAsync();
            Assert.NotNull(result);
            _repoMock.Verify(r => r.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_Should_Call_Repository()
        {
            var id = 123;
            _repoMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                     .ReturnsAsync(new Product { Id = id, Name = "X", Price = 1m });

            var result = await _service.GetByIdAsync(id);
            Assert.NotNull(result);
            Assert.Equal(id, result!.Id);
            _repoMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
