using FakeItEasy;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using Transactions.Dal;
using Transactions.Services;
using Transactions.Services.Contracts;
using Transactions.Services.Impl;

namespace Transactions.Tests.Unit;

[TestClass]
[TestCategory("Unit")]
public class TransactionServiceTests
{
    private static ITransactionRepository _transactionRepository = null!;

    private static readonly DateTime CreatedDt = new DateTime(2025, 7, 7, 0, 0, 0, DateTimeKind.Utc);
    [ClassInitialize]
    public static void Init(TestContext context)
    {
        _transactionRepository = A.Fake<ITransactionRepository>();
    }

    private ITransactionService CreateService() =>
        new TransactionService(_transactionRepository,
            NullLogger<TransactionService>.Instance
        );

    [TestMethod]
    public async Task CreateTransactionNullRequestTest()
    {
        var target = CreateService();
        await Should.ThrowAsync<ArgumentNullException>( target.CreateAsync(null!));
    }
    
    [TestMethod]
    public async Task CreateTransactionResultMappingTest()
    {
        A.CallTo(() => _transactionRepository.CreateAsync(A<Transactions.Domain.Transaction>._))
            .ReturnsLazily((Transactions.Domain.Transaction transaction) => CreatedDt);
        
        var target = CreateService();
        var result = await target.CreateAsync(new SetTransactionRequest());
        
        result.ShouldNotBeNull();
        result.InsertDateTime.ShouldBe(CreatedDt);
    }
    
    [TestMethod]
    public async Task CreateTransactionInputMappingTest()
    {
        Transactions.Domain.Transaction calledTransaction = null!;

        A.CallTo(() => _transactionRepository.CreateAsync(A<Transactions.Domain.Transaction>._))
            .Invokes((Transactions.Domain.Transaction transaction) =>
            {
                calledTransaction = transaction;
            })
            .Returns(Task.FromResult(CreatedDt));
        
        var target = CreateService();

        var request = new SetTransactionRequest
        {
            Amount = 99.99m,
            Id = Guid.NewGuid(),
            TransactionDate = DateTime.Today
        };
        
        var result = await target.CreateAsync(request);
        
        calledTransaction.PrintToConsole().ShouldNotBeNull();
        calledTransaction.Amount.ShouldBe(request.Amount);
        calledTransaction.Id.ShouldBe(request.Id);
        calledTransaction.TransactionDate.ShouldBe(request.TransactionDate);
    }
    
    [TestMethod]
    public async Task GetTransactionResultMappingTest()
    {
        var sampleTransaction = new Transactions.Domain.Transaction
        {
            Amount = 123m,
            TransactionDate = DateTime.Today,
            Id = Guid.NewGuid()
        };
        
        A.CallTo(() =>  _transactionRepository.GetAsync(A<Guid>._))
            .ReturnsLazily((Guid transactionId) => sampleTransaction with { Id = transactionId });
        
        var target = CreateService();
        

        var result = await target.GetAsync(sampleTransaction.Id);
        
        result.ShouldNotBeNull();
        result.Amount.ShouldBe(sampleTransaction.Amount);
        result.TransactionDate.ShouldBe(sampleTransaction.TransactionDate);
        result.Id.ShouldBe(sampleTransaction.Id);
    }
}