namespace LibraryManagement.API.Models.Entities;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime? PublishedDate { get; set; }
    public int? PublisherId { get; set; }
    public Publisher? Publisher { get; set; }
    public string? Language { get; set; }
    public int? PageCount { get; set; }
    public string? CoverImageUrl { get; set; }
    public bool IsDeleted { get; set; } = false;

    public ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();
    public ICollection<BookCategory> BookCategories { get; set; } = new List<BookCategory>();
    public ICollection<BookCopy> Copies { get; set; } = new List<BookCopy>();
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
