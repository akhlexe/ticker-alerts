namespace TickerAlert.Application.Interfaces.Authentication;

public interface ICurrentUserService
{
    int UserId { get; }
    bool IsAuthenticated  { get; }
}