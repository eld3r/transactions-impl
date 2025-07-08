namespace Transactions.Services.Contracts;

public record GetTransactionResponse
{
    public Guid Id { get; set; }
    public DateTime TransactionDate { get; set; }
    public decimal Amount { get; set; }
}