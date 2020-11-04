using eWAY.Samples.MonkeyStore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace eWAY.Samples.MonkeyStore.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(b => b.Id);

            var navigation = builder.Metadata.FindNavigation(nameof(Transaction.Product));
            navigation.SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
