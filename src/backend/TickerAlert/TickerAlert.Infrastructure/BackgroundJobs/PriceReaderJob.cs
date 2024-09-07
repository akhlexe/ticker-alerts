using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using TickerAlert.Application.Services.Prices;

namespace TickerAlert.Infrastructure.BackgroundJobs;

public class PriceReaderJob(ILogger<PriceReaderJob> logger, IServiceScopeFactory serviceScopeFactory) : IJob
{
    public async Task Execute(IJobExecutionContext context)
    {
        using (var scope = serviceScopeFactory.CreateScope())
        {
            var priceReaderService = scope.ServiceProvider.GetRequiredService<PriceReaderService>();
                
            try
            {
                //await priceReaderService.ReadPricesAndSave();
            }
            catch (Exception ex)
            {
                logger.LogError($"An error occurred: {ex.Message}", ex);
            }
        }
    }
}