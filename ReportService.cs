using LibraryManagement.API.Data;
using LibraryManagement.API.DTOs;
using LibraryManagement.API.Models.Enums;
using LibraryManagement.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Services;

public class ReportService : IReportService
{
    private readonly AppDbContext _context;

    public ReportService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardSummaryDto> GetDashboardSummaryAsync()
    {
        var now = DateTime.UtcNow;

        return new DashboardSummaryDto
        {
            TotalBooks = await _context.Books.CountAsync(),
            TotalMembers = await _context.Members.CountAsync(),
            TotalBookCopies = await _context.BookCopies.CountAsync(),
            AvailableCopies = await _context.BookCopies.CountAsync(c => c.Status == CopyStatus.Available),
            ActiveLoans = await _context.Loans.CountAsync(l => l.ReturnDate == null),
            OverdueLoans = await _context.Loans.CountAsync(l => l.ReturnDate == null && l.DueDate < now),
            PendingReservations = await _context.Reservations.CountAsync(r => r.Status == ReservationStatus.Pending),
            UnpaidFinesCount = await _context.Fines.CountAsync(f => f.Status == FineStatus.Unpaid)
        };
    }

    public async Task<IEnumerable<MostBorrowedBookDto>> GetMostBorrowedBooksAsync(int top = 10)
    {
        var grouped = await _context.Loans
            .AsNoTracking()
            .GroupBy(l => l.BookCopy.BookId)
            .Select(g => new { BookId = g.Key, BorrowCount = g.Count() })
            .OrderByDescending(x => x.BorrowCount)
            .Take(top)
            .ToListAsync();

        var bookIds = grouped.Select(g => g.BookId).ToList();

        var books = await _context.Books
            .AsNoTracking()
            .Where(b => bookIds.Contains(b.Id))
            .Select(b => new
            {
                b.Id,
                b.Title,
                Authors = b.BookAuthors.Select(ba => ba.Author.Name)
            })
            .ToListAsync();

        return grouped
            .Join(books, g => g.BookId, b => b.Id, (g, b) => new MostBorrowedBookDto
            {
                BookId = b.Id,
                Title = b.Title,
                Authors = string.Join(", ", b.Authors),
                BorrowCount = g.BorrowCount
            })
            .OrderByDescending(x => x.BorrowCount)
            .ToList();
    }

    public async Task<IEnumerable<MostActiveMemberDto>> GetMostActiveMembersAsync(int top = 10)
    {
        var grouped = await _context.Loans
            .AsNoTracking()
            .GroupBy(l => l.MemberId)
            .Select(g => new { MemberId = g.Key, LoanCount = g.Count() })
            .OrderByDescending(x => x.LoanCount)
            .Take(top)
            .ToListAsync();

        var memberIds = grouped.Select(g => g.MemberId).ToList();

        var members = await _context.Members
            .AsNoTracking()
            .Where(m => memberIds.Contains(m.Id))
            .Select(m => new { m.Id, m.User.FullName })
            .ToListAsync();

        return grouped
            .Join(members, g => g.MemberId, m => m.Id, (g, m) => new MostActiveMemberDto
            {
                MemberId = m.Id,
                MemberName = m.FullName,
                LoanCount = g.LoanCount
            })
            .OrderByDescending(x => x.LoanCount)
            .ToList();
    }

    public async Task<IEnumerable<OverdueLoanDto>> GetOverdueLoansAsync()
    {
        var now = DateTime.UtcNow;

        return await _context.Loans
            .AsNoTracking()
            .Where(l => l.ReturnDate == null && l.DueDate < now)
            .Select(l => new OverdueLoanDto
            {
                LoanId = l.Id,
                MemberId = l.MemberId,
                MemberName = l.Member.User.FullName,
                BookCopyId = l.BookCopyId,
                BookTitle = l.BookCopy.Book.Title,
                DueDate = l.DueDate,
                DaysOverdue = EF.Functions.DateDiffDay(l.DueDate, now)
            })
            .OrderByDescending(l => l.DaysOverdue)
            .ToListAsync();
    }

    public async Task<InventoryStatusDto> GetInventoryStatusAsync()
    {
        var counts = await _context.BookCopies
            .AsNoTracking()
            .GroupBy(c => c.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        var dto = new InventoryStatusDto();
        foreach (var c in counts)
        {
            switch (c.Status)
            {
                case CopyStatus.Available: dto.Available = c.Count; break;
                case CopyStatus.Borrowed: dto.Borrowed = c.Count; break;
                case CopyStatus.Reserved: dto.Reserved = c.Count; break;
                case CopyStatus.Lost: dto.Lost = c.Count; break;
                case CopyStatus.Damaged: dto.Damaged = c.Count; break;
            }
        }
        dto.Total = counts.Sum(c => c.Count);
        return dto;
    }

    public async Task<FinesSummaryDto> GetFinesSummaryAsync()
    {
        return new FinesSummaryDto
        {
            TotalIssued = await _context.Fines.SumAsync(f => (decimal?)f.Amount) ?? 0,
            TotalPaid = await _context.Fines.Where(f => f.Status == FineStatus.Paid).SumAsync(f => (decimal?)f.Amount) ?? 0,
            TotalUnpaid = await _context.Fines.Where(f => f.Status == FineStatus.Unpaid).SumAsync(f => (decimal?)f.Amount) ?? 0,
            TotalWaived = await _context.Fines.Where(f => f.Status == FineStatus.Waived).SumAsync(f => (decimal?)f.Amount) ?? 0,
            UnpaidCount = await _context.Fines.CountAsync(f => f.Status == FineStatus.Unpaid)
        };
    }

    public async Task<ReservationsSummaryDto> GetReservationsSummaryAsync()
    {
        var counts = await _context.Reservations
            .AsNoTracking()
            .GroupBy(r => r.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        var dto = new ReservationsSummaryDto();
        foreach (var c in counts)
        {
            switch (c.Status)
            {
                case ReservationStatus.Pending: dto.Pending = c.Count; break;
                case ReservationStatus.Ready: dto.Ready = c.Count; break;
                case ReservationStatus.Fulfilled: dto.Fulfilled = c.Count; break;
                case ReservationStatus.Cancelled: dto.Cancelled = c.Count; break;
                case ReservationStatus.Expired: dto.Expired = c.Count; break;
            }
        }
        dto.Total = counts.Sum(c => c.Count);
        return dto;
    }
}
