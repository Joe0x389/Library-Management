using LibraryManagement.API.Data;
using LibraryManagement.API.Models.Entities;
using LibraryManagement.API.Repositories.Interfaces;

namespace LibraryManagement.API.Repositories;

public class PublisherRepository
    : Repository<Publisher>, IPublisherRepository
{
    public PublisherRepository(AppDbContext context)
        : base(context)
    {
    }
}