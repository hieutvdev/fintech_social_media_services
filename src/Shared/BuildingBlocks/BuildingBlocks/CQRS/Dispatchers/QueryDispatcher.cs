using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;
using MediatR;

namespace BuildingBlocks.CQRS.Dispatchers;

public class QueryDispatcher(IMediator mediator) : IQueryDispatcher
{
    public async Task<ResultT<TResult>> DispatchAsync<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
    {
        return await mediator.Send(query);
    }
}