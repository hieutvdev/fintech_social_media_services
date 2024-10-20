using Article.Domain.ValueObjects;
using BuildingBlocks.Helpers;
using ShredKernel.Aggregates;

namespace Article.Domain.Models;

public class Tag : Entity<TagId>
{

    private readonly List<ArticleTag> _articleTags = new();

    public IReadOnlyList<ArticleTag> ArticleTags => _articleTags.AsReadOnly();
    
    public string Name { get; private set; } = default!;

    public string Slug { get; private set; } = default!;
    

    public static Tag Create(TagId tagId, string name)
    {
        var tag = new Tag
        {
            Id = tagId,
            Name = name,
            Slug = SlugHelper.GenerateSlug(name)
        };
        return tag;
    }
   
}