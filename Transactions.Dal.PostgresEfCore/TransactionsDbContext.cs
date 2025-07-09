using Microsoft.EntityFrameworkCore;
using Transactions.Dal.PostgresEfCore.Model;

namespace Transactions.Dal.PostgresEfCore;

public class TransactionsDbContext : DbContext
{
    public TransactionsDbContext(DbContextOptions<TransactionsDbContext> options)
        : base(options) { }

    public DbSet<TransactionEntity> Transactions => Set<TransactionEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TransactionsDbContext).Assembly);
    }
}