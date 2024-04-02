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
}