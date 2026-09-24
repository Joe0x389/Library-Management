namespace LibraryManagement.API.DTOs;

public class AuthorDto
{
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public DateTime? DateOfBirth { get; set; }
}