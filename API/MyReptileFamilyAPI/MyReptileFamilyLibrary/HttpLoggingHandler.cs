using System.Diagnostics;
using Microsoft.Extensions.Logging;
using MyReptileFamilyLibrary.Extensions;
using MyReptileFamilyLibrary.Extensions.Internal;

namespace MyReptileFamilyLibrary;

/// <summary>
///     Logs and times HTTP requests/responses, including all content (as <see cref="LogLevel.Debug" />);
///     each HTTP request is logged alongside a unique ID for easier matching/filtering
/// </summary>
/// <remarks>
///     CAUTION: Services with heavy HTTP traffic that log <see cref="LogLevel.Debug" />-level messages can potentially
///     log a lot of information!
/// </remarks>
/// <inheritdoc />
public class HttpLoggingHandler(ILogger<HttpLoggingHandler> Logger) : DelegatingHandler
{
    private static readonly Stopwatch _Stopwatch = new();
    private readonly ILogger<HttpLoggingHandler> _Logger = Logger;

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage Request,
        CancellationToken CancellationToken)
    {
        Guid _id = Guid.NewGuid();
        _Logger.LogDebug("[{Id}] Request: {Request}", _id, Request.ToStringWithoutHeaders());
        if (Request.ContentIsTextBased())
        {
            string _requestContent = await Request.Content!.ReadAsStringAsync(CancellationToken);
            _Logger.LogDebug("[{Id}] Request Content: {RequestContent}", _id, _requestContent.TryFormatJson());
        }

        _Stopwatch.Restart();
        HttpResponseMessage _response = await base.SendAsync(Request, CancellationToken).ConfigureAwait(false);
        _Stopwatch.Stop();
        _Logger.LogDebug("[{Id}] Response [{RequestDuration}]: {Response}", _id, _Stopwatch.Elapsed, _response);
        if (_response.ContentIsTextBased())
        {
            _Stopwatch.Restart();
            string _responseContent = await _response.Content.ReadAsStringAsync(CancellationToken);
            _Stopwatch.Stop();
            _Logger.LogDebug("[{Id}] Response Content [{ResponseReadDuration}]: {ResponseContent}", _id,
                _Stopwatch.Elapsed, _responseContent.TryFormatJson());
        }

        return _response;
    }
}