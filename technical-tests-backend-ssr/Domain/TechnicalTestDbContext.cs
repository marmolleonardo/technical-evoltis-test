using Microsoft.EntityFrameworkCore;
using technical_tests_backend_ssr.Models;

namespace technical_tests_backend_ssr.Domain
{
    public class TechnicalTestDbContext : DbContext
    {
        public TechnicalTestDbContext(DbContextOptions<TechnicalTestDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(e => e.Price)
                    .HasPrecision(18, 2);
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime(6)")
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("datetime(6)")
                    .IsRequired(false);
            });
        }
    }
}
