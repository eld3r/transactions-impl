using System.ComponentModel.DataAnnotations;
using Transactions.Services.Attributes;

namespace Transactions.Services.Contracts;

public record SetTransactionRequest
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [ShouldPastDate]
    public DateTime TransactionDate { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0.")]
    public decimal Amount { get; set; }
}