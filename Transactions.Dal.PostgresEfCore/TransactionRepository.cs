using Transactions.Domain;

namespace Transactions.Dal.PostgresEfCore;

public class TransactionRepository : ITransactionRepository
{
    public Task<DateTime> CreateAsync(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public Task<Transaction> GetAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}