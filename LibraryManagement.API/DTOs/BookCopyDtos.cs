using System.ComponentModel.DataAnnotations;
using LibraryManagement.API.Models.Enums;

namespace LibraryManagement.API.DTOs;

public class BookCopyDto
{
    public int Id { get; set; }
    public int BookId { get; set; }
    public string CopyNumber { get; set; } = string.Empty;
    public CopyStatus Status { get; set; }
    public string? ShelfLocation { get; set; }
    public DateTime AcquisitionDate { get; set; }
    public string? Condition { get; set; }
}

public class CreateBookCopyDto
{
    public int BookId { get; set; }

    [Required]
    public string CopyNumber { get; set; } = string.Empty;

    public string? ShelfLocation { get; set; }
    public string? Condition { get; set; }
}

public class UpdateCopyStatusDto
{
    public CopyStatus Status { get; set; }
    public string? Condition { get; set; }
}