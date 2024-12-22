using Microsoft.Extensions.Logging;
using System.Diagnostics;
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
public class HttpLoggingHandler(ILogger<HttpLoggingHandler> _p_Logger) : DelegatingHandler
{
    private static readonly Stopwatch _Stopwatch = new();
    private readonly ILogger<HttpLoggingHandler> _Logger = _p_Logger;

    /// <inheritdoc />
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage _p_Request,
        CancellationToken _p_CancellationToken)
    {
        var _id = Guid.NewGuid();
        _Logger.LogDebug("[{Id}] Request: {Request}", _id, _p_Request.ToStringWithoutHeaders());
        if (_p_Request.ContentIsTextBased())
        {
            var _requestContent = await _p_Request.Content!.ReadAsStringAsync(_p_CancellationToken);
            _Logger.LogDebug("[{Id}] Request Content: {RequestContent}", _id, _requestContent.TryFormatJson());
        }

        _Stopwatch.Restart();
        var _response = await base.SendAsync(_p_Request, _p_CancellationToken).ConfigureAwait(false);
        _Stopwatch.Stop();
        _Logger.LogDebug("[{Id}] Response [{RequestDuration}]: {Response}", _id, _Stopwatch.Elapsed, _response);
        if (_response.ContentIsTextBased())
        {
            _Stopwatch.Restart();
            var _responseContent = await _response.Content.ReadAsStringAsync(_p_CancellationToken);
            _Stopwatch.Stop();
            _Logger.LogDebug("[{Id}] Response Content [{ResponseReadDuration}]: {ResponseContent}", _id,
                _Stopwatch.Elapsed, _responseContent.TryFormatJson());
        }

        return _response;
    }
}