using LibraryManagement.API.Data;
using LibraryManagement.API.Models.Entities;
using LibraryManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Repositories;

public class CategoryRepository
    : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(AppDbContext context)
        : base(context)
    {
    }

    public async Task<Category?> GetWithBooksAsync(int id)
    {
        return await _context.Categories
            .Include(c => c.BookCategories)
            .ThenInclude(bc => bc.Book)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}