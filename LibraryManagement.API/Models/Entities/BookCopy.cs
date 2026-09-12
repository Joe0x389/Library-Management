using LibraryManagement.API.Models.Enums;

namespace LibraryManagement.API.Models.Entities;

public class BookCopy
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public Book? Book { get; set; }

    public string CopyNumber { get; set; } = string.Empty;
    public CopyStatus Status { get; set; } = CopyStatus.Available;
    public string? ShelfLocation { get; set; }
    public DateTime AcquisitionDate { get; set; } = DateTime.UtcNow;
    public string? Condition { get; set; }

    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
