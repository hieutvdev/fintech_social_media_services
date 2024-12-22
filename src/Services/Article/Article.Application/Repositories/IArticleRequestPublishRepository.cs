namespace Article.Application.Repositories;

public interface IArticleRequestPublishRepository
{
    Task<bool> CreateAsync(CancellationToken cancellationToken = default!);
}