using LibraryManagement.API.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.API.Data;

// IdentityDbContext<ApplicationUser> gives us AspNetUsers, AspNetRoles, AspNetUserRoles, etc.
// for free via ASP.NET Core Identity, on top of our own library tables.
public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Member> Members => Set<Member>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<BookAuthor> BookAuthors => Set<BookAuthor>();
    public DbSet<BookCategory> BookCategories => Set<BookCategory>();
    public DbSet<BookCopy> BookCopies => Set<BookCopy>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Fine> Fines => Set<Fine>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // required - sets up Identity's own tables first

        // --- Many-to-many: Book <-> Author ---
        builder.Entity<BookAuthor>().HasKey(ba => new { ba.BookId, ba.AuthorId });
        builder.Entity<BookAuthor>()
            .HasOne(ba => ba.Book)
            .WithMany(b => b.BookAuthors)
            .HasForeignKey(ba => ba.BookId);
        builder.Entity<BookAuthor>()
            .HasOne(ba => ba.Author)
            .WithMany(a => a.BookAuthors)
            .HasForeignKey(ba => ba.AuthorId);

        // --- Many-to-many: Book <-> Category ---
        builder.Entity<BookCategory>().HasKey(bc => new { bc.BookId, bc.CategoryId });
        builder.Entity<BookCategory>()
            .HasOne(bc => bc.Book)
            .WithMany(b => b.BookCategories)
            .HasForeignKey(bc => bc.BookId);
        builder.Entity<BookCategory>()
            .HasOne(bc => bc.Category)
            .WithMany(c => c.BookCategories)
            .HasForeignKey(bc => bc.CategoryId);

        // --- Book <-> ISBN uniqueness ---
        builder.Entity<Book>().HasIndex(b => b.ISBN).IsUnique();

        // --- Member <-> ApplicationUser (1:1) ---
        builder.Entity<Member>()
            .HasOne(m => m.User)
            .WithOne(u => u.Member)
            .HasForeignKey<Member>(m => m.UserId);

        // --- Loan relationships ---
        builder.Entity<Loan>()
            .HasOne(l => l.Member)
            .WithMany(m => m.Loans)
            .HasForeignKey(l => l.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Loan>()
            .HasOne(l => l.BookCopy)
            .WithMany(bc => bc.Loans)
            .HasForeignKey(l => l.BookCopyId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- Reservation relationships ---
        builder.Entity<Reservation>()
            .HasOne(r => r.Member)
            .WithMany(m => m.Reservations)
            .HasForeignKey(r => r.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Reservation>()
            .HasOne(r => r.Book)
            .WithMany(b => b.Reservations)
            .HasForeignKey(r => r.BookId)
            .OnDelete(DeleteBehavior.Restrict);

        // --- Fine relationships ---
        builder.Entity<Fine>()
            .HasOne(f => f.Member)
            .WithMany(m => m.Fines)
            .HasForeignKey(f => f.MemberId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Fine>()
            .HasOne(f => f.Loan)
            .WithMany(l => l.Fines)
            .HasForeignKey(f => f.LoanId)
            .OnDelete(DeleteBehavior.SetNull);

        // --- Payment relationships ---
        builder.Entity<Payment>()
            .HasOne(p => p.Fine)
            .WithMany(f => f.Payments)
            .HasForeignKey(p => p.FineId);

        // --- Decimal precision (avoid SQL Server truncation warnings) ---
        builder.Entity<Fine>().Property(f => f.Amount).HasColumnType("decimal(10,2)");
        builder.Entity<Payment>().Property(p => p.Amount).HasColumnType("decimal(10,2)");

        // --- Global soft-delete query filters ---
        builder.Entity<Book>().HasQueryFilter(b => !b.IsDeleted);
        builder.Entity<Member>().HasQueryFilter(m => !m.IsDeleted);
    }
}
