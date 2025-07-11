using Transactions.Domain;

namespace Transactions.Dal;

public interface ITransactionRepository
{
    Task<DateTime> CreateAsync(Transaction transaction);
    Task<(Transaction transaction, DateTime insertDateTime)> GetByIdAsync(Guid id);
}