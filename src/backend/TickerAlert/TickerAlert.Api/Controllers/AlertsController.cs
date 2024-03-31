using Microsoft.AspNetCore.Mvc;
using TickerAlert.Application.UseCases.Alerts.GetAlerts;
using TickerAlert.Application.UseCases.Alerts.GetAlerts.Dtos;

namespace TickerAlert.Api.Controllers;

public class AlertsController : ApiController
{
    [HttpGet]
    public async Task<IEnumerable<AlertDto>> GetAlerts()
    {
        return await Mediator.Send(new GetAlertsRequest());
    }
}