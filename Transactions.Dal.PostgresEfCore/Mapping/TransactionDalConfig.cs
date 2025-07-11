using Mapster;
using Transactions.Dal.PostgresEfCore.Model;
using Transactions.Domain;

namespace Transactions.Dal.PostgresEfCore.Mapping;

public class TransactionDalConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<Transaction, TransactionEntity>()
            .Map(dest => dest.TransactionDate, src => src.TransactionDate.ToUniversalTime());
    }
}