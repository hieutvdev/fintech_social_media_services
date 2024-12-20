using BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Repository.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IRepositoryService<T, TEntity, TContext> GetRepository<T, TEntity, TContext>() where TEntity : class
        where TContext : DbContext;

    Task<int> CommitAsync();
    Task RollbackAsync();
}