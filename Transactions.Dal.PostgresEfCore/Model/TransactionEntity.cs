namespace Transactions.Dal.PostgresEfCore.Model;

public class TransactionEntity
{
    public Guid Id { get; set; } 
    public DateTime TransactionDate { get; set; } 
    public decimal Amount { get; set; }
    public DateTime? CreatedAt { get; set; }
}