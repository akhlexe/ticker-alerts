namespace TickerAlert.Application.Interfaces.Authentication.Dtos;

public class AuthResponse
{
    public string Username { get; }
    public string Token { get; }
    public bool Success { get; }
    public string ErrorMessage { get; }
    
    private AuthResponse(string username, string token, bool success, string errorMessage)
    {
        Username = username;
        Token = token;
        Success = success;
        ErrorMessage = errorMessage;
    }

    public static AuthResponse CreateSuccessResult(string username, string token) 
        => new(username, token, true, string.Empty);

    public static AuthResponse CreateFailedResult(string errorMessage) 
        => new(string.Empty, string.Empty, false, errorMessage);
}