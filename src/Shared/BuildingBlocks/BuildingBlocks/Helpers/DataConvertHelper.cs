using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using JsonException = Newtonsoft.Json.JsonException;

namespace BuildingBlocks.Helpers;

public static class DataConvertHelper
{
    public static bool TryParseDateTime(string value, out DateTime result)
    {
        return DateTime.TryParse(value, out result);
    }

    public static bool TryParseDateTimeYear(string value, out DateTime result)
    {
        return DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out result);
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

            result = JsonConvert.DeserializeObject<T>(json, new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                }
            }) ?? throw new JsonException("Failed to parse JSON");
            
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


