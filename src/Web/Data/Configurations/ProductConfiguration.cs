using eWAY.Samples.MonkeyStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eWAY.Samples.MonkeyStore.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(b => b.Id);
            
            builder.Property(b => b.Text)
                   .IsRequired()
                   .HasMaxLength(256);

            builder.Property(b => b.Image)
                   .IsRequired()
                   .HasMaxLength(512);

            builder.Property(b => b.Price)
                   .IsRequired(true)
                   .HasColumnType("decimal(18,2)");
        }
    }
}
