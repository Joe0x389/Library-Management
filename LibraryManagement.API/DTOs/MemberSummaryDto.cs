namespace LibraryManagement.API.DTOs;

public record MemberSummaryDto(int Id, string Name, string Email, string? Role, string? Address);