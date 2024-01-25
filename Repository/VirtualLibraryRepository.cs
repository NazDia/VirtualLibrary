using Microsoft.EntityFrameworkCore.Internal;
using VirtualLibrary.Interfaces;
using VirtualLibrary.Models;

namespace VirtualLibrary.Repositories;

public class VirtualLibraryRepository : IRepository<VirtualLibraryContext>
{
    private readonly DbContextFactory<VirtualLibraryContext> _dbContextFactory;

    public VirtualLibraryRepository(DbContextFactory<VirtualLibraryContext> dbContextFactory) {
        _dbContextFactory = dbContextFactory;
    }
    public async Task<VirtualLibraryContext> GetInstance() {
        return await _dbContextFactory.CreateDbContextAsync();
    }
}