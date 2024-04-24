using MediatR;
using TickerAlert.Application.Interfaces.Authentication;

namespace TickerAlert.Application.UseCases.Authentication.Login;

public record LoginRequest(string Username, string Password) : IRequest<string>;

public class LoginRequestHandler : IRequestHandler<LoginRequest, string>
{
    private readonly IAuthenticationService _authService;

    public LoginRequestHandler(IAuthenticationService authService) 
        => _authService = authService;

    public async Task<string> Handle(LoginRequest request, CancellationToken cancellationToken) 
        => await _authService.Login(request.Username, request.Password);
}