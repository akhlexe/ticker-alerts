using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using TickerAlert.Application.Common.Mailing;
using TickerAlert.Application.Common.Mailing.Dtos;
using TickerAlert.Application.Common.Responses;

namespace TickerAlert.Infrastructure.Mailing;

public class MailerSendEmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly MailerSendSettings _settings;

    public MailerSendEmailService(IHttpClientFactory httpClientFactory, IOptions<MailerSendSettings> settings)
    {
        _httpClient = httpClientFactory.CreateClient();
        _settings = settings.Value;
        _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_settings.ApiKey}");
        _httpClient.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");
    }

    public async Task<Result> SendAlertTriggeredEmail(Email email, AlertTriggeredData data) 
        => await SendEmail(
            email,
            EmailTemplates.AlertTriggered,
            CreateAlertTriggeredPersonalization(email, data));

    private async Task<Result> SendEmail(Email email, string templateId, object[] personalization)
    {
        var emailData = new
        {
            from = new { email = _settings.SenderEmail, name = _settings.SenderName },
            to = new[] { new { email = email.To } },
            subject = email.Subject,
            template_id = templateId,
            personalization = personalization
        };

        var content = new StringContent(JsonConvert.SerializeObject(emailData), Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _httpClient.PostAsync(_settings.ApiBaseUrl, content);

        return response.IsSuccessStatusCode
            ? Result.SuccessResult()
            : Result.FailureResult([((string?)await response.Content.ReadAsStringAsync()) ?? string.Empty]);
    }

    private static object[] CreateAlertTriggeredPersonalization(Email email, AlertTriggeredData data)
    {
        return
        [
            new
            {
                email = email.To,
                data = new
                {
                    email = email.To,
                    company = data.Company,
                    ticker = data.Ticker,
                    targetPrice = data.TargetPrice.ToString(),
                    message= data.Message,
                    timestamp = data.TriggeredAt,
                }
            }
        ];
    }
}
