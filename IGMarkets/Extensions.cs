using System.Text.Json;

public static class Extensions
{
    public static string JsonPrettify(this string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return string.Empty;
        }
        using var jDoc = JsonDocument.Parse(json);
        return JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true });
    }
}