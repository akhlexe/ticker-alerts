using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Application.Interfaces.Authentication.Dtos;
using TickerAlert.Domain.Entities;
using TickerAlert.Infrastructure.Persistence;

namespace TickerAlert.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private const int ExpirationTime = 240;
    private readonly ApplicationDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public AuthenticationService(ApplicationDbContext context, IOptions<JwtSettings> jwtSettingsOptions)
    {
        _context = context;
        _jwtSettings = jwtSettingsOptions.Value;
    }

    public async Task<AuthResponse> Register(string username, string password)
    {
        var existingUser = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (existingUser != null)
        {
            return AuthResponse.CreateFailedResult("Username already taken.");
        }

        var newUser = User.CreateUser(Guid.NewGuid(), username, PasswordHelper.HashPassword(password));
        
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        string token = GenerateJwtToken(newUser);
        return AuthResponse.CreateSuccessResult(newUser.Username, token);
    }

    public async Task<AuthResponse> Login(string username, string password)
    {
        var existingUser = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (existingUser != null && VerifyUserCredentials(username, password, existingUser))
        {
            string token = GenerateJwtToken(existingUser);
            return AuthResponse.CreateSuccessResult(existingUser.Username, token);
        }

        return AuthResponse.CreateFailedResult("Invalid credentials.");
    }

    private string GenerateJwtToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iss, "https://api.yourdomain.com"),
            new Claim(JwtRegisteredClaimNames.Aud, "https://api.yourdomain.com"),
        };

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(ExpirationTime),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    private static bool VerifyUserCredentials(string username, string password, User existingUser)
    {
        return existingUser.Username == username && PasswordHelper.VerifyPassword(password, existingUser.HashedPassword);
    }
}