using MediatR;
using TickerAlert.Application.Interfaces.Alerts;
using TickerAlert.Application.Services.Alerts.Dtos;

namespace TickerAlert.Application.UseCases.Alerts.GetAlerts;

public class GetAlertsRequest : IRequest<IEnumerable<AlertDto>> { }

public class GetAlertsRequestHandler : IRequestHandler<GetAlertsRequest, IEnumerable<AlertDto>>
{
    private readonly IAlertReader _alertReader;
    public GetAlertsRequestHandler(IAlertReader alertReader) => _alertReader = alertReader;

    public async Task<IEnumerable<AlertDto>> Handle(GetAlertsRequest request, CancellationToken cancellationToken)
    {
        return await _alertReader.GetAlerts();
    }
}