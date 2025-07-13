namespace Catalog.Domain.Repositories;
public interface IUnitOfWork
{
    Task CommitAsync();
}
