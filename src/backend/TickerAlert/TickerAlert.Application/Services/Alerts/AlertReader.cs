using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Services.Alerts.Dtos;

namespace TickerAlert.Application.Services.Alerts;

public class AlertReader : IAlertReader
{
    private readonly IAlertRepository _repository;
    public AlertReader(IAlertRepository repository) => _repository = repository;

    public async Task<IEnumerable<AlertDto>> GetAlerts()
    {
        var alerts = await _repository.GetAll();

        return alerts.Select(a => new AlertDto()
        {
            TickerName = a.FinancialAsset.Ticker,
            TargetPrice = a.TargetPrice,
            ActualPrice = 1000,
            Difference = a.TargetPrice - 1000,
            State = "PENDING"
        });
    }
}