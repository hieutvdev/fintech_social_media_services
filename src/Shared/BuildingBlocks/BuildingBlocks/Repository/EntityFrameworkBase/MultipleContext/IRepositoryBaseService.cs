using System.Linq.Expressions;
using BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using ShredKernel.Aggregates;

namespace BuildingBlocks.Repository.EntityFrameworkBase.MultipleContext;

public interface IRepositoryBaseService<TContext> : IDapperCore, IDisposable where TContext : DbContext
{
    IQueryable<T> Table<T>() where T : class, IAggregateRoot;
    
    Task<int> CountAsync<T>(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot;

    IQueryable<T> Where<T>(Expression<Func<T, bool>> expression) where T : class, IAggregateRoot;

    Task<IQueryable<T>> WhereAsync<T>(Expression<Func<T, bool>> expression) where T : class, IAggregateRoot;
    
    IQueryable<T> WhereTracking<T>(Expression<Func<T, bool>> expression) where T : class, IAggregateRoot;

    Task<IQueryable<T>> WhereTrackingAsync<T>(Expression<Func<T, bool>> expression) where T : class, IAggregateRoot;

    Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken = default(CancellationToken))
        where T : class, IAggregateRoot;

    EntityEntry<T> Update<T>(T entity) where T : class, IAggregateRoot;

    void Update<T>(IEnumerable<T> entities) where T : class, IAggregateRoot;

    EntityEntry<T> Delete<T>(T entity) where T : class, IAggregateRoot;

    void Delete<T>(IEnumerable<T> entities) where T : class, IAggregateRoot;

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken), bool configure = false);

    Task<T> TransactionAsync<T>(Func<Task<T>> func, CancellationToken cancellationToken = default(CancellationToken),
        bool configure = false);

    int SaveChanges();

    Task AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken))
        where T : class, IAggregateRoot;

    Task<T?> FirstOrDefaultAsNoTrackingAsync<T>(Expression<Func<T?, bool>> condition,
        CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot;

    Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T?, bool>> condition,
        CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot;

    T? FirstOrDefault<T>(Expression<Func<T?, bool>> condition) where T : class, IAggregateRoot;

    Task<int> UpdateOrInsertEntities<T>(List<T> updateEntities, Expression<Func<T, bool>> condition,
        Func<T, T, bool>? isEqual, bool save = true) where T : class, IAggregateRoot;

    Task<bool> ExecuteUpdateAsync<T>(
        Expression<Func<T, bool>> condition,
        Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> updateExpression,
        CancellationToken cancellationToken = default) where T : class, IAggregateRoot;
    
    Task<bool> ExecuteDeleteAsync<T>(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default) where T : class, IAggregateRoot;
}
