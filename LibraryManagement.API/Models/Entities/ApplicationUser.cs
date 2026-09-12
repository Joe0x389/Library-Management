using Microsoft.AspNetCore.Identity;

namespace LibraryManagement.API.Models.Entities;

// Extends ASP.NET Core Identity's built-in user. Auth-related fields (email, password hash,
// lockout, etc.) are already handled by IdentityUser - only add library-specific fields here.
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;

    public Member? Member { get; set; }
}
