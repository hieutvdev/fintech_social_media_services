using Article.Domain.Enums;
using Article.Domain.ValueObjects;
using BuildingBlocks.Helpers;
using ShredKernel.Aggregates;

namespace Article.Domain.Models;

public class Category : Entity<CategoryId>, IAggregateRoot
{
    private readonly List<ArticleCategory> _articleCategories = new();

    public IReadOnlyList<ArticleCategory> ArticleCategories => _articleCategories.AsReadOnly();
    public string Name { get; private set; } = default!;
    
    public string Slug { get; private set; } = default!;

    public string Description { get; private set; } = default!;

    public CategoryStatus CategoryStatus { get; private set; } = default!;


    public static Category Create(CategoryId categoryId, string name, string description)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);

        var category = new Category
        {
            Id = categoryId,
            Name = name,
            Description = description,
            Slug = SlugHelper.GenerateSlug(name),
            CategoryStatus = CategoryStatus.Active
        };
        return category;
    }

    public void Update( string name, string description, CategoryStatus categoryStatus)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        Name = name;
        Description = description;
        Slug = SlugHelper.GenerateSlug(name);
        CategoryStatus = categoryStatus;
    }

}