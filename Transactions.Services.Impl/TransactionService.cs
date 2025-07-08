using Transactions.Services.Contracts;

namespace Transactions.Services.Impl;

public class TransactionService : ITransactionService
{
    public Task<SetTransactionResponse> CreateTransactionAsync(SetTransactionRequest request)
    {
        throw new NotImplementedException();
    }

    public Task<GetTransactionResponse> GetTransactionAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}