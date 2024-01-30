using Microsoft.EntityFrameworkCore;

namespace VirtualLibrary.Models;

public class VirtualLibraryContext: DbContext {
    public VirtualLibraryContext(DbContextOptions<VirtualLibraryContext> options)
        : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AuthorModel>()
            .HasMany(a => a.Books)
            .WithOne(b => b.AuthorModel);
        modelBuilder.Entity<LibraryUserModel>()
            .HasMany(u => u.Reviews)
            .WithOne(r => r.LibraryUserModel);
        modelBuilder.Entity<BookModel>()
            .HasMany(b => b.Reviews)
            .WithOne(r => r.BookModel);
        modelBuilder.Entity<AuthorModel>()
            .HasMany(a => a.Subsriptions)
            .WithOne(s => s.Author)
            .HasForeignKey(s => s.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<LibraryUserModel>()
            .HasMany(u => u.Subsriptions)
            .WithOne(s => s.User)
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<AuthorModel> AuthorModels { get; set; }
    public DbSet<BookModel> BookModels { get; set; }
    public DbSet<ReviewModel> ReviewModels { get; set; }
    public DbSet<LibraryUserModel> LibraryUserModels { get; set;}
    public DbSet<SubsriptionModel> SubsriptionModels { get; set; }
}