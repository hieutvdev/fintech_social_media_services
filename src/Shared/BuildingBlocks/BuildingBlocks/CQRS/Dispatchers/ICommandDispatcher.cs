using BuildingBlocks.CQRS.Commands;
using BuildingBlocks.CQRS.Common;

namespace BuildingBlocks.CQRS.Dispatchers;

public interface ICommandDispatcher
{
    Task<Result> DispatchAsync<TCommand>(TCommand command) where TCommand : ICommand;
    
}