using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using TickerAlert.Application.Services.Prices;

namespace TickerAlert.Infrastructure.BackgroundJobs;

public class PriceReaderJob : IJob
{
    private readonly ILogger<PriceReaderJob> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    public PriceReaderJob(ILogger<PriceReaderJob> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    public async Task Execute(IJobExecutionContext context)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var priceReaderService = scope.ServiceProvider.GetRequiredService<PriceReaderService>();
                
            try
            {
                _logger.LogInformation("Running PriceReaderService.");
                await priceReaderService.ReadPricesAndSaveAsync();
                _logger.LogInformation("Price reading and saving completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}", ex);
            }
        }
    }
}