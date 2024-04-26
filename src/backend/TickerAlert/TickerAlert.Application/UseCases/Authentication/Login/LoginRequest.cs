using MediatR;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.Authentication.Dtos;

namespace TickerAlert.Application.UseCases.Authentication.Login;

public record LoginRequest(string Username, string Password) : IRequest<AuthResponse>;

public class LoginRequestHandler : IRequestHandler<LoginRequest, AuthResponse>
{
    private readonly IAuthenticationService _authService;

    public LoginRequestHandler(IAuthenticationService authService) 
        => _authService = authService;

    public async Task<AuthResponse> Handle(LoginRequest request, CancellationToken cancellationToken) 
        => await _authService.Login(request.Username, request.Password);
}