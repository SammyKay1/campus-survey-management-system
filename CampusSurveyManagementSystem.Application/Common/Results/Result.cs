
namespace CampusSurveyManagementSystem.Application.Common.Models;

public class Result
{
    public bool Succeeded { get; protected set; }

    public List<string> Errors { get; protected set; } = new();

    public ErrorType? ErrorType { get; protected set; }

    public static Result Success()
    {
        return new Result
        {
            Succeeded = true
        };
    }

    public static Result Failure(string error,  ErrorType errorType = Models.ErrorType.Validation)
    {
        return new Result
        {
            Succeeded = false,
            ErrorType = errorType,
            Errors = new List<string> { error }
        };
    }
}




public class Result<T> : Result
{
    public T? Value { get; private set; }

    public static Result<T> Success(T value)
    {
        return new Result<T>
        {
            Succeeded = true,
            Value = value
        };
    }

    public static Result<T> Failure(string error,    ErrorType errorType = Models.ErrorType.Validation)
    {
        return new Result<T>
        {
            Succeeded = false,
            ErrorType = errorType,
            Errors = new List<string> { error }
        };
    }
}