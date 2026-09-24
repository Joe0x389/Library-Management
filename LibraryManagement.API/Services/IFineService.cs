using LibraryManagement.Common;
using LibraryManagement.Dtos;

namespace LibraryManagement.Services;

public interface IFineService
{
    Task<ServiceResult<List<FineDto>>> GetAllAsync(int? memberId, string? status);
    Task<ServiceResult<FineDetailsDto>> GetByIdAsync(int id);
    Task<ServiceResult<MemberFinesSummaryDto>> GetMemberSummaryAsync(int memberId);
    Task<ServiceResult<FineDto>> CreateAsync(CreateFineDto dto);
    Task<ServiceResult<bool>> UpdateAsync(int id, UpdateFineDto dto);
    Task<ServiceResult<bool>> DeleteAsync(int id);
}
