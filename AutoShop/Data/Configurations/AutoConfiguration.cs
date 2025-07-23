using AutoShop.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoShop.Data.Configurations
{
    public class AutoConfiguration : IEntityTypeConfiguration<Auto>
    {
        public void Configure(EntityTypeBuilder<Auto> builder)
        {
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Name).HasMaxLength(30);
            builder.Property(x => x.Info).HasMaxLength(300);
            builder.Property(x => x.Price).HasColumnType("float");
        }
    }
}
