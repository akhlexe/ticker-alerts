using TickerAlert.Application.Common.Mailing.Dtos;
using TickerAlert.Application.Common.Responses;

namespace TickerAlert.Application.Common.Mailing;

public interface IEmailService
{
    Task<Result> SendAlertTriggeredEmail(Email email, AlertTriggeredData data);
}
