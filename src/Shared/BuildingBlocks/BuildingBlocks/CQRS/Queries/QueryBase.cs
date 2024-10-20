using BuildingBlocks.CQRS.Common;
using MediatR;

namespace BuildingBlocks.CQRS.Queries;

public abstract class QueryBase<TResult> : IQuery<TResult>
{

}