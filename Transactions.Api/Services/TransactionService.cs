using Mapster;
using Transactions.Api.Contracts;
using Transactions.Dal;
using Transactions.Domain;
using Transactions.Domain.Exceptions;

namespace Transactions.Api.Services;

public class TransactionService(
    ITransactionRepository transactionRepository, ILogger<TransactionService> logger) : ITransactionService
{
    public async Task<SetTransactionResponse> CreateAsync(SetTransactionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        
        var transaction = request.Adapt<Transaction>();
        
        var existingItem = await transactionRepository.GetByIdAsync(request.Id);
        if (existingItem != default)
        {
            if (!existingItem.transaction.Equals(transaction)) 
                throw new TransactionConflictException(request.Id);
            
            logger.LogInformation("Transaction with id {Guid} and same data already exists", request.Id);
            return new SetTransactionResponse() { InsertDateTime = existingItem.insertDateTime };
        }
        
        logger.LogInformation("Creating a new transaction with id {Guid}", request.Id);
        
        var insertDateTime = await transactionRepository.CreateAsync(transaction);
        return new SetTransactionResponse() { InsertDateTime = insertDateTime };
    }

    public async Task<GetTransactionResponse> GetAsync(Guid id)
    {
        logger.LogInformation("Getting a transaction with id {Guid}", id);
        var transaction = await transactionRepository.GetByIdAsync(id);
        
        if (transaction == default)
            throw new KeyNotFoundException($"Transaction with id {id} was not found");
        
        return transaction.transaction.Adapt<GetTransactionResponse>();
    }
}