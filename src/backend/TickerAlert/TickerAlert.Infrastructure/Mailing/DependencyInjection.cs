using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TickerAlert.Application.Common.Mailing;

namespace TickerAlert.Infrastructure.Mailing;

public static class DependencyInjection
{
    public static void RegisterTickerbloomEmailService(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.Configure<MailerSendSettings>(configuration.GetSection("EmailService"));

        if (environment.IsProduction())
        {
            services.AddScoped<IEmailService, MailerSendEmailService>();
        }
        else
        {
            services.AddScoped<IEmailService, FakeEmailService>();
        }


    }
}
