using AutoShop.Models;
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
            modelBuilder.Entity<Auto>().Property(x => x.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Auto>().Property(x => x.Name).HasMaxLength(30);
            modelBuilder.Entity<Auto>().Property(x => x.Info).HasMaxLength(300);
            modelBuilder.Entity<Auto>().Property(x => x.Price).HasColumnType("float");

            modelBuilder.Entity<User>().Property(y => y.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<User>().Property(y => y.Email).IsRequired();
            modelBuilder.Entity<User>().Property(y => y.Email).HasMaxLength(30);
            modelBuilder.Entity<User>().Property(y => y.Username).IsRequired();
            modelBuilder.Entity<User>().Property(y => y.Username).HasMaxLength(30);
            modelBuilder.Entity<User>().Property(y => y.Password).IsRequired();
            modelBuilder.Entity<User>().Property(y => y.Password).HasMaxLength(50);
            modelBuilder.Entity<User>().Property(y => y.Role).HasMaxLength(10);
        }
    }

}
