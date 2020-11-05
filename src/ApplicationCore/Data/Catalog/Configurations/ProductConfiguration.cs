// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonkeyStore.Models;

namespace MonkeyStore.Data.Catalog.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Products");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
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
