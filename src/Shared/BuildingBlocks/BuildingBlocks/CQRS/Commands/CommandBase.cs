using BuildingBlocks.CQRS.Common;
using MediatR;

namespace BuildingBlocks.CQRS.Commands;

public abstract class CommandBase<TCommand> : ICommand
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
   
}