using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Transactions.Dal;
using Transactions.Dal.PostgresEfCore;

namespace Microsoft.Extensions.DependencyInjection;

public static class ServicesExtensions
{
    public static IServiceCollection AddTransactionsPgDal(this IServiceCollection services,
        IConfiguration configuration) =>
        services
            .AddDbContext<TransactionsDbContext>((prov, options) =>
                    options
                        .UseNpgsql(configuration.GetConnectionString("postgres"))
                        .UseSnakeCaseNamingConvention()
#if DEBUG
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
#endif
            )
            .AddScoped<ITransactionRepository, TransactionRepository>()
    ;
}