using Microsoft.AspNetCore.Mvc;

namespace TickerAlert.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
    [HttpGet]
    public IActionResult TestingHello()
    {
        return Ok("Hello from container.");
    }
}