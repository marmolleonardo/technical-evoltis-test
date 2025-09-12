using Microsoft.EntityFrameworkCore;

namespace technical_tests_backend_ssr.Domain
{
    public class TechnicalTestDbContext : DbContext
    {
        //Reutilizar este dbContext o crear uno a medida
        public TechnicalTestDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
