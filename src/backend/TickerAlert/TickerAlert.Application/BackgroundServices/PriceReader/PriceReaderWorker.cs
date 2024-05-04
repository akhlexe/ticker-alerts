using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TickerAlert.Application.Services.PriceReader;

namespace TickerAlert.Application.BackgroundServices.PriceReader;

public class PriceReaderWorker : BackgroundService
{
    private readonly ILogger<PriceReaderWorker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PriceReaderWorker(
        ILogger<PriceReaderWorker> logger, 
        IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
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

            await Task.Delay(TimeSpan.FromMinutes(30), stoppingToken);
        }
    }
}