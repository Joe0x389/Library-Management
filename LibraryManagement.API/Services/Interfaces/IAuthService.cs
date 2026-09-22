using LibraryManagement.API.DTOs;

namespace LibraryManagement.API.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> RegisterAsync(RegisterDto dto);
    Task<AuthResultDto> LoginAsync(LoginDto dto);
    Task<bool> ConfirmEmailAsync(string token);
    Task<string?> GenerateEmailVerificationTokenAsync(int id);
}