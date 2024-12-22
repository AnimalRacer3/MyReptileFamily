using MyReptileFamilyLibrary.Builder;
using Microsoft.AspNetCore.Http;

namespace MyReptileFamilyLibrary.Extensions.Internal;

internal static class HttpRequestInternalExtensions
{
    /// <summary>
    /// Gets the body of the request. Can only happen if, in Program.cs, the method <see cref="WebBuilder.BuildAndValidate(bool)" /> is called and 'true'
    /// is passed, because Request Buffering must be enabled. <paramref name="_p_Default"/> returned if the request body is
    /// not accessible or empty.
    /// </summary>
    internal static async Task<string> GetRequestBodyAsync(this HttpRequest _p_Request, string _p_Default = "[No Body]")
    {
        if (!_p_Request.Body.CanSeek)
        {
            return _p_Default;
        }

        _p_Request.Body.Seek(0, SeekOrigin.Begin);
        using StreamReader _inputStream = new(_p_Request.Body);
        string _body = await _inputStream.ReadToEndAsync();
        _p_Request.Body.Seek(0, SeekOrigin.Begin);
        return !string.IsNullOrWhiteSpace(_body) ? Environment.NewLine + _body : _p_Default;
    }
}