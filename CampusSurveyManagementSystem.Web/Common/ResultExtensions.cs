
using CampusSurveyManagementSystem.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace CampusSurveyManagementSystem.Web.Common;

public static class ResultExtensions
{
    public static IActionResult ToActionResult( this Result result,    ControllerBase controller)
    {
        if (result.Succeeded)
        {
            return controller.Ok();
        }

        return CreateErrorResult( controller,  result.ErrorType,  result.Errors);
    }

    public static IActionResult ToActionResult<T>(this Result<T> result,  ControllerBase controller)
    {
        if (result.Succeeded)
        {
            return controller.Ok(result.Value);
        }

        return CreateErrorResult( controller,  result.ErrorType,  result.Errors);
    }

    private static IActionResult CreateErrorResult( ControllerBase controller,  ErrorType? errorType, string? error)
    {
        var statusCode = errorType switch
        {
            ErrorType.NotFound =>  StatusCodes.Status404NotFound,

            ErrorType.Conflict =>  StatusCodes.Status409Conflict,

            ErrorType.Unauthorized =>  StatusCodes.Status401Unauthorized,

            ErrorType.Forbidden =>  StatusCodes.Status403Forbidden,

            ErrorType.Validation => StatusCodes.Status400BadRequest,

            _ =>   StatusCodes.Status400BadRequest
        };

        return new ObjectResult(new ProblemDetails
        {
            Status = statusCode,
            Title = GetTitle(statusCode),
            Detail = error
        })
        {
            StatusCode = statusCode
        };
    }

    private static string GetTitle(int statusCode)
    {
        return statusCode switch
        {
            StatusCodes.Status400BadRequest =>   "Validation failed.",

            StatusCodes.Status401Unauthorized =>  "Authentication required.",

            StatusCodes.Status403Forbidden =>   "Access denied.",

            StatusCodes.Status404NotFound =>  "Resource not found.",

            StatusCodes.Status409Conflict =>  "Request conflicts with the current state.",

            _ =>  "Request failed."
        };
    }
}