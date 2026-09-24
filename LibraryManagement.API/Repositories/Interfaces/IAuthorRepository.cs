using LibraryManagement.API.Models.Entities;

namespace LibraryManagement.API.Repositories.Interfaces;

public interface IAuthorRepository : IRepository<Author>
{
    Task<Author?> GetWithBooksAsync(int id);
}