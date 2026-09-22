using System.ComponentModel.DataAnnotations;
using LibraryManagement.API.Models.Enums;

namespace LibraryManagement.API.Models.Entities;

public class Member
{
    public int Id { get; set; }
    
    public string Name { get; set; } = string.Empty;
    [EmailAddress] public required string Email { get; set; }
    public string PasswordHash { get; set; } = "";

    public DateTime MembershipDate { get; set; } = DateTime.UtcNow;
    public MembershipStatus Status { get; set; } = MembershipStatus.Active;
    public string? Address { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool EmailConfirmed { get; set; } = false;

    public ICollection<MemberRole> MemberRoles { get; set; } = [];

    public string? VerificationToken { get; set; }
    public DateTime? TokenExpiresAt { get; set; }

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<Fine> Fines { get; set; } = new List<Fine>();
}
