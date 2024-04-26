namespace TickerAlert.Application.Interfaces.Authentication.Dtos;

public class AuthResponse
{
    public string Token { get; }
    public bool Success { get; }
    public string ErrorMessage { get; }
    
    private AuthResponse(string token, bool success, string errorMessage)
    {
        Token = token;
        Success = success;
        ErrorMessage = errorMessage;
    }

    public static AuthResponse CreateSuccessResult(string token) 
        => new(token, true, string.Empty);

    public static AuthResponse CreateFailedResult(string errorMessage) 
        => new(string.Empty, false, errorMessage);
}