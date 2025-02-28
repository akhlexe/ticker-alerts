using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TickerAlert.Application.Common.EventBus;
using TickerAlert.Infrastructure.EventBus.Service;
using TickerAlert.Infrastructure.EventBus.Settings;

namespace TickerAlert.Infrastructure.EventBus;

public static class DependencyInjection
{
    public static void RegisterRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RabbitMqSettings>(configuration.GetSection("RabbitMq"));
        services.AddSingleton<IEventBus, RabbitMqEventBus>();
    }
}
