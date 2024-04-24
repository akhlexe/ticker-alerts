using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TickerAlert.Application.UseCases.Authentication.Login;
using TickerAlert.Application.UseCases.Authentication.Register;

namespace TickerAlert.Api.Controllers;

[AllowAnonymous]
public class AuthController : ApiController
{
    [HttpPost("Register")]
    public async Task<ActionResult<string>> Register([FromBody] RegisterRequest command) 
        => await Mediator.Send(command);

    [HttpPost("Login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginRequest command) 
        => await Mediator.Send(command);
}