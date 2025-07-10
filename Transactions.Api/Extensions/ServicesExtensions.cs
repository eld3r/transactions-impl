using Mapster;
using Transactions.Api.Services;
using Transactions.Dal.PostgresEfCore.Mapping;

namespace Transactions.Api.Extensions;

public static class ServicesExtensions
{
    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(typeof(TransactionConfig).Assembly);
        services.AddSingleton(typeAdapterConfig);
        return services;
    }
    public static IServiceCollection AddTransactionsServices(this IServiceCollection services) =>
        services
            .AddScoped<ITransactionService, TransactionService>()
    ;
}