namespace LibraryManagement.Entities;

public static class FineStatus
{
    public const string Unpaid = "Unpaid";
    public const string PartiallyPaid = "PartiallyPaid";
    public const string Paid = "Paid";

    public static readonly string[] All = { Unpaid, PartiallyPaid, Paid };
}

public class Fine
{
    public int Id { get; set; }
    public int LoanId { get; set; }
    public int MemberId { get; set; }
    public decimal Amount { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = FineStatus.Unpaid;
    public DateTime CreatedAt { get; set; }

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();

    // خصائص محسوبة (مش أعمدة في الداتا بيز)
    public decimal PaidAmount => Payments.Sum(p => p.Amount);
    public decimal RemainingAmount => Amount - PaidAmount;

    /// بيحدّث الـ Status حسب المدفوع
    public void RecalculateStatus()
    {
        var paid = PaidAmount;
        Status = paid <= 0 ? FineStatus.Unpaid
               : paid >= Amount ? FineStatus.Paid
               : FineStatus.PartiallyPaid;
    }
}
