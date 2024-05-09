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

    public Guid UserId => GetUserId();
    
    public bool IsAuthenticated => _httpContextAccessor
        .HttpContext?
        .User?
        .Identity?
        .IsAuthenticated ?? false;
    
    private Guid GetUserId()
    {
        if (!IsAuthenticated) return Guid.Empty;

        string strUserId = _httpContextAccessor.
            HttpContext?
            .User?
            .Claims
            .FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?
            .Value;
        
        
        return Guid.TryParse(strUserId, out Guid userId) 
            ? userId 
            : Guid.Empty;
    }
}