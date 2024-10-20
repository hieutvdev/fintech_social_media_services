using BuildingBlocks.CQRS.Common;
using MediatR;

namespace BuildingBlocks.CQRS.Commands;

public interface ICommand : IRequest<Result>
{
    
}

public interface ICommand<TResponse> : IRequest<ResultT<TResponse>>
{
    
} 