using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TickerAlert.Application.Common.Responses;
using TickerAlert.Application.Services.Alerts.Dtos;
using TickerAlert.Application.UseCases.Alerts.CancelAlert;
using TickerAlert.Application.UseCases.Alerts.ConfirmReception;
using TickerAlert.Application.UseCases.Alerts.CreateAlert;
using TickerAlert.Application.UseCases.Alerts.GetAlerts;

namespace TickerAlert.Api.Controllers;

[Authorize]
public class AlertsController : ApiController
{
    [HttpGet]
    public async Task<IEnumerable<AlertDto>> GetAlerts()
        => await Mediator.Send(new GetAlertsRequest());

    [HttpPost("CreateAlert")]
    public async Task<ActionResult<Result>> CreateAlert([FromBody] CreateAlertCommand command)
    {
        var result = await Mediator.Send(command);

        return result.Success
            ? Ok(result)
            : BadRequest();
    }

    [HttpPost("CancelAlert")]
    public async Task<ActionResult<Result>> CancelAlert([FromBody] CancelAlertCommand command) 
        => await Mediator.Send(command);

    [HttpPost("ConfirmReception")]
    public async Task<ActionResult<Result>> ConfirmReception([FromBody] ConfirmReceptionCommand command) 
        => await Mediator.Send(command);
}