using LibraryManagement.API.Models.Entities;

namespace LibraryManagement.API.Repositories.Interfaces;

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetWithBooksAsync(int id);
}