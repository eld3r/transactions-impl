using Mapster;
using Microsoft.Extensions.Logging;
using Transactions.Dal;
using Transactions.Domain;
using Transactions.Services.Contracts;

namespace Transactions.Services.Impl;

public class TransactionService(
    ITransactionRepository transactionRepository,
    ILogger<TransactionService> logger) : ITransactionService
{
    public async Task<SetTransactionResponse> CreateAsync(SetTransactionRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var transaction = request.Adapt<Transaction>();
        var insertDateTime = await transactionRepository.CreateAsync(transaction);
        return new SetTransactionResponse() { InsertDateTime = insertDateTime };
    }

    public async Task<GetTransactionResponse> GetAsync(Guid id)
    {
        var transaction = await transactionRepository.GetAsync(id);
        return transaction.Adapt<GetTransactionResponse>();
    }
}