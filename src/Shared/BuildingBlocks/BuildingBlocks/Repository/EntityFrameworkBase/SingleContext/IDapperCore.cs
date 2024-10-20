using System.Collections;
using System.Data;
using Dapper;

namespace BuildingBlocks.Repository.EntityFrameworkBase.SingleContext;


#nullable disable
public interface IDapperCore : IDisposable
{
    Task<T> QueryFirstOrDefaultAsync<T>(string qr = "", object pr = null,
        CommandType commandType = CommandType.Text);

    T QueryFirst<T>(string qr = "", object pr = null, CommandType commandType = CommandType.Text);
    
    Task<IEnumerable<T>> QueryAsync<T>(string qr = "", object pr = null, CommandType commandType = CommandType.Text);
    
    IEnumerable<T> Query<T>(string qr = "", object pr = null, CommandType commandType = CommandType.Text);
    
    Task<SqlMapper.GridReader> QueryMultipleAsync(string qr = "", object pr = null, CommandType commandType = CommandType.Text);
    
    SqlMapper.GridReader QueryMultiple(string qr = "", object pr = null, CommandType commandType = CommandType.Text);
    
}