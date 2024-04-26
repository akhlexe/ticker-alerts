using MediatR;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.Authentication.Dtos;

namespace TickerAlert.Application.UseCases.Authentication.Register;

public record RegisterRequest(string Username, string Password) : IRequest<AuthResponse>;

public class RegisterRequestHandler : IRequestHandler<RegisterRequest, AuthResponse>
{
    private readonly IAuthenticationService _authService;
    
    public RegisterRequestHandler(IAuthenticationService authService) 
        => _authService = authService;

    public async Task<AuthResponse> Handle(RegisterRequest request, CancellationToken cancellationToken) 
        => await _authService.Register(request.Username, request.Password);
}