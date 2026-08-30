
using CampusSurveyManagementSystem.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace CampusSurveyManagementSystem.Web.Common;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result, ControllerBase controller)
    {
        if (result.Succeeded)
        {
            return controller.NoContent();
        }

        return CreateProblemDetails(controller, result);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
    {
        if (result.Succeeded)
        {
            return controller.Ok(result.Value);
        }

        return CreateProblemDetails(controller, result);
    }

    private static IActionResult CreateProblemDetails(ControllerBase controller, Result result)
    {
        var statusCode = result.ErrorType switch
        {
            ErrorType.NotFound => StatusCodes.Status404NotFound,

            ErrorType.Conflict => StatusCodes.Status409Conflict,

            ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,

            ErrorType.Forbidden => StatusCodes.Status403Forbidden,

            ErrorType.Validation => StatusCodes.Status400BadRequest,

            _ => StatusCodes.Status400BadRequest
        };

        var title = result.ErrorType switch
        {
            ErrorType.NotFound => "Resource not found.",

            ErrorType.Conflict => "Conflict.",

            ErrorType.Unauthorized => "Unauthorized.",

            ErrorType.Forbidden => "Forbidden.",

            ErrorType.Validation => "Validation failed.",

            _ => "Request failed."
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = string.Join( " ",  result.Errors), 
            Instance = controller.HttpContext.Request.Path
        };

        return controller.StatusCode( statusCode,   problemDetails);
    }
}
