using System.IdentityModel.Tokens.Jwt;
using System.Text;
using LibraryManagement.API.Data;
using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using LibraryManagement.API.Models.Common;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly JwtSettings _jwtSettings;

    public AuthController(AppDbContext context, IPasswordHasher<ApplicationUser> passwordHasher, IOptions<JwtSettings> jwtOptions)
    {
        _context = context;
        _passwordHasher = passwordHasher;
        _jwtSettings = jwtOptions.Value;

        var user = new ApplicationUser
        {
            UserName = "Ahmed"
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, "12345");
        context.Users.Add(user);
    }

    [HttpGet("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        string userName = request.Username;
        string password = request.Password;

        var user = _context.Users.FirstOrDefault(u => u.UserName == userName);
        if (user is null)
            return Unauthorized();
        
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash ?? "", password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)   
            ]),
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(tokenHandler.WriteToken(token));
    }
}
