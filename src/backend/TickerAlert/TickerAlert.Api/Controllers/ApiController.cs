using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TickerAlert.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ApiController : ControllerBase
{
    private ISender _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
}