using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;

namespace BuildingBlocks.CQRS.Dispatchers;

public interface IQueryDispatcher
{
    Task<ResultT<TResult>> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
}