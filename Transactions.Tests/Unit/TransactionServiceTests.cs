using FakeItEasy;
using Mapster;
using Microsoft.Extensions.Logging.Abstractions;
using Shouldly;
using Transactions.Api.Contracts;
using Transactions.Api.Services;
using Transactions.Dal;
using Transactions.Domain.Exceptions;

namespace Transactions.Tests.Unit;

[TestClass]
[TestCategory("Unit")]
public class TransactionServiceTests
{
    private static readonly DateTime CreatedDt = new DateTime(2025, 7, 7, 0, 0, 0, DateTimeKind.Utc);
    
    [ClassInitialize]
    public static void Init(TestContext context)
    {
        
    }

    private ITransactionService CreateService(out ITransactionRepository repository)
    {
        repository = A.Fake<ITransactionRepository>();
        return new TransactionService(repository,
            NullLogger<TransactionService>.Instance
        );
    }

    [TestMethod]
    public async Task CreateTransactionNullRequestTest()
    {
        var target = CreateService(out _);
        await Should.ThrowAsync<ArgumentNullException>( target.CreateAsync(null!));
    }
    
    [TestMethod]
    public async Task CreateTransactionResultMappingTest()
    {
        var target = CreateService(out var repository);
        
        A.CallTo(() => repository.CreateAsync(A<Domain.Transaction>._))
            .ReturnsLazily((Transactions.Domain.Transaction _) => CreatedDt);
        
        A.CallTo(() => repository.GetByIdAsync(A<Guid>._))
            .Returns(Task.FromResult(default((Domain.Transaction, DateTime))));
        
        var result = await target.CreateAsync(new SetTransactionRequest());
        
        result.ShouldNotBeNull();
        result.InsertDateTime.ShouldBe(CreatedDt);
    }
    
    [TestMethod]
    public async Task CreateTransactionInputMappingTest()
    {
        Domain.Transaction calledTransaction = null!;

        var target = CreateService(out var repository);
        
        A.CallTo(() => repository.CreateAsync(A<Domain.Transaction>._))
            .Invokes((Domain.Transaction transaction) =>
            {
                calledTransaction = transaction;
            })
            .Returns(Task.FromResult(CreatedDt));
        
        A.CallTo(() => repository.GetByIdAsync(A<Guid>._))
            .Returns(Task.FromResult(default((Domain.Transaction, DateTime))));

        var request = new SetTransactionRequest
        {
            Amount = 99.99m,
            Id = Guid.NewGuid(),
            TransactionDate = DateTime.Today
        };
        
        await target.CreateAsync(request);
        
        calledTransaction.PrintToConsole().ShouldNotBeNull();
        calledTransaction.Amount.ShouldBe(request.Amount);
        calledTransaction.Id.ShouldBe(request.Id);
        calledTransaction.TransactionDate.ShouldBe(request.TransactionDate);
    }
    
    [TestMethod]
    public async Task GetTransactionResultMappingTest()
    {
        var sampleTransaction = new Domain.Transaction
        {
            Amount = 123m,
            TransactionDate = DateTime.Today,
            Id = Guid.NewGuid()
        };
        var target = CreateService(out var repository);
        
        A.CallTo(() => repository.GetByIdAsync(A<Guid>._))
            .ReturnsLazily((Guid transactionId) => (sampleTransaction with { Id = transactionId }, DateTime.Now));

        var result = await target.GetAsync(sampleTransaction.Id);
        
        result.ShouldNotBeNull();
        result.Amount.ShouldBe(sampleTransaction.Amount);
        result.TransactionDate.ShouldBe(sampleTransaction.TransactionDate);
        result.Id.ShouldBe(sampleTransaction.Id);
    }
    
    [TestMethod]
    public async Task CreateSameRepeatedTransactionTest()
    {
        var target = CreateService(out var repository);

        var insertDate = DateTime.Today.AddTicks(-12345);
        
        var request = new Domain.Transaction
        {
            Amount = 99.99m,
            Id = Guid.NewGuid(),
            TransactionDate = DateTime.Today
        };
        
        A.CallTo(() => repository.GetByIdAsync(A<Guid>._))
            .Returns(Task.FromResult((request, insertDate)));

        var result = await target.CreateAsync(request.Adapt<SetTransactionRequest>());
        result.InsertDateTime.ShouldBe(insertDate);
        
        A.CallTo(() => repository.CreateAsync(A<Domain.Transaction>._)).MustNotHaveHappened();
    }
    
    [TestMethod]
    public async Task CreateDifferentRepeatedTransactionTest()
    {
        var target = CreateService(out var repository);
        
        var request = new Domain.Transaction
        {
            Amount = 99.99m,
            Id = Guid.NewGuid(),
            TransactionDate = DateTime.Today
        };
        
        A.CallTo(() => repository.GetByIdAsync(A<Guid>._))
            .Returns(Task.FromResult((request with { Amount = 99.999m }, DateTime.Now)));

        await Should.ThrowAsync<TransactionConflictException>(
            target.CreateAsync(request.Adapt<SetTransactionRequest>()));
        
        A.CallTo(() => repository.GetByIdAsync(A<Guid>._))
            .Returns(Task.FromResult((request with { TransactionDate = request.TransactionDate.AddMicroseconds(1) }, DateTime.Now)));

        await Should.ThrowAsync<TransactionConflictException>(
            target.CreateAsync(request.Adapt<SetTransactionRequest>()));
        
        A.CallTo(() => repository.CreateAsync(A<Domain.Transaction>._)).MustNotHaveHappened();
    }
}