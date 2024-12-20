using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using MediatR;

namespace BuildingBlocks.CQRS.Handlers;

public abstract class CommandHandlerBase<TCommand> : IRequestHandler<TCommand, Result> 
    where TCommand : ICommand
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public abstract Task<Result> Handle(TCommand request, CancellationToken cancellationToken);

}