namespace MyReptileFamilyLibrary.Extensions.Internal;

/// <summary>
///     Extensions for <see cref="HttpRequestMessage" />
/// </summary>
internal static class HttpRequestMessageInternalExtensions
{
    /// <summary>
    ///     Removes headers from the string that <see cref="HttpRequestMessage.ToString" /> returns
    /// </summary>
    /// <remarks>Used as a precaution in case client secrets or other sensitive data are in the headers</remarks>
    internal static string ToStringWithoutHeaders(this HttpRequestMessage _p_Request)
    {
        const string headersKey = ", Headers:"; // From HttpRequestMessage.ToString implementation
        var _original = _p_Request.ToString();
        return _original.Split(headersKey).FirstOrDefault() ?? string.Empty;
    }
}