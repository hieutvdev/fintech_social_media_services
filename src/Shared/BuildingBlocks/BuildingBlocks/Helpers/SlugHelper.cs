using System.Text.RegularExpressions;

namespace BuildingBlocks.Helpers;

public static class SlugHelper
{
    public static string GenerateSlug(string name)
    {
        var slug = Regex.Replace(name.ToLower(), @"\s+", "-");

        slug = Regex.Replace(slug, @"[^a-z0-9\-]", "");

        return slug;
    }

    public static string GenerateUniqueSlug(string name, Func<string, bool> slugExists)
    {
        var baseSlug = GenerateSlug(name);
        var uniqueSlug = baseSlug;
        var counter = 1;

        while (slugExists(uniqueSlug))
        {
            uniqueSlug = $"{baseSlug}-{counter}";
            counter++;
        }

        return uniqueSlug;
    }
}