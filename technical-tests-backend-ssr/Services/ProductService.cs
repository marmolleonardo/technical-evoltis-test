using System.ComponentModel.DataAnnotations;
using technical_tests_backend_ssr.Models;
using technical_tests_backend_ssr.Repositories;

namespace technical_tests_backend_ssr.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repo;

        public ProductService(IProductRepository repo)
        {
            _repo = repo;
        }

        public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default)
        {
            Validate(product, isCreate: true);
            product.Name = product.Name.Trim();
            if (product.UpdatedAt.HasValue && product.UpdatedAt < product.CreatedAt)
            {
                throw new ValidationException("UpdatedAt cannot be earlier than CreatedAt.");
            }
            return await _repo.AddAsync(product, cancellationToken);
        }

        public Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return _repo.GetByIdAsync(id, cancellationToken);
        }

        public Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return _repo.GetAllAsync(cancellationToken);
        }

        public async Task<Product> UpdateAsync(int id, Product product, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
            {
                throw new ValidationException("Invalid product id.");
            }

            var existing = await _repo.GetByIdAsync(id, cancellationToken);
            if (existing is null)
            {
                throw new KeyNotFoundException($"Product with id {id} was not found.");
            }

            // Apply allowed updates
            existing.Name = (product.Name ?? existing.Name).Trim();
            existing.Price = product.Price;
            existing.UpdatedAt = DateTime.UtcNow;

            Validate(existing, isCreate: false);

            await _repo.UpdateAsync(existing, cancellationToken);
            return existing;
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            if (id <= 0)
            {
                throw new ValidationException("Invalid product id.");
            }
            await _repo.DeleteAsync(id, cancellationToken);
        }

        private static void Validate(Product product, bool isCreate)
        {
            if (product is null)
            {
                throw new ValidationException("Product cannot be null.");
            }
            if (string.IsNullOrWhiteSpace(product.Name))
            {
                throw new ValidationException("Product name is required.");
            }
            if (product.Name.Length > 200)
            {
                throw new ValidationException("Product name must be at most 200 characters.");
            }
            if (product.Price < 0)
            {
                throw new ValidationException("Price cannot be negative.");
            }

            if (isCreate)
            {
                // Ensure CreatedAt is set to UTC now if default
                if (product.CreatedAt == default)
                {
                    product.CreatedAt = DateTime.UtcNow;
                }
            }
        }
    }
}
