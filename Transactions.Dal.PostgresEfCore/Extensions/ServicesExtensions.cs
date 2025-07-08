using Transactions.Dal;
using Transactions.Dal.PostgresEfCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServicesExtensions
{
    public static IServiceCollection AddTransactionsPgDal(this IServiceCollection services) =>
        services
            .AddScoped<ITransactionRepository, TransactionRepository>()
    ;
}