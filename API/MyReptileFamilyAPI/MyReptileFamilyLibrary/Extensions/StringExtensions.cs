using System.Text.Json;

namespace MyReptileFamilyLibrary.Extensions;

public static class StringExtensions
{
    /// <summary>
    ///     If the provided <paramref name="Json" /> is valid JSON, will return it in a readable format; else will return
    ///     whatever the original string value was
    /// </summary>
    public static string? TryFormatJson(this string? Json)
    {
        if (string.IsNullOrWhiteSpace(Json)) return Json;
        try
        {
            dynamic? _deserialized = JsonSerializer.Deserialize<dynamic>(Json);
            if (_deserialized is null) return Json;
            return JsonSerializer.Serialize(_deserialized, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
        catch (JsonException)
        {
            return Json;
        }
    }
}