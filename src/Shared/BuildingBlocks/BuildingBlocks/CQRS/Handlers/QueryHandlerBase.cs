using BuildingBlocks.CQRS.Common;
using BuildingBlocks.CQRS.Queries;
using MediatR;

namespace BuildingBlocks.CQRS.Handlers;

public abstract class QueryHandlerBase<TQuery, TResult> : IRequestHandler<TQuery, ResultT<TResult>>
where TQuery : IQuery<TResult>
{
    public abstract Task<ResultT<TResult>> Handle(TQuery request, CancellationToken cancellationToken);
}