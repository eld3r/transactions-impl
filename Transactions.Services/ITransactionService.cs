using Transactions.Services.Contracts;

namespace Transactions.Services;

public interface ITransactionService
{
    Task<SetTransactionResponse> CreateAsync(SetTransactionRequest request);
    Task<GetTransactionResponse> GetAsync(Guid id);
}