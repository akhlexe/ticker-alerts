using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TickerAlert.Application.Common.Mailing;

namespace TickerAlert.Infrastructure.Mailing;

public static class DependencyInjection
{
    public static void RegisterTickerbloomEmailService(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MailerSendSettings>(configuration.GetSection("EmailService"));
        services.AddScoped<IEmailService, MailerSendEmailService>();
    }
}
