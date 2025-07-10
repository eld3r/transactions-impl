using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;

namespace Transactions.Api.Helpers;

public class ProblemDetailsBuilder()
{
    private int? _statusCode;
    private string? _title;
    private string? _detail;
    private string? _type;
    private string? _instance;

    public ProblemDetailsBuilder WithStatus(int statusCode)
    {
        _statusCode = statusCode;
        return this;
    }

    public ProblemDetailsBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public ProblemDetailsBuilder WithDetail(string detail)
    {
        _detail = detail;
        return this;
    }

    public ProblemDetailsBuilder WithType(string type)
    {
        _type = type;
        return this;
    }
    
    public ProblemDetailsBuilder WithInstance(string instance)
    {
        _instance = instance;
        return this;
    }
    
    public ProblemDetails Build()
    {
        var problem = new ProblemDetails();
        Populate(problem);
        return problem;
    }

    public ValidationProblemDetails Build(ModelStateDictionary modelState)
    {
        var problem = new ValidationProblemDetails(modelState);
        Populate(problem);
        return problem;
    }

    private void Populate(ProblemDetails problem)
    {
        var status = _statusCode ?? StatusCodes.Status500InternalServerError;
        problem.Status = status;
        problem.Title = _title ?? ReasonPhrases.GetReasonPhrase(status);
        problem.Detail = _detail;
        problem.Type = _type ?? $"https://httpstatuses.com/{status}";
        problem.Instance = _instance;
    }
}