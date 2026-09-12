using LibraryManagement.API.Models.Enums;

namespace LibraryManagement.API.Models.Entities;

public class Reservation
{
    public int Id { get; set; }

    public int MemberId { get; set; }
    public Member? Member { get; set; }

    public int BookId { get; set; }
    public Book? Book { get; set; }

    public DateTime ReservationDate { get; set; } = DateTime.UtcNow;
    public DateTime? ExpirationDate { get; set; }
    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;
    public int QueuePosition { get; set; }
}
