using LibraryManagement.API.Data;
using LibraryManagement.API.Models.Entities;
using LibraryManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Repositories;

public class AuthorRepository : Repository<Author>, IAuthorRepository
{
    public AuthorRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<Author?> GetWithBooksAsync(int id)
    {
        return await _context.Authors
            .Include(a => a.BookAuthors)
            .ThenInclude(ba => ba.Book)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}