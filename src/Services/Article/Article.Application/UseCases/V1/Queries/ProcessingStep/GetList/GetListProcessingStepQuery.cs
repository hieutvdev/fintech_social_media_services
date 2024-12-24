using Article.Application.DTOs.Request.ProcessingStep;
using Article.Application.DTOs.Response.ProcessingStep;
using BuildingBlocks.CQRS.Queries;

namespace Article.Application.UseCases.V1.Queries.ProcessingStep.GetList;

public record GetListProcessingStepQuery(ProcessingStepSearchListDto Search) : IQuery<PaginatedResult<ProcessingStepResBaseDto>>;