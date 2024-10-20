using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;
using MediatR;

namespace BuildingBlocks.CQRS.Dispatchers;

public class CommandDispatcher(IMediator mediator) : ICommandDispatcher
{
   

    public async Task<Result> DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand
    {
        return await mediator.Send(command);
    }
}