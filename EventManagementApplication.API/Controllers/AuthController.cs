using EventManagementSystem.Application.Contracts.Services;
using EventManagementSystem.Application.Models.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace EventManagementApplication.API.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthenticationService _authenticationService;

    public AuthController(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService
            ?? throw new ArgumentNullException(nameof(authenticationService));
    }

    [HttpPost("register")]
    public async Task<ActionResult<RegisterUserResponse>> RegisterAsync(RegisterUserRequest request)
    {
        return Ok(await _authenticationService.RegisterUserAsync(request));
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginUserResponse>> LoginAsync(LoginUserRequest request)
    {
        return Ok(await _authenticationService.LoginUserAsync(request));
    }
}
