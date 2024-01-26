using VirtualLibrary.Models;

namespace VirtualLibrary.Interfaces;

public interface IRepository<T>: IDisposable {
    public Task<T> GetInstance();
}

public interface IVirtualLibraryInterface: IDisposable, IRepository<VirtualLibraryContext> {
    public Task<ListModels<LibraryUserModel>> ListUsers(int offset, int limit);
    public Task<bool> DeleteUser(long userId);
    public void CreateUser(CreateUserModel createUserModel);
    public Task<bool> SetPfp(long userId, string url);
    public void CreateSubscription(long userId, long authorId);
    public Task<bool> DeleteSubscription(long userId, long authorId);
}