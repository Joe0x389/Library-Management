namespace LibraryManagement.API.Models.Entities;

public class Payment
{
    public int Id { get; set; }

    public int FineId { get; set; }
    public Fine? Fine { get; set; }

    public decimal Amount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
}
