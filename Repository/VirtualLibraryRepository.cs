using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using VirtualLibrary.Interfaces;
using VirtualLibrary.Models;

namespace VirtualLibrary.Repositories;

public class VirtualLibraryRepository : IVirtualLibraryInterface
{
    private readonly DbContextFactory<VirtualLibraryContext> _dbContextFactory;

    public VirtualLibraryRepository(DbContextFactory<VirtualLibraryContext> dbContextFactory) {
        _dbContextFactory = dbContextFactory;
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public async Task<VirtualLibraryContext> GetInstance() {
        var context = await _dbContextFactory.CreateDbContextAsync();
        context.Database.EnsureCreated();
        context.ChangeTracker.DetectChanges();
        return context;
    }

    
    public async Task<ListModels<LibraryUserModel>> ListUsers(int offset, int limit) {
        var context = await GetInstance();
        var users = await context.LibraryUserModels
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
        ListModels<LibraryUserModel> listModels = new ListModels<LibraryUserModel> { 
            Limit = limit,
            Offset = offset,
            Users = users
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

    public async void CreateUser(CreateUserModel createUserModel) {
        var context = GetInstance();
        LibraryUserModel libraryUserModel = new LibraryUserModel {
            Id = 0,
            Name = createUserModel.Name,
            Email = createUserModel.Email,
            Pfp_url = createUserModel.Pfp_url
        };
        var wcontext = await context;
        wcontext.Add(libraryUserModel);
        await wcontext.SaveChangesAsync();
    }

    public async Task<bool> SetPfp(long userId, string Pfp_url) {
        var context = await GetInstance();
        var user = await context.LibraryUserModels.FindAsync(userId);
        if (user == null) return false;
        user.Pfp_url = Pfp_url;
        context.Entry(user).State = EntityState.Modified;
        return true;
    }

    public async void CreateSubscription(long userId, long authorId) {
        var context = GetInstance();
        SubsriptionModel subsriptionModel = new SubsriptionModel {
            UserId = userId,
            AuthorId = authorId
        };
        var wcontext = await context;
        wcontext.Add(subsriptionModel);
        await wcontext.SaveChangesAsync();
    }

    public async Task<bool> DeleteSubscription(long userId, long authorId) {
        var context = await GetInstance();
        var subscription = await context.SubsriptionModels.FindAsync(userId, authorId);
        if (subscription == null) return false;
        context.Remove(context);
        await context.SaveChangesAsync();
        return true;
    }
}