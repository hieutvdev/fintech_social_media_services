using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using BuildingBlocks.Exceptions;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using ShredKernel.Aggregates;
using Serilog;
using System.Runtime.ExceptionServices;

namespace BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;
#nullable disable
public class RepositoryServiceV2 : IRepositoryServiceV2, IDapperCore, IDisposable
{
    private readonly DbContext _context;

    private readonly string _connectionString;

    private readonly DbConnection _sqlConnection;

    public RepositoryServiceV2(DbContext context)
    {
        this._context = context ?? throw new ArgumentNullException(nameof(context));
        // this._connectionString = this._context.Database.Get
    }
 
    public void Dispose() => GC.SuppressFinalize((object) this); 

    public async Task<T> QueryFirstOrDefaultAsync<T>(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        DbConnection connection = this._sqlConnection;
        DbConnection dbConnection = connection;

        string str = qr;
        object obj1 = pr;
        CommandType? nullable1 = new CommandType?(commandType);
        int? nullable2 = new int?();
        CommandType? nullable3 = nullable1;
        T obj2 = await SqlMapper.QueryFirstOrDefaultAsync<T>((IDbConnection)dbConnection, str, obj1,
            (IDbTransaction)null, nullable2, nullable3);
        connection = (DbConnection)null;
        return obj2;
    }

    public T QueryFirst<T>(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        DbConnection connection = this._sqlConnection;
        string str = qr;
        object obj = pr;
        CommandType? nullable1 = new CommandType?(commandType);
        int? nullable2 = new int?();
        CommandType? nullable3 = nullable1;
        return SqlMapper.QueryFirst<T>((IDbConnection)connection, str, obj, (IDbTransaction)null, nullable2, nullable3);

    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        DbConnection connection = this._sqlConnection;
        DbConnection sqlConnection = connection;
        string str = qr;
        object obj = pr;
        CommandType? nullable1 = new CommandType?(commandType);
        int? nullable2 = new int?();
        CommandType? nullable3 = nullable1;
        IEnumerable<T> objs = await ((IDbConnection)sqlConnection).QueryAsync<T>(str, obj,
            (IDbTransaction)null, nullable2, nullable3);
        connection = (DbConnection)null;
        return objs;
    }

    public IEnumerable<T> Query<T>(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        DbConnection connection = this._sqlConnection;
        string str = qr;
        object obj = pr;
        CommandType? nullable1 = new CommandType?(commandType);
        int? nullable2 = new int?();
        CommandType? nullable3 = nullable1;
        return SqlMapper.Query<T>((IDbConnection)connection, str, obj, (IDbTransaction)null, true, nullable2, nullable3);
    }

    public async Task<SqlMapper.GridReader> QueryMultipleAsync(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        DbConnection connection = this._sqlConnection;
        DbConnection sqlConnection = connection;
        string str = qr;
        object obj = pr;
        CommandType? nullable1 = new CommandType?(commandType);
        int? nullable2 = new int?();
        CommandType? nullable3 = nullable1;
        SqlMapper.GridReader gridReader = await SqlMapper.QueryMultipleAsync((IDbConnection)sqlConnection, str, obj,
            (IDbTransaction)null, nullable2, nullable3);
        return gridReader;
    }

    public SqlMapper.GridReader QueryMultiple(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        DbConnection sqlConnection = this._sqlConnection;
        string str = qr;
        object obj = pr;
        CommandType? nullable1 = new CommandType?(commandType);
        int? nullable2 = new int?();
        CommandType? nullable3 = nullable1;
        return SqlMapper.QueryMultiple((IDbConnection) sqlConnection, str, obj, (IDbTransaction) null, nullable2, nullable3);
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
        IQueryable<T> _query = this._context.Set<T>().AsQueryable();
        IQueryable<T> queryable = await Task.FromResult<IQueryable<T>>(_query.AsNoTracking<T>().Where<T>(expression));
        _query = (IQueryable<T>)null;
        return queryable;
    }

    public IQueryable<T> WhereTracking<T>(Expression<Func<T, bool>> expression) where T : class, IAggregateRoot
    {
        return this._context.Set<T>().AsQueryable().Where<T>(expression);
    }

    public async Task<IQueryable<T>> WhereTrackingAsync<T>(Expression<Func<T, bool>> expression) where T : class, IAggregateRoot
    {
        IQueryable<T> _query = this._context.Set<T>().AsQueryable();
        IQueryable<T> queryable = await Task.FromResult<IQueryable<T>>(_query.Where<T>(expression));
        _query = (IQueryable<T>)null;
        return queryable;
    }

    public virtual async Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot
    {
        cancellationToken.ThrowIfCancellationRequested();
        if ((object)(T)entity == null)
            throw new BadRequestException(nameof(entity));
        DbSet<T> dbSet = this._context.Set<T>();
        EntityEntry<T> entityEntry = await dbSet.AddAsync(entity, cancellationToken);
        T entity1 = entityEntry.Entity;
        dbSet = (DbSet<T>)null;
        return entity1;
    }

    public virtual EntityEntry<T> Update<T>(T entity) where T : class, IAggregateRoot
    {
        return (object)entity != null
            ? this._context.Set<T>().Update(entity)
            : throw new BadRequestException(nameof(entity));
    }

    public void Update<T>(IEnumerable<T> entities) where T : class, IAggregateRoot
    {
        if (entities == null)
        {
            throw new BadRequestException(nameof(entities));
        }
        this._context.Set<T>().UpdateRange(entities);
    }

    public virtual EntityEntry<T> Delete<T>(T entity) where T : class, IAggregateRoot
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
        int num = await this._context.SaveChangesAsync(cancellationToken).ConfigureAwait(configure);
        return num;
    }

    public async Task<T> TransactionAsync<T>(Func<Task<T>> func, CancellationToken cancellationToken = default(CancellationToken),
        bool configure = false)
    {
        if (func == null)
        {
            throw new ArgumentNullException(nameof(func));
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
                    using (Serilog.Context.LogContext.PushProperty("TransactionContext", (object) transaction.TransactionId, false))
                    {
                        if (transaction == null)
                            throw new ArgumentNullException("transaction");

                        try
                        {
                            Log.Information<Guid, DateTime>("----- Begin transaction {TransactionId} - {date}", transaction.TransactionId, DateTime.UtcNow);
                            T obj2 = await func();
                            response = obj2;
                            obj2 = default(T);
                            Log.Information<Guid, DateTime>("----- Commit transaction {TransactionId} - {date}", transaction.TransactionId, DateTime.UtcNow);
                            await transaction.CommitAsync(cancellationToken);
                        }
                        catch (Exception ex) 
                        {
                            Log.Error(ex.GetType().ToString());
                            Log.Error(ex.Message);
                            await this.RollBackTransactionAsync(transaction, cancellationToken);
                            if (ex.GetType() != typeof(DbUpdateException))
                                throw;
                            throw new BadRequestException(ex.Message);
                        }
                        finally
                        {
                            await transaction.DisposeAsync();
                        }
                    }
                    num = 1;
                }
                catch (Exception ex)
                {
                    obj1 = ex;
                }
                if(transaction != null)
                {
                    await transaction.DisposeAsync();
                }
                object obj4 = obj1;
                if(obj4 != null)
                {
                    if (!(obj4 is Exception source2))
                        throw new BadRequestException("obj4");
                    ExceptionDispatchInfo.Capture(source2).Throw();
                }
                if(num == 1)
                {
                    transaction = (IDbContextTransaction)null;
                }
                else
                {
                    obj1 = (object)null;
                    transaction = (IDbContextTransaction)null;
                    transaction = (IDbContextTransaction)null;
                }
            }));
            obj3 = response;
        }
        catch(Exception ex)
        {
            Log.Error(ex.Message);
            if (!(ex.GetType() != typeof(DbUpdateException)))
                throw new BadRequestException(ex.Message);
            throw;
        }
        return obj3;
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        IDbContextTransaction contextTransaction = await this._context.Database.BeginTransactionAsync();
        return contextTransaction;
    }

    public async Task CommitTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken,
        bool configure)
    {
        if (transaction == null)
        {
            throw new ArgumentNullException(nameof(transaction));
        }

        try
        {
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            await this.RollBackTransactionAsync(transaction, cancellationToken);
            throw;
        }
        finally
        {
            if (transaction != null)
            {
                await transaction.DisposeAsync();
            }
        }
    }

    public async Task RollBackTransactionAsync(IDbContextTransaction transaction, CancellationToken cancellationToken)
    {
        try
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            if (transaction != null)
            {
                await transaction.DisposeAsync();
            }
        }
    }

    public int SaveChanges()
    {
        int num = this._context.SaveChanges();
        return num;
    }

    public async Task AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot
    {
        if (entities == null)
        {
            throw new BadRequestException(nameof(entities));
        }

        DbSet<T> _dbSet = this._context.Set<T>();
        await _dbSet.AddRangeAsync(entities, cancellationToken);
        _dbSet = (DbSet<T>)null;
    }

    public async Task<T?> FirstOrDefaultAsNoTrackingAsync<T>(Expression<Func<T?, bool>> condition,
        CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot
    {
        IQueryable<T> _query = this._context.Set<T>().AsQueryable().AsNoTracking<T>();
        T obj = await _query.FirstOrDefaultAsync(condition, cancellationToken);
        _query = (IQueryable<T>)null;
        return obj;
    }

    public async Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T?, bool>> condition, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot
    {
        IQueryable<T> _query = this._context.Set<T>().AsQueryable();
        T obj = await _query.FirstOrDefaultAsync<T>(condition, cancellationToken);
        _query = (IQueryable<T>)null;
        return obj;
    }

    public T? FirstOrDefault<T>(Expression<Func<T?, bool>> condition) where T : class, IAggregateRoot
    {
        return this._context.Set<T>().AsQueryable().FirstOrDefault<T>(condition);
    }

    public async Task<int> UpdateOrInsertEntities<T>(List<T> updateEntities, Expression<Func<T, bool>> condition, Func<T, T, bool> isEqual, bool save = true) where T : class, IAggregateRoot
    {
        if (condition == null)
        {
            throw new ArgumentNullException(nameof(condition));
        }

        DbSet<T> _dbSet = this._context.Set<T>();
        List<T> existingEntities = await _dbSet.AsNoTracking<T>().Where<T>(condition).ToListAsync<T>();
        if (existingEntities.Any<T>())
        {
            if (!((IEnumerable<T>)updateEntities).Any<T>())
            {
                _dbSet.RemoveRange((IEnumerable<T>) existingEntities);
            }else if (isEqual != null)
            {
                IEnumerable<T> entitiesToDelete = existingEntities.Where<T>((Func<T, bool>) (db => !((IEnumerable<T>) updateEntities).Any<T>((Func<T, bool>) (updated => isEqual(updated, db)))));
                if (entitiesToDelete.Any<T>())
                    _dbSet.RemoveRange(entitiesToDelete);
                entitiesToDelete = (IEnumerable<T>) null;
            }
        }

        if (((IEnumerable<T>) updateEntities).Any<T>())
        {
            foreach (var updateEntity in updateEntities)
            {
                T updateE = updateEntity;
                T existingEntity =
                    existingEntities.FirstOrDefault<T>((Func<T, bool>)(db =>
                        isEqual != null && isEqual(updateE, db)));
                if ((object) existingEntity != null)
                {
                    _dbSet.Update(existingEntity);
                }
                else
                {
                    EntityEntry<T> entityEntry = await _dbSet.AddAsync(updateE);
                }

                existingEntity = default(T);
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

        int num1 = num;
        _dbSet = (DbSet<T>)null;
        existingEntities = (List<T>)null;
        return num1;
    }
}