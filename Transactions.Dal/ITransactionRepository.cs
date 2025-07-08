using Transactions.Domain;

namespace Transactions.Dal;

public interface ITransactionRepository
{
    Task<DateTime> CreateAsync(Transaction transaction);
    Task<Transaction> GetAsync(Guid id);
}