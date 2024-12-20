
using System.Linq.Expressions;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;

public class RepositoryService<T, TEntity, TContext> : IRepositoryService<T, TEntity, TContext>
where TEntity : class
where TContext : DbContext
{
    private readonly TContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public RepositoryService(TContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(T? id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbSet.FindAsync(new object[] { id }, cancellationToken);
        return entity ?? null ;
    }

    public async Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        if (entities == null)
        {
            throw new BadRequestException($"Add {nameof(TEntity)} error!");
        }
        await _context.AddRangeAsync(entities, cancellationToken);
    }

    public  async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        if (entity == null)
        {
            throw new BadRequestException($"Add {nameof(TEntity)} error!");
        }
        await _context.AddAsync(entity, cancellationToken);
    }

    public EntityEntry Update(TEntity entity)
    {
        return (object)entity != null ? this._context.Set<TEntity>().Update(entity) : throw new BadRequestException($"Update {nameof(TEntity)} error!");
    }

    public EntityEntry Delete(TEntity entity)
    {
        return (object)entity != null
            ? this._context.Set<TEntity>().Remove(entity)
            : throw new BadRequestException($"Remove {nameof(TEntity)} error!");
    }

    public void DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        this._context.Set<TEntity>().RemoveRange(entities);
    }

    public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    public IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
    {
        return this._context.Set<TEntity>().AsNoTracking().Where(expression);
    }

    public async Task<IQueryable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = this._context.Set<TEntity>().AsQueryable();
        IQueryable<TEntity> queryable = await Task.FromResult<IQueryable<TEntity>>(query.Where(expression));
        return queryable;
    }

    public async Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity?, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await this._context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(expression, cancellationToken);
    }

    public async Task<PaginatedResult<TEntity>> GetPageAsync(PaginationRequest paginationRequest,Expression<Func<TEntity, bool>>? expression, CancellationToken cancellationToken)
    {
        long totalRecords = await this._context.Set<TEntity>().CountAsync(cancellationToken);
        IEnumerable<TEntity> dataResponses = null!;
        if (expression is not null)
        {
            dataResponses = await this._context.Set<TEntity>().Where(expression)
                .Skip(paginationRequest.PageIndex * paginationRequest.PageSize).Take(paginationRequest.PageSize)
                .ToListAsync(cancellationToken);
            return new PaginatedResult<TEntity>(paginationRequest.PageIndex, paginationRequest.PageSize, totalRecords,
                dataResponses);
        }
        dataResponses = await this._context.Set<TEntity>()
            .Skip(paginationRequest.PageIndex * paginationRequest.PageSize).Take(paginationRequest.PageSize)
            .ToListAsync(cancellationToken); 
        return new PaginatedResult<TEntity>(paginationRequest.PageIndex, paginationRequest.PageSize, totalRecords,
            dataResponses);
    }

    public async Task<IEnumerable<TResult>> GetSelectorAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default)
    {
        var dataResponse = await this._context.Set<TEntity>().AsNoTracking<TEntity>().Select(selector).ToListAsync(cancellationToken);
        return dataResponse;
    }

    public async Task<PaginatedResult<TResult>> GetSelectAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>>? expression, PaginationRequest paginationRequest,
        CancellationToken cancellationToken = default)
    {
        long totalRecords = await this._context.Set<TEntity>().CountAsync(cancellationToken);
        IEnumerable<TResult> dataResponses = null!;
        if (expression is not null)
        {
            dataResponses = await this._context.Set<TEntity>()
                .Where(expression)
                .Select(selector)
                .Skip(paginationRequest.PageIndex * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync(cancellationToken);
            return new PaginatedResult<TResult>(paginationRequest.PageIndex, paginationRequest.PageSize, totalRecords,
                dataResponses);
        }
        dataResponses = await this._context.Set<TEntity>()
            .Select(selector)
            .Skip(paginationRequest.PageIndex * paginationRequest.PageSize)
            .Take(paginationRequest.PageSize)
            .ToListAsync(cancellationToken);
        return new PaginatedResult<TResult>(paginationRequest.PageIndex, paginationRequest.PageSize, totalRecords,
            dataResponses);
    }

    public List<TEntity> FindByCondition(Func<TEntity, bool> expression)
    {
        return this._context.Set<TEntity>().AsNoTracking().AsEnumerable().Where(expression).ToList() ;
    }

    public async Task<PaginatedResult<TEntity>> GetSortedPageAsync(Expression<Func<TEntity, bool>>? filterExpression, Expression<Func<TEntity, object>>? orderByExpression,
        PaginationRequest paginationRequest, CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> queryable = this._context.Set<TEntity>().AsQueryable();

        if (filterExpression is not null)
        {
            queryable = queryable.Where(filterExpression);
        }

        if (orderByExpression is not null)
        {
            queryable = queryable.OrderBy(orderByExpression);
        }

        long totalRecords = await queryable.CountAsync(cancellationToken);
        IEnumerable<TEntity> dataResponses = queryable.Skip(paginationRequest.PageIndex * paginationRequest.PageSize)
            .Take(paginationRequest.PageSize).ToList();
        return new PaginatedResult<TEntity>(paginationRequest.PageIndex, paginationRequest.PageSize, totalRecords,
            dataResponses);
    }

    public IQueryable<TEntity> Table()
    {
        return this._context.Set<TEntity>().AsQueryable().AsNoTracking();
    }

    public async Task<bool> ExecuteInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default)
    {
        var executionStrategy = _context.Database.CreateExecutionStrategy();
        return await executionStrategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await operation();
                var saveResult = await _context.SaveChangesAsync(cancellationToken);
                if (saveResult > 0)
                {
                    await transaction.CommitAsync(cancellationToken);
                    return true;
                }

                await transaction.RollbackAsync(cancellationToken);
                return false;
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw new BadRequestException("There was an error saving changes (transaction) or committing changes.");
            }
        });
    }

}