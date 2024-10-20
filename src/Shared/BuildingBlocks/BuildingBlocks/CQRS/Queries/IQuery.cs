using BuildingBlocks.CQRS.Common;
using MediatR;

namespace BuildingBlocks.CQRS.Queries;

public interface IQuery
{
    
}


public interface IQuery<TResponse> : IRequest<ResultT<TResponse>>
{
    
}