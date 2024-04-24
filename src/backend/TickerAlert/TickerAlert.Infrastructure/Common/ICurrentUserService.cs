using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using TickerAlert.Application.Interfaces.Authentication;

namespace TickerAlert.Infrastructure.Common;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int UserId => GetUserId();
    
    public bool IsAuthenticated => _httpContextAccessor
        .HttpContext?
        .User?
        .Identity?
        .IsAuthenticated ?? false;
    
    private int GetUserId()
    {
        if (!IsAuthenticated) return 0;

        string userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);
        return int.TryParse(userId, out int userIntId) ? userIntId : 0;
    }
}