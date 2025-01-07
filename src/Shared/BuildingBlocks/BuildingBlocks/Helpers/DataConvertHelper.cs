using Newtonsoft.Json;

namespace BuildingBlocks.Helpers;

public static class DataConvertHelper
{
    public static bool TryParseDateTime(string value, out DateTime result)
    {
        return DateTime.TryParse(value, out result);
    }
    
    public static bool TryParseObject<T>(string json, out T result)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                result = default!;
                return false;
            }

            result = JsonConvert.DeserializeObject<T>(json) ?? throw new JsonException("Failed to parse JSON");
            return true;
        }
        catch (JsonException)
        {
            result = default!;
            return false;
        }
        catch (Exception e)
        {
            Console.WriteLine($"Unexpected error while parsing JSON: {e.Message}");
            result = default!;
            return false;
        }
    }
}