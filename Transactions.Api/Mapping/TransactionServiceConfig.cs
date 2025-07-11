using Mapster;
using Transactions.Api.Contracts;
using Transactions.Domain;

namespace Transactions.Api.Mapping;

public class TransactionServiceConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.ForType<SetTransactionRequest, Transaction>()
            .Map(dest => dest.Amount, src => Math.Round(src.Amount, 4));
    }
}