namespace LibraryManagement.API.DTOs;

public class AuthResultDto
{
    public bool Succeeded { get; set; } = false;
    public string? Token { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string? ErrorMessage { get; set; }
    public MemberSummaryDto? Member { get; set; }
}