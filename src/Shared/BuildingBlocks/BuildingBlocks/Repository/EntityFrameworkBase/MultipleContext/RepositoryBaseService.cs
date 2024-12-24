using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;
using ShredKernel.Aggregates;

namespace BuildingBlocks.Repository.EntityFrameworkBase.MultipleContext;

public class RepositoryBaseService<TContext> : IRepositoryBaseService<TContext> where TContext : DbContext
{
    private readonly TContext _context;
    
    private readonly DbConnection _sqlConnection;

    public RepositoryBaseService(TContext context)
    {
        this._context = context;
        this._sqlConnection = _context.Database.GetDbConnection();
    }
    
    public IQueryable<T> Table<T>() where T : class, IAggregateRoot
    {
        return this._context.Set<T>().AsQueryable().AsNoTracking();
    }

    public IQueryable<T> Where<T>(Expression<Func<T, bool>> expression) where T : class, IAggregateRoot
    {
        return this._context.Set<T>().AsQueryable().AsNoTracking<T>().Where<T>(expression);
    }

    public async Task<IQueryable<T>> WhereAsync<T>(Expression<Func<T, bool>> expression) where T : class, IAggregateRoot
    {
        IQueryable<T> query = this._context.Set<T>().AsQueryable() ?? throw new ArgumentNullException(nameof(expression));
        IQueryable<T> queryable = await Task.FromResult<IQueryable<T>>(query.AsNoTracking<T>().Where<T>(expression));
        query = (IQueryable<T>)null!;
        return queryable;
    }

    public IQueryable<T> WhereTracking<T>(Expression<Func<T, bool>> expression) where T : class, IAggregateRoot
    {
        return this._context.Set<T>().AsQueryable().Where<T>(expression);
    }

    public async Task<IQueryable<T>> WhereTrackingAsync<T>(Expression<Func<T, bool>> expression) where T : class, IAggregateRoot
    {
        IQueryable<T> query = this._context.Set<T>().AsQueryable();
        IQueryable<T> queryable = await Task.FromResult<IQueryable<T>>(query.Where<T>(expression));
        query = (IQueryable<T>)null!;
        return queryable;
    }

    public async Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot
    {
        cancellationToken.ThrowIfCancellationRequested();
        if ((object)(T)entity == null)
        {
            throw new BadRequestException(nameof(entity));
        }
        DbSet<T> dbSet = this._context.Set<T>();
        EntityEntry<T> entityEntry = await dbSet.AddAsync(entity, cancellationToken);
        T entity1 = entityEntry.Entity;
        dbSet = (DbSet<T>)null!;
        return entity1;
    }

    public EntityEntry<T> Update<T>(T entity) where T : class, IAggregateRoot
    {
        return (object)entity != null ? this._context.Set<T>().Update(entity) : throw new BadRequestException(nameof(entity));
    }

    public void Update<T>(IEnumerable<T> entities) where T : class, IAggregateRoot
    {
        if (entities == null)
        {
            throw new BadRequestException(nameof(entities));
        }
        this._context.Set<T>().UpdateRange(entities);
    }

    public EntityEntry<T> Delete<T>(T entity) where T : class, IAggregateRoot
    {
        return (object)entity != null
            ? this._context.Set<T>().Remove(entity)
            : throw new BadRequestException(nameof(entity));
    }

    public void Delete<T>(IEnumerable<T> entities) where T : class, IAggregateRoot
    {
        if (entities == null)
        {
            throw new BadRequestException(nameof(entities));
        }
        this._context.Set<T>().RemoveRange(entities);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken), bool configure = false)
    {
        int isSuccess = await this._context.SaveChangesAsync(cancellationToken).ConfigureAwait(configure);
        return isSuccess;
    }

    

    public int SaveChanges()
    {
        return this._context.SaveChanges();
    }

    public async Task AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot
    {
        if (entities == null)
        {
            throw new BadRequestException(nameof(entities));
        }
        DbSet<T> dbSet = this._context.Set<T>();
        await dbSet.AddRangeAsync(entities, cancellationToken);
        dbSet = (DbSet<T>)null!;
    }

    public async Task<T?> FirstOrDefaultAsNoTrackingAsync<T>(Expression<Func<T?, bool>> condition,
        CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot
    {
        IQueryable<T> queryable = this._context.Set<T>().AsQueryable().AsNoTracking<T>();
        T? objectFind = await queryable.FirstOrDefaultAsync(condition, cancellationToken);
        queryable = (IQueryable<T>)null!;
        return objectFind;
    }

    public async Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T?, bool>> condition, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot
    {
        IQueryable<T> queryable = this._context.Set<T>().AsQueryable();
        T? objectFind = await queryable.FirstOrDefaultAsync(condition, cancellationToken);
        queryable = (IQueryable<T>)null!;
        return objectFind;
    }

    public T? FirstOrDefault<T>(Expression<Func<T?, bool>> condition) where T : class, IAggregateRoot
    {
        return this._context.Set<T>().AsQueryable().AsNoTracking<T>().FirstOrDefault<T>(condition!);
    }

    public async Task<int> UpdateOrInsertEntities<T>(List<T> updateEntities, Expression<Func<T, bool>> condition, Func<T, T, bool>? isEqual, bool save = true) where T : class, IAggregateRoot
    {
        if (condition == null)
        {
            throw new ArithmeticException(nameof(condition));
        }
        DbSet<T> dbSet = this._context.Set<T>();
        List<T> existEntities = await dbSet.AsNoTracking<T>().Where<T>(condition).ToListAsync();
        if (existEntities.Any<T>())
        {
            if (!((IEnumerable<T>)updateEntities).Any<T>())
            {
                dbSet.RemoveRange((IEnumerable<T>)existEntities);
            }else if (isEqual != null)
            {
                IEnumerable<T> entitiesToDel = existEntities.Where<T>((Func<T, bool>)(db =>
                    !((IEnumerable<T>)updateEntities).Any<T>((Func<T, bool>)(update => isEqual(update, db)))));

                var aggregateRoots = entitiesToDel.ToList();
                if(aggregateRoots.Any<T>())
                {
                    dbSet.RemoveRange(aggregateRoots);
                    entitiesToDel = (IEnumerable<T>)null!;
                }
            }
        }

        if (((IEnumerable<T>)updateEntities).Any<T>())
        {
            foreach (var updateEntity in updateEntities)
            {
                T updateE = updateEntity;
                T? existEntity = existEntities.FirstOrDefault<T>((Func<T, bool>)(db =>
                    isEqual != null && isEqual(updateE, db)));
                if ((object) existEntity != null)
                {
                    dbSet.Update(updateEntity);
                }
                else
                {
                    EntityEntry<T> entityEntry = await dbSet.AddAsync(updateE);
                }
                existEntity = default(T);
            }
        }

        int num;
        if (save)
        {
            num = await this._context.SaveChangesAsync();
        }
        else
        {
            num = -1;
        }
        int isSuccess = num;
        dbSet = (DbSet<T>)null!;
        existEntities = (List<T>)null!;
        return isSuccess;
    }

    public async Task<bool> ExecuteUpdateAsync<T>(Expression<Func<T, bool>> condition, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> updateExpression,
        CancellationToken cancellationToken = default) where T : class, IAggregateRoot
    {
        if (condition == null || updateExpression == null)
        {
            throw new ArgumentNullException(nameof(condition), "Condition and update expression must be provided.");
        }

        var updatedCount = await _context.Set<T>()
            .Where(condition)
            .ExecuteUpdateAsync(updateExpression, cancellationToken);

        return updatedCount > 0;
    }

    public async Task<bool> ExecuteDeleteAsync<T>(Expression<Func<T, bool>> condition, CancellationToken cancellationToken = default) where T : class, IAggregateRoot
    {
        if (condition == null)
        {
            throw new ArgumentNullException(nameof(condition), "Condition must be provided.");
        }

        var deletedCount = await _context.Set<T>()
            .Where(condition)
            .ExecuteDeleteAsync(cancellationToken);
        
        return deletedCount > 0;
    }



    public void Dispose() => GC.SuppressFinalize((object)this);
    
    
    public async Task<T?> QueryFirstOrDefaultAsync<T>(
        string query = "", 
        object parameters = null, 
        CommandType commandType = CommandType.Text)
    {
        if (string.IsNullOrWhiteSpace(query))
            throw new ArgumentException("Query cannot be null or empty.", nameof(query));

        if (_sqlConnection.State != ConnectionState.Open)
            await _sqlConnection.OpenAsync();

        try
        {
            return await _sqlConnection.QueryFirstOrDefaultAsync<T>(
                query, 
                parameters, 
                commandType: commandType);
        }
        finally
        {
            await _sqlConnection.CloseAsync();
        }
    }


    public T QueryFirst<T>(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        DbConnection connection = this._sqlConnection;
        string str = qr;
        object obj = pr;
        CommandType? type1 = new CommandType?(commandType);
        int? type2 = new int?();
        CommandType? type3 = type1;
        return ((IDbConnection)connection).QueryFirst<T>(str, obj, (IDbTransaction)null, type2, type3);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        DbConnection connection = this._sqlConnection;
        DbConnection sqlConnection = connection;

        string str = qr;
        object obj = pr;

        CommandType? type1 = new CommandType?(commandType);
        int? type2 = new int?();
        CommandType? type3 = type1;
        
        IEnumerable<T> objs = await ((IDbConnection)sqlConnection).QueryAsync<T>(str, obj, (IDbTransaction)null!, type2, type3);
        connection = (DbConnection)null!;
        sqlConnection = (DbConnection)null!;
        return objs;
    }

    public IEnumerable<T> Query<T>(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        DbConnection connection = this._sqlConnection;
        string str = qr;
        object obj = pr;
        CommandType? type1 = new CommandType?(commandType);
        int? type2 = new int?();
        CommandType? type3 = type1;
        return ((IDbConnection)connection).Query<T>(str, obj, (IDbTransaction)null!, true, type2, type3);
    }

    public Task<SqlMapper.GridReader> QueryMultipleAsync(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        DbConnection connection = this._sqlConnection;
        DbConnection sqlConnection = connection;
        string str = qr;
        object obj = pr;
        CommandType? type1 = new CommandType?(commandType);
        int? type2 = new int?();
        CommandType? type3 = type1;
        Task<SqlMapper.GridReader> task = ((IDbConnection)sqlConnection).QueryMultipleAsync(str, obj, (IDbTransaction)null!, type2, type3);
        connection = (DbConnection)null!;
        sqlConnection = (DbConnection)null!;
        return task;
    }

    public SqlMapper.GridReader QueryMultiple(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        DbConnection connection = this._sqlConnection;
        string str = qr;
        object obj = pr;
        CommandType? type1 = new CommandType?(commandType);
        int? type2 = new int?();
        CommandType? type3 = type1;
        return ((IDbConnection)connection).QueryMultiple(str, obj, (IDbTransaction)null!, type2, type3);
    }
    
    public async Task<T> TransactionAsync<T>(Func<Task<T>> func, CancellationToken cancellationToken = default(CancellationToken),
        bool configure = false)
    {
        if (func == null)
        {
            throw new ArgumentNullException();
        }

        T obj3;

        try
        {
            IExecutionStrategy strategy = this._context.Database.CreateExecutionStrategy();
            T response = default(T);

            await strategy.ExecuteAsync((Func<Task>)(async () =>
            {
                IDbContextTransaction transaction = await this.BeginTransactionAsync();
                object obj1 = (object)null;

                int num = 0;

                try
                {
                    using (Serilog.Context.LogContext.PushProperty("TransactionContext", (object)transaction.TransactionId, false))
                    {
                        if (transaction == null)
                        {
                            throw new ArgumentNullException("Transaction");
                        }

                        try
                        {
                            Log.Information<Guid, DateTime>("----- Begin transaction {TransactionId} - {date}", transaction.TransactionId, DateTime.UtcNow);
                            T obj2 = await func();
                            response = obj2;
                            obj2 = default(T);
                            Log.Information<Guid, DateTime>("----- Commit transaction {TransactionId} - {date}", transaction.TransactionId, DateTime.UtcNow);
                            await transaction.CommitAsync(cancellationToken);
                        }
                        catch (Exception e)
                        {
                            Log.Error(e.GetType().ToString());
                            Log.Error(e.Message);
                            await this.RollbackTransactionAsync(transaction, cancellationToken);
                            if (e.GetType() != typeof(DbUpdateException))
                            {
                                throw;
                            }

                            throw new BadRequestException(e.Message);
                        }
                        finally
                        {
                            await transaction.DisposeAsync();
                        }

                        num = 1;
                    }
                }
                catch (Exception e)
                {
                    obj1 = e;
                }

                if (transaction != null)
                {
                    await transaction.DisposeAsync();
                }

                object obj4 = obj1;
                if (obj4 != null)
                {
                    if (!(obj4 is Exception source2))
                    {
                        throw new BadRequestException("obj4");
                    }
                    ExceptionDispatchInfo.Capture(source2).Throw();
                }

                if (num == 1)
                {
                    transaction = (IDbContextTransaction)null!;
                }
                else
                {
                    obj1 = (object)null!;
                    transaction = (IDbContextTransaction)null!;
                    transaction = (IDbContextTransaction)null!;
                }
            }));
            obj3 = response!;
        }
        catch (Exception e)
        {
            Log.Error(e.Message);
            if (!(e.GetType() != typeof(DbUpdateException)))
            {
                throw new BadRequestException(e.Message);
            }

            throw;
        }

        return obj3;
    }

    private async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        IDbContextTransaction contextTransaction = await this._context.Database.BeginTransactionAsync();
        return contextTransaction;
    }

    private async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken,
        bool configure)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException();
        }

        try
        {
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await this.RollbackTransactionAsync(transaction, cancellationToken);
            throw;
        }
        finally
        {
            await transaction.DisposeAsync();
        }
    }
    
    private async Task RollbackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException();
        }

        try
        {
            await transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            await transaction.DisposeAsync();
        }
    }
    
    
}