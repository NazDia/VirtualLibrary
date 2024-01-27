using VirtualLibrary.Models;

namespace VirtualLibrary.Interfaces;

public interface IRepository<T> {
    public Task<T> GetInstance();
}

public interface IVirtualLibraryInterface: IRepository<VirtualLibraryContext> {
    public Task<ListModels<ShowUserModel>> ListUsers(int offset, int limit);
    public Task<bool> DeleteUser(long userId);
    public void CreateUser(CreateUserModel createUserModel);
    public Task<bool> SetPfp(long userId, string url);
    public Task<bool> CreateSubscription(long userId, long authorId);
    public Task<bool> DeleteSubscription(long userId, long authorId);
    public void CreateAuthor(CreateAuthorModel createAuthorModel);
    public Task<ShowAuthorModel?> DetailsAuthor(long authorId);
    public Task<ShowBookListedModel> CreateBook(long authorId, CreateBookModel createBookModel);
    public Task<ListModels<ShowBookListedModel>> ListBooks(
        long? authorId,
        string? editorialName,
        DateTime? before,
        DateTime? after,
        int offset,
        int limit
    );
}