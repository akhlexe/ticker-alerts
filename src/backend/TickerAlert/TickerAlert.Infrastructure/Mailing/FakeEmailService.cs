using Microsoft.Extensions.Logging;
using TickerAlert.Application.Common.Mailing;
using TickerAlert.Application.Common.Mailing.Dtos;
using TickerAlert.Application.Common.Responses;

namespace TickerAlert.Infrastructure.Mailing;

public sealed class FakeEmailService(ILogger<FakeEmailService> logger) : IEmailService
{
    public Task<Result> SendAlertTriggeredEmail(Email email, AlertTriggeredData data)
    {
        logger.LogInformation("Sending email for {Email}, of type AlertTriggeredData = {Data}", email, data);

        return Task.FromResult(Result.SuccessResult());
    }
}