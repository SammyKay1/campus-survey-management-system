using CampusSurveyManagementSystem.Domain.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CampusSurveyManagementSystem.Web.Infrastructure.ErrorHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(
        ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync( HttpContext httpContext,  Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "An unhandled exception occurred.");

        var problemDetails = exception switch
        {
            DomainException domainException => new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Business rule violation.",
                Detail = domainException.Message,
                Instance = httpContext.Request.Path
            }, 

            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An unexpected error occurred.",
                Detail = "An unexpected error occurred while processing the request.",
                Instance = httpContext.Request.Path
            }
        };

        httpContext.Response.StatusCode =   problemDetails.Status!.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}