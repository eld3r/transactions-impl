namespace Transactions.Domain.Exceptions;

public class RowLimitExceededException(string? message = null) : Exception(message ?? "Row limit exceeded");