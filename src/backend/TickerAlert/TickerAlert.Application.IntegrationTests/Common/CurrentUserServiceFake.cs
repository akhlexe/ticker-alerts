using TickerAlert.Application.Interfaces.Authentication;

namespace TickerAlert.Application.IntegrationTests.Common;

public class CurrentUserServiceFake : ICurrentUserService
{
    private const string CurrentUserGuid = "e2b0b4e1-21c7-4d2b-b45c-9c2b9a9f4e2a";
    
    public Guid UserId => Guid.Parse(CurrentUserGuid);
    public bool IsAuthenticated => true;
}