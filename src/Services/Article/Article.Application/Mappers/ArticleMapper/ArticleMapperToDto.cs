using Article.Application.DTOs.Response.Article;

namespace Article.Application.Mappers.ArticleMapper;

public static class ArticleMapperToDto
{
    public static ArticleGetDetailsResponseDto ArticleToDetailDto(Article.Domain.Models.Article model)
    {
        // return new ArticleGetDetailsResponseDto(model.Id.Value.ToString(), model.Title, model.Description, model.Content, model.MainImageUrl, model.PublishAt, model.AuthorId, string.Join("|||", model.ArticleTags.Select(r => r.TagId.Value))  )
        return null;
    }


    public static ArticleResponseBaseDto ArticleToBaseDto(this Article.Domain.Models.Article model)
        => new ArticleResponseBaseDto(model.Id.Value.ToString(), model.Title, model.Description, model.MainImageUrl,
            model.Slug, model.PublishAt,model.TagsToString(), model.CategoriesToString());
}