using Mapster;
using Microsoft.EntityFrameworkCore;
using Transactions.Dal.PostgresEfCore.Model;
using Transactions.Domain;

namespace Transactions.Dal.PostgresEfCore;

public class TransactionRepository(TransactionsDbContext dbContext) : ITransactionRepository
{
    public async Task<DateTime> CreateAsync(Transaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);

        var existingItem = await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == transaction.Id);
        if (existingItem != null)
        {
            return existingItem.CreatedAt ??
                   throw new Exception($"TransactionEntity.CreatedAt should not be null for existing transaction");
        }
        
        var transactionEntity = transaction.Adapt<TransactionEntity>();
        
        await dbContext.AddAsync(transactionEntity);
        await dbContext.SaveChangesAsync();
        
        //todo Excetion
        return transactionEntity.CreatedAt ?? throw new Exception("TransactionEntity.CreatedAt should not be null after insert");
    }

    public async Task<Transaction> GetAsync(Guid id)
    {
        var result = await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id);
        
        if (result == null)
            throw new KeyNotFoundException($"Transaction with id {id} not found");
        
        return result.Adapt<Transaction>();
    }
}