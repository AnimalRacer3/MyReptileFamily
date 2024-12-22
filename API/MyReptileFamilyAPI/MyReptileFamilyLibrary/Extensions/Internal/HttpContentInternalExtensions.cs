using System.Net.Http.Headers;

namespace MyReptileFamilyLibrary.Extensions.Internal;

internal static class HttpContentInternalExtensions
{
    private static readonly string[] _TextBasedTypes =
    [
        "html",
        "text",
        "xml",
        "json",
        "txt",
        "x-www-form-urlencoded"
    ];

    internal static bool ContentIsTextBased(this HttpRequestMessage _p_Request)
    {
        return _p_Request.Content.ContentIsTextBased();
    }

    internal static bool ContentIsTextBased(this HttpResponseMessage _p_Response)
    {
        return _p_Response.Content.ContentIsTextBased();
    }

    private static bool ContentIsTextBased(this HttpContent? _p_Content)
    {
        if (_p_Content is null) return false;
        return _p_Content is StringContent || ContentIsTextBased(_p_Content.Headers) ||
               ContentIsTextBased(_p_Content.Headers);
    }

    private static bool ContentIsTextBased(this HttpHeaders _p_Headers)
    {
        if (!_p_Headers.TryGetValues("Content-Type", out var _values)) return false;

        var _header = string.Join(" ", _values);
        return _TextBasedTypes.Any(_p_T => _header.Contains(_p_T, StringComparison.InvariantCultureIgnoreCase));
    }
}