using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;

namespace TickerAlert.Infrastructure.NotificationService;

public class SignalrUserIdProvider : IUserIdProvider
{
    public string? GetUserId(HubConnectionContext connection)
    {
        var userId = connection.User.FindFirstValue(JwtRegisteredClaimNames.Sub);
        
        return string.IsNullOrWhiteSpace(userId) 
            ? string.Empty 
            : userId;
    }
}