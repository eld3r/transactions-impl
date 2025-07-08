namespace Transactions.Services.Contracts;

public class SetTransactionRequest
{
    public Guid Id { get; set; }
    public Guid TransactionDate { get; set; }
    public decimal Amount { get; set; }
}