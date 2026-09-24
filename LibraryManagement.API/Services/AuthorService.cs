using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models.Entities;
using LibraryManagement.API.Repositories.Interfaces;
using LibraryManagement.API.Services.Interfaces;

namespace LibraryManagement.API.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _repository;

    public AuthorService(IAuthorRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Author?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Author> CreateAsync(AuthorDto dto)
    {
        var author = new Author
        {
            Name = dto.Name,
            Bio = dto.Bio,
            DateOfBirth = dto.DateOfBirth
        };

        await _repository.AddAsync(author);
        await _repository.SaveChangesAsync();

        return author;
    }

    public async Task<bool> UpdateAsync(int id, AuthorDto dto)
    {
        var author = await _repository.GetByIdAsync(id);

        if (author == null)
            return false;

        author.Name = dto.Name;
        author.Bio = dto.Bio;
        author.DateOfBirth = dto.DateOfBirth;

        _repository.Update(author);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var author = await _repository.GetByIdAsync(id);

        if (author == null)
            return false;

        var authorWithBooks = await _repository.GetWithBooksAsync(id);

        if (authorWithBooks!.BookAuthors.Any())
            throw new InvalidOperationException(
                "Cannot delete an author who is linked to books.");

        _repository.Remove(author);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<Author?> GetWithBooksAsync(int id)
    {
        return await _repository.GetWithBooksAsync(id);
    }
}