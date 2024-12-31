using System.Reflection;
using System.Text;
using System.Web;
using Microsoft.AspNetCore.Http;

namespace BuildingBlocks.Helpers;

public static class CachingHelper
{
    public static string GenerateCacheKeyFromRequest(HttpRequest request)
    {
        var keyBuilder = new StringBuilder();

        keyBuilder.Append(request.Path);

        foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
        {
            keyBuilder.Append($"|{key}-{value}");
        }

        return keyBuilder.ToString();
    }

    public static string ObjectToQueryString(object obj)
    {
        if (obj == null)
        {
            throw new ArgumentNullException(nameof(obj));
        }

        var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
        var queryString = new List<string>();

        foreach (var property in properties)
        {
            var value = property.GetValue(obj);
            if (value != null)
            {
                if (value is DateTime dateTimeValue)
                {
                    queryString.Add($"{property.Name}={HttpUtility.UrlEncode(dateTimeValue.ToString("o"))}");
                }
                else
                {
                    queryString.Add($"{property.Name}={HttpUtility.UrlEncode(value.ToString())}");
                }
            }
        }
        
        return string.Join("||", queryString);
    }
}