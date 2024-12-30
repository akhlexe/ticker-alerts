using Microsoft.Extensions.DependencyInjection;
using Quartz;
using TickerAlert.Application.Services.Prices.DolarArgentina;

namespace TickerAlert.Infrastructure.BackgroundJobs;

public sealed class CotizacionDolarReaderJob : IJob
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public CotizacionDolarReaderJob(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            CotizacionDolarReader reader = scope.ServiceProvider.GetRequiredService<CotizacionDolarReader>();

            await reader.ReadCotizacionAsync();
        }
    }
}
