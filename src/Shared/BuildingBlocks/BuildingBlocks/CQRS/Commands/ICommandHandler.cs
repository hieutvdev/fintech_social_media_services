using BuildingBlocks.CQRS.Common;
using MediatR;

namespace BuildingBlocks.CQRS.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
where TCommand : ICommand
{
}


public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, ResultT<TResponse>>
    where TCommand : ICommand<TResponse>
{
}