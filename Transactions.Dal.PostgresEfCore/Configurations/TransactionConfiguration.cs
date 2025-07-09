using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Transactions.Dal.PostgresEfCore.Model;

namespace Transactions.Dal.PostgresEfCore.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<TransactionEntity>
{
    public void Configure(EntityTypeBuilder<TransactionEntity> builder)
    {
        builder.HasKey(a => a.Id);
        builder.Property(a => a.Amount).HasPrecision(18, 4).IsRequired();
        builder.Property(a => a.TransactionDate).IsRequired();
        builder.Property(a => a.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
    }
}