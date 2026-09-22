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
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;

namespace LibraryManagement.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<Member> _passwordHasher = new();
    private readonly JwtSettings _jwtSettings;

    public AuthController(AppDbContext context, IOptionsSnapshot<JwtSettings> jwtOptions)
    {
        _context = context;
        _jwtSettings = jwtOptions.Value;
    }

    [HttpGet("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        var password = request.Password;

        var user = _context.Members.FirstOrDefault(m => m.Email == email);
        if (user is null)
            return Unauthorized();
        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
            return Unauthorized();

        string jwtToken = GetJwtToken(user);
        return Ok(jwtToken);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var name = request.Name;
        var email = request.Email.Trim().ToLowerInvariant();
        var password = request.Password;

        var emailExists = await _context.Members.AnyAsync(m => m.Email == email);
        if (emailExists)
        {
            return BadRequest("An account with this email already exists.");
        }

        var member = new Member
        {
            Name = name,
            Email = email
        };
        member.PasswordHash = _passwordHasher.HashPassword(member, password);

        _context.Members.Add(member);
        await _context.SaveChangesAsync();
        return Created();
    }

    [HttpPost("email-verification")]
    public async Task<IActionResult> GetEmailVerificationToken()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? User.FindFirstValue("sub")
                    ?? User.FindFirstValue("nameid");

        if (userId is null)
            return Unauthorized("User ID claim not found in token");

        int id = Convert.ToInt32(userId);
        string token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes);

        var member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);
        if (member is null)
            return Unauthorized("Invalid User ID");

        member.VerificationToken = token;
        member.TokenExpiresAt = expiresAt;
        await _context.SaveChangesAsync();

        return Ok(token);
    }

    [HttpGet("confirm-email")]
    public IActionResult ConfirmEmail([FromQuery] string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return BadRequest("Invalid token.");

        var member = _context.Members.FirstOrDefault(m => m.VerificationToken == token);
        if (member is null)
            return BadRequest("Invalid verification link.");

        if (member.TokenExpiresAt < DateTime.UtcNow)
            return BadRequest("Verification link has expired.");

        member.EmailConfirmed = true;
        member.VerificationToken = null;
        member.TokenExpiresAt = null;
        _context.SaveChanges();

        return Ok("Email confirmed successfully!");
    }

    private string GetJwtToken(Member member)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(type: ClaimTypes.Name, member.Name),
                new Claim(type: ClaimTypes.NameIdentifier, member.Id.ToString())
            ]),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes),
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
