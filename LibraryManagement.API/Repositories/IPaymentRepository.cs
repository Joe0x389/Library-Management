using LibraryManagement.API.Models.Entities;

namespace LibraryManagement.Repositories;

public interface IPaymentRepository
{
    Task<List<Payment>> GetAllAsync(int? fineId);
    Task<Payment?> GetByIdAsync(int id);
    Task AddAsync(Payment payment);
}
