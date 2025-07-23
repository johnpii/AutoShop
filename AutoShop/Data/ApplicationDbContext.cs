using AutoShop.Data.Configurations;
using AutoShop.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoShop.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Auto> Autos { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AutoConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
