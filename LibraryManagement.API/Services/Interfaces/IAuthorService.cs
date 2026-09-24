using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models.Entities;

namespace LibraryManagement.API.Services.Interfaces;

public interface IAuthorService
{
    Task<IEnumerable<Author>> GetAllAsync();
    Task<Author?> GetByIdAsync(int id);
    Task<Author> CreateAsync(AuthorDto dto);
    Task<bool> UpdateAsync(int id, AuthorDto dto);
    Task<bool> DeleteAsync(int id);
    Task<Author?> GetWithBooksAsync(int id);
}