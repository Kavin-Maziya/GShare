using GearShare.Api.DTOs;
using GearShare.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GearShare.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    
    // Login and generate a JWT token.
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        => Ok(await authService.LoginAsync(dto));
}