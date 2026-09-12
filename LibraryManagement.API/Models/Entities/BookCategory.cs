namespace LibraryManagement.API.Models.Entities;

// Many-to-many join: Book <-> Category
public class BookCategory
{
    public int BookId { get; set; }
    public Book? Book { get; set; }

    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}
