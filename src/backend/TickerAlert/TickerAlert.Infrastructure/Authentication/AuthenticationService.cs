using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using TickerAlert.Application.Interfaces.Authentication;
using TickerAlert.Domain.Entities;
using TickerAlert.Infrastructure.Persistence;

namespace TickerAlert.Infrastructure.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly ApplicationDbContext _context;
    private readonly JwtSettings _jwtSettings;

    public AuthenticationService(ApplicationDbContext context, IOptions<JwtSettings> jwtSettingsOptions)
    {
        _context = context;
        _jwtSettings = jwtSettingsOptions.Value;
    }

    public async Task<string> Register(string username, string password)
    {
        var existingUser = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (existingUser != null)
        {
            return "Ya existe usuario con ese username";
        }

        var newUser = new User()
        {
            Username = username,
            HashedPassword = PasswordHelper.HashPassword(password),
            CreatedOn = DateTime.UtcNow,
            IsActive = true
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();

        return GenerateJwtToken(newUser);
    }

    public async Task<string> Login(string username, string password)
    {
        var existingUser = await _context
            .Users
            .FirstOrDefaultAsync(u => u.Username == username);

        if (existingUser == null)
        {
            return "Credenciales inválidas.";
        }

        if (existingUser.Username == username && PasswordHelper.VerifyPassword(password, existingUser.HashedPassword))
        {
            return GenerateJwtToken(existingUser);
        }

        return "Credenciales inválidas.";
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
            expires: DateTime.Now.AddMinutes(240),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}