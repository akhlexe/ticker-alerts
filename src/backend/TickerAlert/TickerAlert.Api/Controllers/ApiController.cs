using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace TickerAlert.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ApiController : ControllerBase
{
    private ISender _mediator;
    public ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
}