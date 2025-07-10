using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Transactions.Api.Extensions;
using Transactions.Api.Helpers;
using Transactions.Api.Middlewares;
using Transactions.Dal.PostgresEfCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
builder.Services.AddControllers();

builder.Services
    .AddTransactionsServices()
    .AddTransactionsPgDal(builder.Configuration)
    .AddMapster()
    ;

builder.Services.AddLogging();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var problemDetails = new ProblemDetailsBuilder()
            .WithStatus(StatusCodes.Status422UnprocessableEntity)
            .WithTitle("One or more validation errors occurred.")
            .WithType("https://api.example.com/probs/validation")
            .WithInstance(context.HttpContext.Request.Path)
            .Build(context.ModelState);

        return new UnprocessableEntityObjectResult(problemDetails)
        {
            ContentTypes = { "application/problem+json" }
        };
    };
});

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/app.log", rollingInterval: RollingInterval.Day)
    // .WriteTo.Elasticsearch - как дальнейшее развитие
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TransactionsDbContext>();
    await db.Database.MigrateAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.EnvironmentName == "localhost")
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(opts => opts
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseMiddleware<ProblemDetailsExceptionMiddleware>();
app.MapControllers();
app.Run();