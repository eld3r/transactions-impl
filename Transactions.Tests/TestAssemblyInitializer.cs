using Mapster;
using Microsoft.Extensions.DependencyInjection;
using Transactions.Api.Mapping;
using Transactions.Dal.PostgresEfCore.Mapping;

namespace Transactions.Tests;

[TestClass]
public static class TestAssemblyInitializer
{
    [AssemblyInitialize]
    public static void Init(TestContext context)
    {
        var services = new ServiceCollection();
        
        var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        typeAdapterConfig.Scan(typeof(TransactionDalConfig).Assembly);
        typeAdapterConfig.Scan(typeof(TransactionServiceConfig).Assembly);
        
        services.AddSingleton(typeAdapterConfig);
    }
}