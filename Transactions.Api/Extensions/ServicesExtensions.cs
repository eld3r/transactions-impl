using Mapster;
using Transactions.Api.Mapping;
using Transactions.Api.Services;
using Transactions.Dal.PostgresEfCore.Mapping;

namespace Transactions.Api.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(typeof(TransactionDalConfig).Assembly);
        typeAdapterConfig.Scan(typeof(TransactionServiceConfig).Assembly);
        services.AddSingleton(typeAdapterConfig);
        return services;
    }
    public static IServiceCollection AddTransactionsServices(this IServiceCollection services) =>
        services
            .AddScoped<ITransactionService, TransactionService>()
    ;
}