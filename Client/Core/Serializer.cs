using System.Text.Json;

namespace Client.Core;

internal static class Serializer
{
    private static JsonSerializerOptions _options = new JsonSerializerOptions {
        PropertyNameCaseInsensitive = true
    };

    internal static string Serialize<T> (T obj)
    {
        return JsonSerializer.Serialize(obj, _options);
    }

    internal static T Deserialize<T> (string json)
    {
        return JsonSerializer.Deserialize<T>(json, _options);
    }
}