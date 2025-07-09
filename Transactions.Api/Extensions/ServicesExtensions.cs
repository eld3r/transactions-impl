using Mapster;
using Transactions.Dal.PostgresEfCore.Mapping;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServicesExtensions
{
    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(typeof(TransactionConfig).Assembly);
        services.AddSingleton(typeAdapterConfig);
        return services;
    }
}