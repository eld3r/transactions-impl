using Mapster;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Transactions.Dal.PostgresEfCore.Model;
using Transactions.Domain;
using Transactions.Domain.Exceptions;

namespace Transactions.Dal.PostgresEfCore;

public class TransactionRepository(TransactionsDbContext dbContext) : ITransactionRepository
{
    public async Task<DateTime> CreateAsync(Transaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        
        var transactionEntity = transaction.Adapt<TransactionEntity>();
        
        await dbContext.AddAsync(transactionEntity);

        try
        {
            await dbContext.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (ex?.InnerException is PostgresException { SqlState: "P0003" })
        {
            throw new RowLimitExceededException();
        }

        //Пришёл к выводу, что какое-то особое исключение не потребуется
        return transactionEntity.CreatedAt?.ToLocalTime() ?? throw new Exception("TransactionEntity.CreatedAt should not be null after insert");
    }

    public async Task<(Transaction transaction, DateTime insertDateTime)> GetByIdAsync(Guid id)
    {
        var result = await dbContext.Transactions.FirstOrDefaultAsync(t => t.Id == id);
        
        if (result == null)
            return default;

        return (result.Adapt<Transaction>(),
            result.CreatedAt?.ToLocalTime() ?? throw new Exception("TransactionEntity.CreatedAt should not be null"));
    }
}