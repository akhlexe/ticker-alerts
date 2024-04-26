using TickerAlert.Application.Interfaces.Authentication.Dtos;

namespace TickerAlert.Application.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<AuthResponse> Register(string username, string password);
    Task<AuthResponse> Login(string username, string password);
}