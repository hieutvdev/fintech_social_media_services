using BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BuildingBlocks.Repository.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private IDbContextTransaction _transaction;

    public UnitOfWork(DbContext context)
    {
        _context = context;
    }
    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }

    public IRepositoryService<T, TEntity, TContext> GetRepository<T, TEntity, TContext>() where TEntity : class where TContext : DbContext
    {
        throw new NotImplementedException();
    }

    public async Task<int> CommitAsync()
    {
        try
        {
            var result = await _context.SaveChangesAsync();
            await _transaction?.CommitAsync()!;
            return result;
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
    }

    public async Task RollbackAsync()
    {
        await _transaction?.RollbackAsync()!;
    }
}