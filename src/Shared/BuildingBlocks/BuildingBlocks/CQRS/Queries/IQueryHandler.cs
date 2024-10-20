using BuildingBlocks.CQRS.Common;
using MediatR;

namespace BuildingBlocks.CQRS.Queries;

public interface IQueryHandler<in TRequest, TResponse> : IRequestHandler<TRequest, ResultT<TResponse>>
where TRequest : IQuery<TResponse>
{
    
}