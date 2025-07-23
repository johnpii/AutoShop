using AutoShop.Entities;
using AutoShop.Helpers;
using AutoShop.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoShop.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(y => y.Id).ValueGeneratedOnAdd();
            builder.Property(y => y.Email).IsRequired().HasMaxLength(30);
            builder.Property(y => y.Username).IsRequired().HasMaxLength(30);
            builder.Property(y => y.Password).IsRequired().HasMaxLength(50);
            builder.Property(y => y.Role).HasMaxLength(10);

            // Добавляем начальные данные
            builder.HasData(
                new User
                {
                    Id = 1,
                    Email = ConfigurationHelper.config.GetSection("AdminDefaultEmail").Value,
                    Username = "Admin",
                    Password = PasswordEncryption.EncryptPassword(ConfigurationHelper.config.GetSection("AdminDefaultPassword").Value),
                    Role = "admin"
                }
            );
        }
    }
}
