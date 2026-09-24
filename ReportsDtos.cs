namespace LibraryManagement.API.DTOs;

public class DashboardSummaryDto
{
    public int TotalBooks { get; set; }
    public int TotalMembers { get; set; }
    public int TotalBookCopies { get; set; }
    public int AvailableCopies { get; set; }
    public int ActiveLoans { get; set; }
    public int OverdueLoans { get; set; }
    public int PendingReservations { get; set; }
    public int UnpaidFinesCount { get; set; }
}

public class MostBorrowedBookDto
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Authors { get; set; } = string.Empty;
    public int BorrowCount { get; set; }
}

public class MostActiveMemberDto
{
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public int LoanCount { get; set; }
}

public class OverdueLoanDto
{
    public int LoanId { get; set; }
    public int MemberId { get; set; }
    public string MemberName { get; set; } = string.Empty;
    public int BookCopyId { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int DaysOverdue { get; set; }
}

public class InventoryStatusDto
{
    public int Available { get; set; }
    public int Borrowed { get; set; }
    public int Reserved { get; set; }
    public int Lost { get; set; }
    public int Damaged { get; set; }
    public int Total { get; set; }
}

public class FinesSummaryDto
{
    public decimal TotalIssued { get; set; }
    public decimal TotalPaid { get; set; }
    public decimal TotalUnpaid { get; set; }
    public decimal TotalWaived { get; set; }
    public int UnpaidCount { get; set; }
}

public class ReservationsSummaryDto
{
    public int Pending { get; set; }
    public int Ready { get; set; }
    public int Fulfilled { get; set; }
    public int Cancelled { get; set; }
    public int Expired { get; set; }
    public int Total { get; set; }
}
