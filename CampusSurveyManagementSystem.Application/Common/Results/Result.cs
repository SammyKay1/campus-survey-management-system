
namespace CampusSurveyManagementSystem.Application.Common.Models;

public class Result
{
    public bool Succeeded { get; }

    public IReadOnlyCollection<string> Errors { get; }

    protected Result(bool succeeded, IEnumerable<string>? errors = null)
    {
        Succeeded = succeeded;
        Errors = errors?.ToArray()  ?? Array.Empty<string>();
    }

    public static Result Success()  => new(true);

    public static Result Failure( params string[] errors) => new(false, errors);
}




public class Result<T> : Result
{
    public T? Value { get; }

    private Result( bool succeeded,  T? value,   IEnumerable<string>? errors)  : base(succeeded, errors)
    {
        Value = value;
    }

    public static Result<T> Success(T value)  => new(true, value, null);

    public static Result<T> Failure( params string[] errors) => new(false, default, errors);
}