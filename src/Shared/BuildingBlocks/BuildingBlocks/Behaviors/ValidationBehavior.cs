using BuildingBlocks.CQRS.Commands;
using FluentValidation;
using MediatR;

namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse> (IEnumerable<IValidator<TRequest>> validators)
: IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);

        var validationResults = await Task.WhenAll(
            validators.Select(v => v.ValidateAsync(context, cancellationToken))
        );

        var failures =
            validationResults
                .Where(e => e.Errors.Any())
                .SelectMany(r => r.Errors)
                .ToList();


        if (failures.Any())
        {
            throw new ValidationException(failures);
        }
        
        return await next();
    }
}