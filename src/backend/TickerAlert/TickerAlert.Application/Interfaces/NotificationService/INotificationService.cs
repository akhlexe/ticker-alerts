namespace TickerAlert.Application.Interfaces.NotificationService;

public interface INotificationService
{
    Task Notify(string userId, string message);
}