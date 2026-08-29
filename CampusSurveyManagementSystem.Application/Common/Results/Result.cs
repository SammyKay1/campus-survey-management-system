
namespace CampusSurveyManagementSystem.Application.Common.Models;

public class Result
{
    public bool Succeeded { get; }

    public string? Errors { get; }

    public ErrorType? ErrorType { get; }

    protected Result(  bool succeeded,  string? error = null,  ErrorType? errorType = null)
    {
        Succeeded = succeeded;
        Errors = error;
        ErrorType = errorType;
    }

    public static Result Success()
    {
        return new Result(true);
    }

    public static Result Failure(string error,  ErrorType? errorType = Models.ErrorType.Validation)
    {
        return new Result( false, error,  errorType);
    }
}




public class Result<T> : Result
{
    public T? Value { get; }

    private Result( bool succeeded, T? value,  string? error,    ErrorType? errorType)   : base(succeeded, error, errorType)
    {
        Value = value;
    }

    public static Result<T> Success(T value)
    {
        return new Result<T>(  true,    value,  null,  null);
    }

    public static Result<T> Failure(  string error,  ErrorType errorType = Models.ErrorType.Validation)
    {
        return new Result<T>(false, default,  error,  errorType);
    }
}