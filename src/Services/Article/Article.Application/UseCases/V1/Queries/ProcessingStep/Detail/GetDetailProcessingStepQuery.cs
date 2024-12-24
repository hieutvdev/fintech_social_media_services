using Article.Application.DTOs.Response.ProcessingStep;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ProcessingStep.Detail;

public record GetDetailProcessingStepQuery(string Id) : IQuery<ProcessingStepResBaseDto>;