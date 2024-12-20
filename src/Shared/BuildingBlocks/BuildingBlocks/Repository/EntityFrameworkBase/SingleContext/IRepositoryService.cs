
using System.Linq.Expressions;
using BuildingBlocks.Pagination;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;

public interface IRepositoryService<in T, TEntity, TContext>
    where TEntity : class
    where TContext : DbContext
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default!);
    
    Task<TEntity?> GetByIdAsync(T? id, CancellationToken cancellationToken = default!);
    
    Task AddAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default!);

    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default!);
    
    EntityEntry Update(TEntity entity);
    
    EntityEntry Delete(TEntity entity);

    void DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default!);
    
    Task<int> SaveChangeAsync(CancellationToken cancellationToken = default!);
    
    IEnumerable<TEntity> Where(Expression<Func<TEntity, bool>> expression);

    Task<IQueryable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> expression,
        CancellationToken cancellationToken = default!);

    Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity?, bool>> expression,
        CancellationToken cancellationToken = default!);

    Task<PaginatedResult<TEntity>> GetPageAsync(PaginationRequest paginationRequest, Expression<Func<TEntity, bool>>? expression,
        CancellationToken cancellationToken = default!);


    Task<IEnumerable<TResult>> GetSelectorAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
        CancellationToken cancellationToken = default!);
    Task<PaginatedResult<TResult>> GetSelectAsync<TResult>(
        Expression<Func<TEntity, TResult>> selector,
        Expression<Func<TEntity, bool>>? expression, 
        PaginationRequest paginationRequest,
        CancellationToken cancellationToken = default!);

    List<TEntity> FindByCondition(Func<TEntity, bool> expression);

    Task<PaginatedResult<TEntity>> GetSortedPageAsync(Expression<Func<TEntity, bool>>? filterExpression,
        Expression<Func<TEntity, object>>? orderByExpression, PaginationRequest paginationRequest,
        CancellationToken cancellationToken = default!);

    IQueryable<TEntity> Table();

    Task<bool> ExecuteInTransactionAsync(Func<Task> operation, CancellationToken cancellationToken = default!);


}