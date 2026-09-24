using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LibraryManagement.API.Data;
using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models.Common;
using LibraryManagement.API.Models.Entities;
using LibraryManagement.API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace LibraryManagement.API.Services;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly PasswordHasher<Member> _passwordHasher = new();
    private readonly JwtSettings _jwtSettings;

    public AuthService(AppDbContext context, IOptions<JwtSettings> jwtOptions)
    {
        _context = context;
        _jwtSettings = jwtOptions.Value;
    }

    public async Task<AuthResultDto> RegisterAsync(RegisterDto dto)
    {
        var name = dto.Name;
        var email = dto.Email.Trim().ToLowerInvariant();
        var password = dto.Password;

        var emailExists = await _context.Members.AnyAsync(m => m.Email == email);
        if (emailExists)
            return new AuthResultDto
            {
                ErrorMessage = "An account with this email already exists."
            };

        var member = new Member
        {
            Name = name,
            Email = email
        };
        member.PasswordHash = _passwordHasher.HashPassword(member, password);

        _context.Members.Add(member);
        await _context.SaveChangesAsync();

        var jwtToken = GenerateJwtToken(member, out var expiresAt);
        return new AuthResultDto
        {
            Succeeded = true,
            Token = jwtToken,
            ExpiresAt = expiresAt,
            Member = GetSummary(member)
        };
    }

    public async Task<AuthResultDto> LoginAsync(LoginDto dto)
    {
        var email = dto.Email.Trim().ToLowerInvariant();
        var password = dto.Password;

        var failResponse = new AuthResultDto
        {
            ErrorMessage = "E-mail or password is incorrect."
        };
        var member = await _context.Members.FirstOrDefaultAsync(m => m.Email == email);
        if (member is null)
            return failResponse;
        var result = _passwordHasher.VerifyHashedPassword(member, member.PasswordHash, password);
        if (result == PasswordVerificationResult.Failed)
            return failResponse;

        var jwtToken = GenerateJwtToken(member, out var expiresAt);
        return new AuthResultDto
        {
            Succeeded = true,
            Token = jwtToken,
            ExpiresAt = expiresAt,
            Member = GetSummary(member)
        };
    }

    public async Task<string?> GenerateEmailVerificationTokenAsync(int id)
    {
        var member = _context.Members.Find(id);

        if (member is null)
            return null;

        string token = Convert.ToHexString(RandomNumberGenerator.GetBytes(32));
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes);

        member.VerificationToken = token;
        member.TokenExpiresAt = expiresAt;
        await _context.SaveChangesAsync();

        return token;
    }

    public async Task<bool> ConfirmEmailAsync(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return false;

        var member = await _context.Members.FirstOrDefaultAsync(m => m.VerificationToken == token);
        if (member is null)
            return false;

        if (member.TokenExpiresAt < DateTime.UtcNow)
            return false;

        member.EmailConfirmed = true;
        member.VerificationToken = null;
        member.TokenExpiresAt = null;
        await _context.SaveChangesAsync();
        
        return true;
    }

    private string GenerateJwtToken(Member member, out DateTime expiresAt)
    {
        var role = member.Role;
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
        List<Claim> claims = [
            new Claim(type: ClaimTypes.Name, member.Name),
            new Claim(type: ClaimTypes.Email, member.Email),
            new Claim(type: ClaimTypes.NameIdentifier, member.Id.ToString())
        ];

        if (!string.IsNullOrWhiteSpace(role))
            claims.Add(new(type: ClaimTypes.Role, role));

        var expiry = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenExpiryMinutes);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Expires = expiry,
            SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        expiresAt = expiry;
        return tokenHandler.WriteToken(token);
    }

    private static MemberSummaryDto GetSummary(Member member)
        => new(member.Id, member.Name, member.Email, member.Role, member.Address);
}