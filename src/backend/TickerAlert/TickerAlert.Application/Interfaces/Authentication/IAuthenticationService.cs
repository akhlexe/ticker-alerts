namespace TickerAlert.Application.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<string> Register(string username, string password);
    Task<string> Login(string username, string password);
}