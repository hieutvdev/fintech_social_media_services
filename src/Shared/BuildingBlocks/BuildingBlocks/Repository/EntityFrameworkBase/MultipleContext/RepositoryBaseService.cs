using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        throw new NotImplementedException();
    }

    public IQueryable<IQueryable<T>> WhereAsync<T>(Expression<Func<T, bool>> expression) where T : class, IAggregateRoot
    {
        throw new NotImplementedException();
    }

    public IQueryable<T> WhereTracking<T>(Expression<Func<T, bool>> expression) where T : class, IAggregateRoot
    {
        throw new NotImplementedException();
    }

    public Task<IQueryable<T>> WhereTrackingAsync<T>(Expression<Func<T, bool>> expression) where T : class, IAggregateRoot
    {
        throw new NotImplementedException();
    }

    public Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot
    {
        throw new NotImplementedException();
    }

    public EntityEntry<T> Update<T>(T entity) where T : class, IAggregateRoot
    {
        throw new NotImplementedException();
    }

    public void Update<T>(IEnumerable<T> entities) where T : class, IAggregateRoot
    {
        throw new NotImplementedException();
    }

    public EntityEntry<T> Delete<T>(T entity) where T : class, IAggregateRoot
    {
        throw new NotImplementedException();
    }

    public void Delete<T>(IEnumerable<T> entities) where T : class, IAggregateRoot
    {
        throw new NotImplementedException();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken), bool configure = false)
    {
        throw new NotImplementedException();
    }

    public Task<T> TransactionAsync<T>(Func<Task<T>> func, CancellationToken cancellationToken = default(CancellationToken),
        bool configure = false)
    {
        throw new NotImplementedException();
    }

    public int SaveChanges()
    {
        throw new NotImplementedException();
    }

    public Task AddAsync<T>(IEnumerable<T> entities, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot
    {
        throw new NotImplementedException();
    }

    public Task<T?> FirstOrDefaultAsNoTrackingAsync<T>(Expression<Func<T?, bool>> condition,
        CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot
    {
        throw new NotImplementedException();
    }

    public Task<T?> FirstOrDefaultAsync<T>(Expression<Func<T?, bool>> condition, CancellationToken cancellationToken = default(CancellationToken)) where T : class, IAggregateRoot
    {
        throw new NotImplementedException();
    }

    public T? FirstOrDefault<T>(Expression<Func<T?, bool>> condition) where T : class, IAggregateRoot
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateOrInsertEntities<T>(List<T> updateEntities, Expression<Func<T, bool>> condition, Func<T, T, bool> isEqual, bool save = true) where T : class, IAggregateRoot
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> QueryAsync<T>(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> Query<T>(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        throw new NotImplementedException();
    }

    public Task<SqlMapper.GridReader> QueryMultipleAsync(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        throw new NotImplementedException();
    }

    public SqlMapper.GridReader QueryMultiple(string qr = "", object pr = null, CommandType commandType = CommandType.Text)
    {
        throw new NotImplementedException();
    }
}