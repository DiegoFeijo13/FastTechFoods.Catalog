using Catalog.Domain.Repositories;
using Catalog.Infrastructure.Data;

namespace Catalog.Infrastructure.Repositories;
internal class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CommitAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}
