using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using VirtualLibrary.Interfaces;
using VirtualLibrary.Mappers;
using VirtualLibrary.Models;

namespace VirtualLibrary.Repositories;

public class VirtualLibraryRepository : IVirtualLibraryInterface
{
    private readonly DbContextFactory<VirtualLibraryContext> _dbContextFactory;

    public VirtualLibraryRepository(DbContextFactory<VirtualLibraryContext> dbContextFactory) {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<VirtualLibraryContext> GetInstance() {
        var context = await _dbContextFactory.CreateDbContextAsync();
        context.Database.EnsureCreated();
        context.ChangeTracker.DetectChanges();
        return context;
    }

    
    public async Task<ListModels<ShowUserModel>> ListUsers(int offset, int limit) {
        // IMapper<LibraryUserModel, ShowUserModel> showUserMapper = new ShowUserMapper();
        var context = await GetInstance();
        var users = await context.LibraryUserModels
            .Include(u => u.Subsriptions)
            .Skip(offset)
            .Take(limit)
            .Select(u => new ShowUserModel {
                Name = u.Name,
                Pfp_url = u.Pfp_url,
                Id = u.Id,
                CreationTime = u.CreationTime,
                Email = u.Email,
                SubscriptionCount = u.Subsriptions.Count
            })
            .ToListAsync();
        ListModels<ShowUserModel> listModels = new ListModels<ShowUserModel> { 
            Limit = limit,
            Offset = offset,
            Elements = users
        };
        return listModels;
    }

    public async Task<bool> DeleteUser(long userId) {
        var context = await GetInstance();
        var user = await context.LibraryUserModels.FindAsync(userId);
        if (user == null) return false;
        context.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<ShowUserModel> CreateUser(CreateUserModel createUserModel) {
        var context = GetInstance();
        LibraryUserModel libraryUserModel = new UserCreationMapper().map(createUserModel);
        var wcontext = await context;
        wcontext.Add(libraryUserModel);
        await wcontext.SaveChangesAsync();
        return (new ShowUserMapper()).map(libraryUserModel);
    }

    public async Task<ShowUserModel?> SetPfp(long userId, string Pfp_url) {
        var context = await GetInstance();
        var user = await context.LibraryUserModels.FindAsync(userId);
        if (user == null) return null;
        user.Pfp_url = Pfp_url;
        context.LibraryUserModels.Attach(user);
        context.Entry(user).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return (new ShowUserMapper()).map(user);
    }

    public async Task<bool> CreateSubscription(long userId, long authorId) {
        var context = await GetInstance();
        var context1 = await GetInstance();
        var user = context.LibraryUserModels.FindAsync(userId);
        var author = context1.AuthorModels.FindAsync(authorId);
        var wuser = await user;
        if (wuser == null) return false;
        var wauthor = await author;
        if (wauthor == null) return false;
        SubsriptionModel subsriptionModel = new SubsriptionModel {
            UserId = userId,
            AuthorId = authorId,
        };
        context.Add(subsriptionModel);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteSubscription(long userId, long authorId) {
        var context = await GetInstance();
        var subscription = await context.SubsriptionModels.FindAsync(userId, authorId);
        if (subscription == null) return false;
        context.Remove(subscription);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<ShowAuthorModel?> CreateAuthor(CreateAuthorModel createAuthorModel) {
        var context = GetInstance();
        AuthorModel authorModel = new CreateAuthorMapper().map(createAuthorModel);
        var wcontext = await context;
        wcontext.AuthorModels.Add(authorModel);
        await wcontext.SaveChangesAsync();
        return (new ShowAuthorMapper()).map(authorModel);
    }

    public async Task<ShowAuthorModel?> DetailsAuthor(long authorId) {
        ShowAuthorMapper showAuthorMapper = new ShowAuthorMapper();
        var context = await GetInstance();
        var author = await context.AuthorModels
            .Include(a => a.Books)
            .Include(a => a.Subsriptions)
            .Select(a => new ShowAuthorModel {
                Id = a.Id,
                Name = a.Name,
                Nationality = a.Nationality,
                SubsriptionCount = a.Subsriptions.Count,
                BirthDate = a.BirthDate,
                Books = a.Books.Select(b => new ShowBookAuthoredModel {
                    Isbn = b.Isbn,
                    Name = b.Name,
                    PublicationDate = b.PublicationDate
                })
                // Books = (new ShowBookAuthoredMapper() as IMapper<BookModel, ShowBookAuthoredModel>).lMap(a.Books)
            })
            .FirstOrDefaultAsync(a => a.Id == authorId);
        if (author == null) return null;
        return author;
    }

    public async Task<ShowBookListedModel?> CreateBook(long authorId, CreateBookModel createBookModel) {
        var context = await GetInstance();
        var author = await context.AuthorModels.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == authorId);
        if (author == null) return null;
        CreateBookMapper createBookMapper = new CreateBookMapper();
        var bookModel = createBookMapper.map(createBookModel);
        bookModel.AuthorModel = author;
        bookModel.AuthorModelId = authorId;
        context.BookModels.Add(bookModel);
        await context.SaveChangesAsync();
        return new ShowBookListedMapper().map(bookModel);
    }

    public async Task<ListModels<ShowBookListedModel>> ListBooks(
        long? authorId,
        string? editorialName,
        DateTime? before,
        DateTime? after,
        int offset,
        int limit,
        bool? sort
    ) {
        var context = await GetInstance();
        var semiBooks = context.BookModels
            .Include(b => b.AuthorModel)
            .Include(b => b.Reviews)
            .Where(b => 
                (authorId == null || authorId == b.AuthorModelId) &&
                (editorialName == null || editorialName == b.Editorial) &&
                (before == null || b.PublicationDate < before) &&
                (after == null || b.PublicationDate > after)
            )
            .Skip(offset)
            .Take(limit)
            .Select(b => new ShowBookListedModel {
                Qualification = (int)b.Reviews.Average(r => r.Qualification),
                Name = b.Name,
                AuthorName = b.AuthorModel.Name,
                Editorial = b.Editorial,
                Isbn = b.Isbn
            });
        if (sort != null && (bool)sort) {
            semiBooks = semiBooks.OrderBy(b => b.Qualification);
        }
        else if (sort != null && !(bool)sort) {
            semiBooks = semiBooks.OrderByDescending(b => b.Qualification);
        }
        var books = await semiBooks.ToListAsync();
        ListModels<ShowBookListedModel> listModels = new ListModels<ShowBookListedModel> {
            Offset = offset,
            Limit = limit,
            Elements = books
        };
        return listModels;
    }
    
    public async Task<ShowReviewModel?> CreateReview(long bookId, long userId, CreateReviewModel createReviewModel) {
        var context = await GetInstance();
        var context1 = await GetInstance();
        var book = context.BookModels.FindAsync(bookId);
        var user = context1.LibraryUserModels.FindAsync(userId);
        var wuser = await user;
        if (await book == null) return null;
        if (wuser == null) return null;
        var review = (new CreateReviewMapper()).map(createReviewModel);
        review.BookModelId = bookId;
        review.LibraryUserModelId = userId;
        context.ReviewModels.Add(review);
        await context.SaveChangesAsync();
        review.LibraryUserModel = wuser;
        var ret = (new ShowReviewMapper()).map(review);
        return ret;
    }
    
    public async Task<ListModels<ShowReviewModel>?> ListReviews(
        long bookId,
        int? qual,
        bool? sort,
        int offset,
        int limit
    ) {
        var context = await GetInstance();
        var semiReviews = context.ReviewModels
            .Include(r => r.LibraryUserModel)
            .Where(r => 
                r.BookModelId == bookId &&
                (qual == null || qual == r.Qualification)
            )
            .Skip(offset)
            .Take(limit);
        if (sort != null && (bool)sort) {
            semiReviews = semiReviews.OrderBy(r => r.CreationTime);
        }
        else if (sort != null && !(bool)sort) {
            semiReviews = semiReviews.OrderByDescending(r => r.CreationTime);
        }
        var reviews = await semiReviews
            .Select(r => new ShowReviewModel {
                User = (new ShowUserNaiveMapper()).map(r.LibraryUserModel),
                Review = r.Review,
                Qualification = r.Qualification,
                CreationTime = r.CreationTime,
            })
            .ToListAsync();
        if (reviews == null) return null;
        return new ListModels<ShowReviewModel> {
            Offset = offset,
            Limit = limit,
            Elements = reviews
        };
    }
}