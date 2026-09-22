using LibraryManagement.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using LibraryManagement.API.Services.Interfaces;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        var response = await _authService.LoginAsync(request);
        if (!response.Succeeded)
            return Unauthorized(response.ErrorMessage);

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        var response = await _authService.RegisterAsync(request);
        if (!response.Succeeded)
            return BadRequest(response.ErrorMessage);
        
        return CreatedAtAction("Register", response);
    }

    [HttpPost("email-verification")]
    [Authorize]
    public async Task<IActionResult> GetEmailVerificationToken()
    {
        var identifier = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!int.TryParse(identifier, out int id))
            return Unauthorized();
        
        var token = await _authService.GenerateEmailVerificationTokenAsync(id);

        return Ok(token);
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token)
    {
        var success = await _authService.ConfirmEmailAsync(token);

        if (!success)
            return BadRequest("Token invalid or expired.");

        return Ok("Email confirmed successfully!");
    }

}
