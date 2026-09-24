using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models.Enums;

namespace LibraryManagement.API.Services.Interfaces;

public interface IBookCopyService
{
    Task<IEnumerable<BookCopyDto>> GetAllAsync(CopyStatus? status = null);
    Task<BookCopyDto?> GetByIdAsync(int id);
    Task<IEnumerable<BookCopyDto>> GetByBookIdAsync(int bookId);
    Task<BookCopyDto?> CreateAsync(CreateBookCopyDto dto);
    Task<bool> UpdateStatusAsync(int id, UpdateCopyStatusDto dto);
    Task<bool> DeleteAsync(int id);
}