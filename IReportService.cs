using LibraryManagement.API.DTOs;

namespace LibraryManagement.API.Services.Interfaces;

public interface IReportService
{
    Task<DashboardSummaryDto> GetDashboardSummaryAsync();
    Task<IEnumerable<MostBorrowedBookDto>> GetMostBorrowedBooksAsync(int top = 10);
    Task<IEnumerable<MostActiveMemberDto>> GetMostActiveMembersAsync(int top = 10);
    Task<IEnumerable<OverdueLoanDto>> GetOverdueLoansAsync();
    Task<InventoryStatusDto> GetInventoryStatusAsync();
    Task<FinesSummaryDto> GetFinesSummaryAsync();
    Task<ReservationsSummaryDto> GetReservationsSummaryAsync();
}
