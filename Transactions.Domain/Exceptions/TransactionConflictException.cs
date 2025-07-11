namespace Transactions.Domain.Exceptions;

public class TransactionConflictException(Guid id, string? message = null) : Exception(message ?? $"Different data provided for transaction with id {id}");