using Transactions.Services;
using Transactions.Services.Impl;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServicesExtensions
{
    public static IServiceCollection AddArticlesServices(this IServiceCollection services) =>
        services
            .AddScoped<ITransactionService, TransactionService>()
    ;
}