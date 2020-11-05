// Copyright (c) eWAY and Contributors. All rights reserved.
// Licensed under the MIT License

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MonkeyStore.Models;

namespace MonkeyStore.Data.Catalog.Configurations
{
    public class PaymentTransactionConfiguration : IEntityTypeConfiguration<PaymentTransaction>
    {
        public void Configure(EntityTypeBuilder<PaymentTransaction> builder)
        {
            builder.ToTable("Transactions");

            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Order)
                   .WithOne()
                   .HasForeignKey(nameof(PaymentTransaction.OrderId))
                   .IsRequired()
                   .HasConstraintName("FK_transaction2order")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
