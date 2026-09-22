using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.API.DTOs;

public class RegisterDto
{
    public required string Name { get; set; }

    [EmailAddress(ErrorMessage = "Invalid e-mail address.")]
    [MaxLength(256)]
    public required string Email { get; set; }

    public required string Password { get; set; }
}