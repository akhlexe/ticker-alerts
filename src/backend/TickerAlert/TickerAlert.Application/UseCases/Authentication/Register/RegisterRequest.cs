using MediatR;
using TickerAlert.Application.Interfaces.Authentication;

namespace TickerAlert.Application.UseCases.Authentication.Register;

public record RegisterRequest(string Username, string Password) : IRequest<string>;

public class RegisterRequestHandler : IRequestHandler<RegisterRequest, string>
{
    private readonly IAuthenticationService _authService;
    
    public RegisterRequestHandler(IAuthenticationService authService) 
        => _authService = authService;

    public async Task<string> Handle(RegisterRequest request, CancellationToken cancellationToken) 
        => await _authService.Register(request.Username, request.Password);
}