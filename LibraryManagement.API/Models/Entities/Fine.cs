using LibraryManagement.API.Models.Enums;

namespace LibraryManagement.API.Models.Entities;

public class Fine
{
    public int Id { get; set; }

    public int MemberId { get; set; }
    public Member? Member { get; set; }

    public int? LoanId { get; set; }
    public Loan? Loan { get; set; }

    public FineReason Reason { get; set; }
    public decimal Amount { get; set; }
    public FineStatus Status { get; set; } = FineStatus.Unpaid;
    public DateTime IssuedDate { get; set; } = DateTime.UtcNow;
    public DateTime? PaidDate { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
