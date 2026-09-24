using LibraryManagement.API.Data;
using LibraryManagement.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Repositories;

public class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _db;
    public PaymentRepository(AppDbContext db) => _db = db;

    public async Task<List<Payment>> GetAllAsync(int? fineId)
    {
        var q = _db.Payments.AsNoTracking().AsQueryable();
        if (fineId.HasValue) q = q.Where(p => p.FineId == fineId);
        return await q.OrderByDescending(p => p.PaidAt).ToListAsync();
    }

    public Task<Payment?> GetByIdAsync(int id) =>
        _db.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

    public async Task AddAsync(Payment payment) => await _db.Payments.AddAsync(payment);
}
