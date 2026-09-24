using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models.Entities;
using LibraryManagement.API.Repositories.Interfaces;
using LibraryManagement.API.Services.Interfaces;

namespace LibraryManagement.API.Services;

public class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _repository;

    public PublisherService(IPublisherRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Publisher>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<Publisher?> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<Publisher> CreateAsync(PublisherDto dto)
    {
        var publisher = new Publisher
        {
            Name = dto.Name,
            Address = dto.Address,
            ContactInfo = dto.ContactInfo
        };

        await _repository.AddAsync(publisher);
        await _repository.SaveChangesAsync();

        return publisher;
    }

    public async Task<bool> UpdateAsync(
        int id,
        PublisherDto dto)
    {
        var publisher = await _repository.GetByIdAsync(id);

        if (publisher == null)
            return false;

        publisher.Name = dto.Name;
        publisher.Address = dto.Address;
        publisher.ContactInfo = dto.ContactInfo;

        _repository.Update(publisher);
        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var publisher = await _repository.GetByIdAsync(id);

        if (publisher == null)
            return false;

        _repository.Remove(publisher);
        await _repository.SaveChangesAsync();

        return true;
    }
}