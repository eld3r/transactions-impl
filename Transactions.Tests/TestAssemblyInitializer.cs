using Mapster;
using Microsoft.Extensions.DependencyInjection;
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
        typeAdapterConfig.Scan(typeof(TransactionConfig).Assembly);
        
        services.AddSingleton(typeAdapterConfig);
    }
}