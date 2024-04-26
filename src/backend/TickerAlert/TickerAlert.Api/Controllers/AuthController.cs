using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TickerAlert.Application.Interfaces.Authentication.Dtos;
using TickerAlert.Application.UseCases.Authentication.Login;
using TickerAlert.Application.UseCases.Authentication.Register;

namespace TickerAlert.Api.Controllers;

[AllowAnonymous]
public class AuthController : ApiController
{
    [HttpPost("Register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest command)
    {
        var response = await Mediator.Send(command);
        return Ok(response);
    }

    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest command)
    {
        var response = await Mediator.Send(command);
        return Ok(response);
    }
}