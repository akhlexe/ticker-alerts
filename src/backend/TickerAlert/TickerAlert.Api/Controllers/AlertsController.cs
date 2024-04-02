using Microsoft.AspNetCore.Mvc;
using TickerAlert.Application.Common.Responses;
using TickerAlert.Application.UseCases.Alerts.CreateAlert;
using TickerAlert.Application.UseCases.Alerts.GetAlerts;
using TickerAlert.Application.UseCases.Alerts.GetAlerts.Dtos;

namespace TickerAlert.Api.Controllers;

public class AlertsController : ApiController
{
    [HttpGet]
    public async Task<IEnumerable<AlertDto>> GetAlerts() 
        => await Mediator.Send(new GetAlertsRequest());

    [HttpPost("CreateAlert")]
    public async Task<Result> CreateAlert([FromBody]CreateAlertCommand command) 
        => await Mediator.Send(command);
}