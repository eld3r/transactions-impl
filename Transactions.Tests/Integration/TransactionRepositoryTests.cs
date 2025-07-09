using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Transactions.Dal;
using Transactions.Dal.PostgresEfCore;
using Transactions.Dal.PostgresEfCore.Model;
using Transactions.Domain;

namespace Transactions.Tests.Integration;

[TestClass]
[TestCategory("Integration")]
public class TransactionRepositoryTests
{
    private static ServiceProvider _serviceProvider = null!;
    
    [ClassInitialize]
    public static async Task ClassInitialize(TestContext context)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("testconfig.localhost.json")
            .Build();

        var services = new ServiceCollection();
        services.AddTransactionsPgDal(configuration);

        _serviceProvider = services.BuildServiceProvider();

        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TransactionsDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.MigrateAsync();
    }
    
    [TestMethod]
    public async Task CreateNullTest()
    {
        var target = _serviceProvider.GetRequiredService<ITransactionRepository>();

        await Should.ThrowAsync<ArgumentNullException>(target.CreateAsync(null!));
    }
    
    [TestMethod]
    public async Task CreateTest()
    {
        var target = _serviceProvider.GetRequiredService<ITransactionRepository>();

        var transaction = new Domain.Transaction()
            { Id = Guid.NewGuid(), Amount = 100, TransactionDate = DateTime.Now };
        
        await target.CreateAsync(transaction);
        
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<TransactionsDbContext>();
        
        var transactionEntity = dbContext.Transactions
            .ShouldHaveSingleItem()
            .PrintToConsole();
        transactionEntity.Id.ShouldBe(transaction.Id);
        transactionEntity.Amount.ShouldBe(transaction.Amount);
        transactionEntity.TransactionDate.ShouldBe(transaction.TransactionDate.ToUniversalTime().DropSeventhDigit());
        transactionEntity.CreatedAt.ShouldNotBeNull()
            .ShouldBeGreaterThan(DateTime.Now.AddMinutes(-1).ToUniversalTime());
    }
    
    [TestMethod]
    public async Task GetByIdTest()
    {
        var target = _serviceProvider.GetRequiredService<ITransactionRepository>();

        var transactionEntity = new TransactionEntity()
            { Id = Guid.NewGuid(), Amount = 4356, TransactionDate = DateTime.Now.ToUniversalTime() };
        
        using (var scope = _serviceProvider.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<TransactionsDbContext>();
            await dbContext.AddAsync(transactionEntity);
            await dbContext.SaveChangesAsync();
        }
        
        var result = await target.GetAsync(transactionEntity.Id);
        
        result
            .ShouldNotBeNull()
            .PrintToConsole();
        
        result.Id.ShouldBe(transactionEntity.Id);
        result.Amount.ShouldBe(transactionEntity.Amount);
        result.TransactionDate.ShouldBe(transactionEntity.TransactionDate.ToLocalTime().DropSeventhDigit());
    }

    [TestMethod]
    public async Task CreateTransactionsOverLimitTest()
    {
        var dbContext = _serviceProvider.GetRequiredService<TransactionsDbContext>();
        dbContext.Transactions.RemoveRange(dbContext.Transactions);
        await dbContext.SaveChangesAsync();
        
        var target = _serviceProvider.GetRequiredService<ITransactionRepository>();

        var i = 0;
        for (; i < 200; i++)
            try
            {
                await target.CreateAsync(new Transaction
                    { Id = Guid.NewGuid(), Amount = 1000 + i, TransactionDate = DateTime.Now });
            }
            catch(DbUpdateException ex)
            {
                break;
            }
        
        i.ShouldBe(100);
    }
}