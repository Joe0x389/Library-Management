using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models.Entities;

namespace LibraryManagement.API.Services.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category> CreateAsync(CategoryDto dto);
    Task<bool> UpdateAsync(int id, CategoryDto dto);
    Task<bool> DeleteAsync(int id);
    Task<Category?> GetWithBooksAsync(int id);
}