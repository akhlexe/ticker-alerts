namespace TickerAlert.Application.Interfaces.Authentication;

public interface ICurrentUserService
{
    Guid UserId { get; }
    bool IsAuthenticated  { get; }
}