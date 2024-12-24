using System.Net.Http.Headers;

namespace MyReptileFamilyLibrary.Extensions.Internal;

internal static class HttpContentInternalExtensions
{
    private static readonly string[] _textBasedTypes =
    [
        "html",
        "text",
        "xml",
        "json",
        "txt",
        "x-www-form-urlencoded"
    ];

    internal static bool ContentIsTextBased(this HttpRequestMessage Request)
    {
        return Request.Content.ContentIsTextBased();
    }

    internal static bool ContentIsTextBased(this HttpResponseMessage Response)
    {
        return Response.Content.ContentIsTextBased();
    }

    private static bool ContentIsTextBased(this HttpContent? Content)
    {
        if (Content is null) return false;
        return Content is StringContent || ContentIsTextBased(Content.Headers) ||
               ContentIsTextBased(Content.Headers);
    }

    private static bool ContentIsTextBased(this HttpHeaders Headers)
    {
        if (!Headers.TryGetValues("Content-Type", out IEnumerable<string>? _values)) return false;

        string _header = string.Join(" ", _values);
        return _textBasedTypes.Any(T => _header.Contains(T, StringComparison.InvariantCultureIgnoreCase));
    }
}