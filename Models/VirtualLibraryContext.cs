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
        base.OnModelCreating(modelBuilder);
    }
    public DbSet<AuthorModel> AuthorModels { get; set; }
    public DbSet<BookModel> BookModels { get; set; }
    public DbSet<ReviewModel> ReviewModels { get; set; }
    public DbSet<LibraryUserModel> LibraryUserModels { get; set;}
}