using Transactions.Api.Contracts;

namespace Transactions.Api.Services;

public interface ITransactionService
{
    Task<SetTransactionResponse> CreateAsync(SetTransactionRequest request);
    Task<GetTransactionResponse> GetAsync(Guid id);
}