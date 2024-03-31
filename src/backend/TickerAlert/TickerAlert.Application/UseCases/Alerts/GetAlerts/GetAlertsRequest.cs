using MediatR;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.UseCases.Alerts.GetAlerts.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.UseCases.Alerts.GetAlerts;

public class GetAlertsRequest : IRequest<IEnumerable<AlertDto>> { }

public class GetAlertsRequestHandler : IRequestHandler<GetAlertsRequest, IEnumerable<AlertDto>>
{
    private readonly IAlertsReader _alertsReader;
    public GetAlertsRequestHandler(IAlertsReader alertsReader) => _alertsReader = alertsReader;

    public async Task<IEnumerable<AlertDto>> Handle(GetAlertsRequest request, CancellationToken cancellationToken)
    {
        var alerts = await _alertsReader.GetAlerts();
        var alertsDto = MapToDto(alerts);
        return alertsDto;
    }

    private static IEnumerable<AlertDto> MapToDto(IEnumerable<Alert> alerts)
    {
        return alerts.Select(a => new AlertDto()
        {
            TickerName = a.FinancialAsset.Name,
            TargetPrice = a.TargetPrice,
            ActualPrice = 500,
            Difference = a.TargetPrice - 500,
            State = "PENDING"
        });
    }
}