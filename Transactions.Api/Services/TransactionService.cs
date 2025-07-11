using Mapster;
using Transactions.Api.Contracts;
using Transactions.Dal;
using Transactions.Domain;

namespace Transactions.Api.Services;

public class TransactionService(
    ITransactionRepository transactionRepository, ILogger<TransactionService> logger) : ITransactionService
{
    public async Task<SetTransactionResponse> CreateAsync(SetTransactionRequest request)
    {
        //todo: Возможно, нужно переосмыслить поведение сервиса: проверку транзакции
        //на существование нужно бы осуществлять именно здесь, а так же проверять остальные 
        //параметры и выкидывать птичку, если что-то не сходится при повторной отправке
        //с другими суммой или датой
        ArgumentNullException.ThrowIfNull(request);

        logger.LogInformation("Creating a new transaction with id {Guid}", request.Id);
        
        var transaction = request.Adapt<Transaction>();
        var insertDateTime = await transactionRepository.CreateAsync(transaction);
        return new SetTransactionResponse() { InsertDateTime = insertDateTime };
    }

    public async Task<GetTransactionResponse> GetAsync(Guid id)
    {
        logger.LogInformation("Getting a transaction with id {Guid}", id);
        var transaction = await transactionRepository.GetByIdAsync(id);
        return transaction.Adapt<GetTransactionResponse>();
    }
}