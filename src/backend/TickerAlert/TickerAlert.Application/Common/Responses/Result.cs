namespace TickerAlert.Application.Common.Responses;

public class Result
{
    public bool Success { get; private set; }
    public string[] Errors { get; private set; }

    private Result(bool success, string[] errors)
    {
        this.Success = success;
        this.Errors = errors;
    }

    public static Result SuccessResult() => new(true, null);
    public static Result FailureResult(string[] errors) => new(false, errors);
    public static Result FailureResult(string error) => new(false, [error]);
}

public class Result<T>
{
    public T? Data { get; private set; }
    public bool Success { get; private set; }
    public string[] Errors { get; private set; }

    private Result(bool success, string[] errors, T? data)
    {
        this.Success = success;
        this.Errors = errors;
        this.Data = data;
    }

    public static Result<T> SuccessResult(T data) => new(true, Array.Empty<string>(), data);
    public static Result<T> FailureResult(params string[] errors) => new(false, errors, default);
    public static Result<T> FailureResult(string error) => new(false, [error], default);
}