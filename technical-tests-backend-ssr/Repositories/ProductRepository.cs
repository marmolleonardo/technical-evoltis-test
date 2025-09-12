using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Domain;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly TechnicalTestDbContext _db;

        public ProductRepository(TechnicalTestDbContext db)
        {
            _db = db;
        }

        public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
        {
            await _db.Products.AddAsync(product, cancellationToken);
            await _db.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task<Product?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _db.Products.AsNoTracking().OrderBy(p => p.Id).ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(Product product, CancellationToken cancellationToken = default)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _db.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (entity is null)
            {
                return;
            }
            _db.Products.Remove(entity);
            await _db.SaveChangesAsync(cancellationToken);
        }
    }
}
