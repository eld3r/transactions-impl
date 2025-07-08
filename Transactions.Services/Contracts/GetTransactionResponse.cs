namespace Transactions.Services.Contracts;

public class GetTransactionResponse
{
    public Guid Id { get; set; }
    public Guid TransactionDate { get; set; }
    public decimal Amount { get; set; }
}