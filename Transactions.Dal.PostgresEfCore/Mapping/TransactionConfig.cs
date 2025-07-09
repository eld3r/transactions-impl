using Mapster;
using Transactions.Dal.PostgresEfCore.Model;
using Transactions.Domain;

namespace Transactions.Dal.PostgresEfCore.Mapping;

public class TransactionConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<Transaction, TransactionEntity>()
            .Map(dest => dest.TransactionDate, src => src.TransactionDate.ToUniversalTime());
        
        config.ForType<TransactionEntity, Transaction>()
            .Map(dest => dest.TransactionDate, src => src.TransactionDate.ToLocalTime());
    }
}