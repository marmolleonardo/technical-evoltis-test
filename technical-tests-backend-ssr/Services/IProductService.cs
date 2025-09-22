using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Services
{
    public interface IProductService
    {
        Task<Product> CreateAsync(Product product, CancellationToken cancellationToken = default);
        Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Product> UpdateAsync(int id, Product product, CancellationToken cancellationToken = default);
        Task DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
