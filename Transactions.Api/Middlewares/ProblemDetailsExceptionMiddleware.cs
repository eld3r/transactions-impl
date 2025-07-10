using Microsoft.AspNetCore.Mvc;
using Transactions.Api.Helpers;
using Transactions.Domain.Exceptions;

namespace Transactions.Api.Middlewares;

public class ProblemDetailsExceptionMiddleware(RequestDelegate next, ILogger<ProblemDetailsExceptionMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (KeyNotFoundException knfe)
        {
            //своё исключение скорее не требуется, так как id видно в Instance проблемы
            var problemDetails = new ProblemDetailsBuilder()
                .WithStatus(StatusCodes.Status404NotFound)
                .WithDetail("Transaction with provided id was not found")
                .WithInstance(context.Request.Path)
                .Build();
            
            await Write(problemDetails, context, knfe);
        }
        catch (RowLimitExceededException rlcee)
        {
            //решено в качестве кода ответа выбрать 409. 403 не подходит всё-таки из-за своего смысла недостаточного доступа
            //величину 100 можно было бы вынести в константы или даже настройки, но она в миграции с триггером, поэтому пока изменения не требуются, оставим так
            var problemDetails = new ProblemDetailsBuilder()
                .WithStatus(StatusCodes.Status409Conflict)
                .WithTitle("Row limit exceeded")
                .WithDetail("Maximum transaction count is 100")
                .WithType("https://api.example.com/probs/row-limit-exceeded")
                .WithInstance(context.Request.Path)
                .Build();
            
            await Write(problemDetails, context, rlcee);
        }
        catch (Exception ex)
        {
            var problemDetails = new ProblemDetailsBuilder()
                .WithStatus(StatusCodes.Status500InternalServerError)
                .WithDetail("An unexpected error occurred. Please try again later.")
                .WithInstance(context.Request.Path)
                .Build();
            
            await Write(problemDetails, context, ex);
        }
    }

    private async Task Write(ProblemDetails problemDetails, HttpContext context, Exception exception)
    {
        logger.LogError(exception, "{ExceptionType} occurred", exception.GetType().Name);
        
        context.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";
        await context.Response.WriteAsJsonAsync(problemDetails);
    }
}