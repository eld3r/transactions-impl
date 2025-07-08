using Transactions.Services.Contracts;

namespace Transactions.Services;

public interface ITransactionService
{
    Task<SetTransactionResponse> CreateTransactionAsync(SetTransactionRequest request);
    Task<GetTransactionResponse> GetTransactionAsync(Guid id);
}