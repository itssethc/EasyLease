using EasyLease.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyLease.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Property> Properties { get; set; } = null!;
        public DbSet<Unit> Units { get; set; } = null!;
        public DbSet<Tenant> Tenants { get; set; } = null!;
        public DbSet<Lease> Leases { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Use SQLite for a self-contained demo database
            optionsBuilder.UseSqlite("Data Source=easylease.db");
        }
    }
}
