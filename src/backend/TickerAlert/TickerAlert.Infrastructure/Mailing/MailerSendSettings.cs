namespace TickerAlert.Infrastructure.Mailing;

public class MailerSendSettings
{
    public string ApiKey { get; set; }
    public string ApiBaseUrl { get; set; }
    public string SenderEmail { get; set; }
    public string SenderName { get; set; }
}
