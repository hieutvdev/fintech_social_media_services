using Article.Application.DTOs.Response.ProcessingStep;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ProcessingStep.GetAll;

public record GetAllProcessingStepQuery() : IQuery<IEnumerable<ProcessingStepResBaseDto>>;
