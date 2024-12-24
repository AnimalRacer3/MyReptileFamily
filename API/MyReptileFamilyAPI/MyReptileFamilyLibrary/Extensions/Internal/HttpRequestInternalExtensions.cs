using Microsoft.AspNetCore.Http;
using MyReptileFamilyLibrary.Builder;

namespace MyReptileFamilyLibrary.Extensions.Internal;

internal static class HttpRequestInternalExtensions
{
    /// <summary>
    ///     Gets the body of the request. Can only happen if, in Program.cs, the method
    ///     <see cref="WebBuilder.BuildAndValidate(bool)" /> is called and 'true'
    ///     is passed, because Request Buffering must be enabled. <paramref name="Default" /> returned if the request body is
    ///     not accessible or empty.
    /// </summary>
    internal static async Task<string> GetRequestBodyAsync(this HttpRequest Request, string Default = "[No Body]")
    {
        if (!Request.Body.CanSeek) return Default;

        Request.Body.Seek(0, SeekOrigin.Begin);
        using StreamReader _inputStream = new(Request.Body);
        string _body = await _inputStream.ReadToEndAsync();
        Request.Body.Seek(0, SeekOrigin.Begin);
        return !string.IsNullOrWhiteSpace(_body) ? Environment.NewLine + _body : Default;
    }
}