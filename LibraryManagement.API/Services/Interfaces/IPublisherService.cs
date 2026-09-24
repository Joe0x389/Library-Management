using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models.Entities;

namespace LibraryManagement.API.Services.Interfaces;

public interface IPublisherService
{
    Task<IEnumerable<Publisher>> GetAllAsync();
    Task<Publisher?> GetByIdAsync(int id);
    Task<Publisher> CreateAsync(PublisherDto dto);
    Task<bool> UpdateAsync(int id, PublisherDto dto);
    Task<bool> DeleteAsync(int id);
}