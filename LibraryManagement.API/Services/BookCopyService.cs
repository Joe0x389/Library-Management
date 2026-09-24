using LibraryManagement.API.Data;
using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models.Entities;
using LibraryManagement.API.Models.Enums;
using LibraryManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Services;

public class BookCopyService : IBookCopyService
{
    private readonly AppDbContext _context;

    public BookCopyService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookCopyDto>> GetAllAsync(CopyStatus? status = null)
    {
        var query = _context.BookCopies.AsNoTracking();

        if (status.HasValue)
            query = query.Where(c => c.Status == status.Value);

        var copies = await query.OrderBy(c => c.Id).ToListAsync();
        return copies.Select(ToDto);
    }

    public async Task<BookCopyDto?> GetByIdAsync(int id)
    {
        var copy = await _context.BookCopies
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        return copy is null ? null : ToDto(copy);
    }

    public async Task<IEnumerable<BookCopyDto>> GetByBookIdAsync(int bookId)
    {
        var copies = await _context.BookCopies
            .AsNoTracking()
            .Where(c => c.BookId == bookId)
            .OrderBy(c => c.Id)
            .ToListAsync();

        return copies.Select(ToDto);
    }

    public async Task<BookCopyDto?> CreateAsync(CreateBookCopyDto dto)
    {
        var bookExists = await _context.Books.AnyAsync(b => b.Id == dto.BookId);
        if (!bookExists)
            return null;

        var duplicate = await _context.BookCopies
            .AnyAsync(c => c.CopyNumber == dto.CopyNumber);
        if (duplicate)
            throw new InvalidOperationException("A copy with this copy number already exists.");

        var copy = new BookCopy
        {
            BookId = dto.BookId,
            CopyNumber = dto.CopyNumber,
            ShelfLocation = dto.ShelfLocation,
            Condition = dto.Condition
        };

        _context.BookCopies.Add(copy);
        await _context.SaveChangesAsync();

        return ToDto(copy);
    }

    public async Task<bool> UpdateStatusAsync(int id, UpdateCopyStatusDto dto)
    {
        var copy = await _context.BookCopies.FindAsync(id);
        if (copy is null)
            return false;

        copy.Status = dto.Status;
        if (dto.Condition is not null)
            copy.Condition = dto.Condition;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var copy = await _context.BookCopies.FindAsync(id);
        if (copy is null)
            return false;

        var hasLoans = await _context.Loans.AnyAsync(l => l.BookCopyId == id);
        if (hasLoans)
            throw new InvalidOperationException("Cannot delete a copy that has loan history.");

        _context.BookCopies.Remove(copy);
        await _context.SaveChangesAsync();
        return true;
    }

    private static BookCopyDto ToDto(BookCopy c) => new()
    {
        Id = c.Id,
        BookId = c.BookId,
        CopyNumber = c.CopyNumber,
        Status = c.Status,
        ShelfLocation = c.ShelfLocation,
        AcquisitionDate = c.AcquisitionDate,
        Condition = c.Condition
    };
}