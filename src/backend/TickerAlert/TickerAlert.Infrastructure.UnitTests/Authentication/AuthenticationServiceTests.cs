using Microsoft.Extensions.Options;
using TickerAlert.Infrastructure.Authentication;
using TickerAlert.Infrastructure.Persistence;
using TickerAlert.Infrastructure.UnitTests.Common.Persistence;

namespace TickerAlert.Infrastructure.UnitTests.Authentication;

public class AuthenticationServiceTests
{
    private readonly ApplicationDbContext _context;
    private readonly JwtSettings _jwtSettings;
    private readonly AuthenticationService _authenticationService;

    public AuthenticationServiceTests()
    {
        _context = DbContextInMemory.Create();
        _jwtSettings = new JwtSettings
        {
            Key = "supersecretkeyyoushouldnotcommittogithub",
            Issuer = "https://api.yourdomain.com",
            Audience = "https://api.yourdomain.com"
        };
        IOptions<JwtSettings> jwtSettingsOptions = Options.Create(_jwtSettings);
        _authenticationService = new AuthenticationService(_context, jwtSettingsOptions);
    }
    
    [Fact]
    public async Task Register_ShouldReturnSuccessResult_WhenUsernameIsAvailable()
    {
        // Arrange
        var username = "newuser";
        var password = "password";

        // Act
        var result = await _authenticationService.Register(username, password);

        // Assert
        Assert.True(result.Success);
        Assert.Equal(username, result.Username);
        Assert.NotNull(result.Token);
    }
    
    [Fact]
    public async Task Register_ShouldReturnFailedResult_WhenUsernameAlreadyTaken()
    {
        // Arrange
        var username = "testuser";
        var password = "password";

        var response = await _authenticationService.Register(username, password);
        Assert.True(response.Success);

        // Act
        var result = await _authenticationService.Register(username, password);

        // Assert
        Assert.False(result.Success);
        Assert.Equal("Username already taken.", result.ErrorMessage);
    }
}