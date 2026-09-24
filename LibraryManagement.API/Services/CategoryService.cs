using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models.Entities;
using LibraryManagement.API.Repositories.Interfaces;
using LibraryManagement.API.Services.Interfaces;

namespace LibraryManagement.API.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _repository;

    public CategoryService(ICategoryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Category?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Category> CreateAsync(CategoryDto dto)
    {
        var category = new Category
        {
            Name = dto.Name,
            Description = dto.Description,
            ParentCategoryId = dto.ParentCategoryId
        };

        await _repository.AddAsync(category);
        await _repository.SaveChangesAsync();

        return category;
    }

    public async Task<bool> UpdateAsync(int id, CategoryDto dto)
    {
        var category = await _repository.GetByIdAsync(id);

        if (category == null)
            return false;

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.ParentCategoryId = dto.ParentCategoryId;

        _repository.Update(category);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var category = await _repository.GetByIdAsync(id);

        if (category == null)
            return false;

        var categoryWithBooks =
            await _repository.GetWithBooksAsync(id);

        if (categoryWithBooks!.BookCategories.Any())
            throw new InvalidOperationException(
                "Cannot delete a category that is linked to books.");

        _repository.Remove(category);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<Category?> GetWithBooksAsync(int id)
    {
        return await _repository.GetWithBooksAsync(id);
    }
}