using LibraryManagement.API.Common;
using LibraryManagement.Dtos;

namespace LibraryManagement.Services;

public interface IPaymentService
{
    Task<ServiceResult<List<PaymentDto>>> GetAllAsync(int? fineId);
    Task<ServiceResult<PaymentDto>> GetByIdAsync(int id);
    Task<ServiceResult<PaymentDto>> CreateAsync(CreatePaymentDto dto);
}
