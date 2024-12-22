using System.Text.Json;

namespace MyReptileFamilyLibrary.Extensions;

public static class StringExtensions
{
    /// <summary>
    ///     If the provided <paramref name="_p_Json" /> is valid JSON, will return it in a readable format; else will return
    ///     whatever the original string value was
    /// </summary>
    public static string? TryFormatJson(this string? _p_Json)
    {
        if (string.IsNullOrWhiteSpace(_p_Json)) return _p_Json;
        try
        {
            dynamic? _deserialized = JsonSerializer.Deserialize<dynamic>(_p_Json);
            if (_deserialized is null) return _p_Json;
            return JsonSerializer.Serialize(_deserialized, new JsonSerializerOptions
            {
                WriteIndented = true
            });
        }
        catch (JsonException)
        {
            return _p_Json;
        }
    }
}