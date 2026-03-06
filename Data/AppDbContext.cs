using LibApp.Models;
using Microsoft.EntityFrameworkCore;

namespace LibApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Author> Authors => Set<Author>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Category> Categories => Set<Category>();
    
    public DbSet<ExampleBook> ExampleBooks => Set<ExampleBook>();
    public DbSet<Fine> Fines => Set<Fine>();
    public DbSet<Loan> Loans => Set<Loan>();
    
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();

    public DbSet<VersionBook> VersionBooks => Set<VersionBook>();
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ─── Author ───────────────────────────────────────────
        modelBuilder.Entity<Author>(e =>
        {
            e.HasKey(a => a.AuthorId);
            e.Property(a => a.FullName).IsRequired().HasMaxLength(100);
            e.Property(a => a.Bio).HasColumnType("text");

            e.HasMany(a => a.Books)
             .WithOne(b => b.Author)
             .HasForeignKey(b => b.AuthorId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── Category ─────────────────────────────────────────
        modelBuilder.Entity<Category>(e =>
        {
            e.HasKey(c => c.CategoryId);
            e.Property(c => c.Name).IsRequired().HasMaxLength(50);
            e.Property(c => c.Description).HasMaxLength(255);
        });

        // ─── Book ─────────────────────────────────────────────
        modelBuilder.Entity<Book>(e =>
        {
            e.HasKey(b => b.BookId);
            e.Property(b => b.Name).IsRequired().HasMaxLength(255);

            e.HasOne(b => b.Author)
             .WithMany(a => a.Books)
             .HasForeignKey(b => b.AuthorId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(b => b.Category)
             .WithMany()
             .HasForeignKey(b => b.CategoryId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── Publisher ────────────────────────────────────────
        modelBuilder.Entity<Publisher>(e =>
        {
            e.HasKey(p => p.PublisherId);
            e.Property(p => p.Name).IsRequired().HasMaxLength(100);
            e.Property(p => p.Description).HasColumnType("text");
            e.Property(p => p.Address).HasMaxLength(100);
        });

        // ─── VersionBook ──────────────────────────────────────
        modelBuilder.Entity<VersionBook>(e =>
        {
            e.HasKey(v => v.VersionBookId);
            e.Property(v => v.Name).IsRequired().HasMaxLength(255);

            e.HasOne(v => v.Book)
             .WithMany()
             .HasForeignKey(v => v.BookId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(v => v.Publisher)
             .WithMany(p => p.VersionBooks)
             .HasForeignKey(v => v.PublisherId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── ExampleBook ──────────────────────────────────────
        modelBuilder.Entity<ExampleBook>(e =>
        {
            e.HasKey(eb => eb.ExampleBookId);

            e.HasOne(eb => eb.VersionBook)
             .WithMany(v => v.ExampleBooks)
             .HasForeignKey(eb => eb.VersionBookId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── Role ─────────────────────────────────────────────
        modelBuilder.Entity<Role>(e =>
        {
            e.HasKey(r => r.RoleId);
            e.Property(r => r.Name).IsRequired().HasMaxLength(50);
        });

        // ─── User ─────────────────────────────────────────────
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(u => u.UserId);
            e.Property(u => u.Login).IsRequired().HasMaxLength(50);
            e.HasIndex(u => u.Login).IsUnique();
            e.Property(u => u.Email).IsRequired().HasMaxLength(50);
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.HashPass).IsRequired().HasMaxLength(255);
            e.Property(u => u.FullName).IsRequired().HasMaxLength(100);
            e.Property(u => u.PhoneNumber).HasMaxLength(20);

            e.HasOne(u => u.Role)
             .WithMany()
             .HasForeignKey(u => u.RoleId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── Loan ─────────────────────────────────────────────
        modelBuilder.Entity<Loan>(e =>
        {
            e.HasKey(l => l.LoanId);

            e.HasOne(l => l.User)
             .WithMany(u => u.Loans)
             .HasForeignKey(l => l.UserId)
             .OnDelete(DeleteBehavior.Restrict);
            
            e.HasOne(l => l.ExampleBook)
             .WithMany()
             .HasForeignKey(l => l.ExampleBookId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ─── Fine ─────────────────────────────────────────────
        modelBuilder.Entity<Fine>(e =>
        {
            e.HasKey(f => f.FineId);
            e.Property(f => f.Amount).HasColumnType("numeric(10,2)");
            e.Property(f => f.Reason).HasColumnType("text");
            
            e.HasOne(f => f.Reader)
             .WithMany(u => u.ReaderFines)
             .HasForeignKey(f => f.ReaderId)
             .OnDelete(DeleteBehavior.Restrict);

            e.HasOne(f => f.Librarian)
             .WithMany(u => u.LibrarianFines)
             .HasForeignKey(f => f.LibrarianId)
             .OnDelete(DeleteBehavior.Restrict);
        });
    }

}
