using LibraryManagement.API.Models.Enums;

namespace LibraryManagement.API.Models.Entities;

public class Member
{
    public int Id { get; set; }
    public string UserId { get; set; } = string.Empty; // FK -> ApplicationUser.Id
    public ApplicationUser? User { get; set; }

    public DateTime MembershipDate { get; set; } = DateTime.UtcNow;
    public MembershipStatus Status { get; set; } = MembershipStatus.Active;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public bool IsDeleted { get; set; } = false;

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
    public ICollection<Fine> Fines { get; set; } = new List<Fine>();
}
