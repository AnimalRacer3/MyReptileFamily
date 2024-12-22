using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyReptileFamilyLibrary.Extensions.Internal;

namespace MyReptileFamilyLibrary.EndpointFilters;

/// <summary>
/// Writes HTTP Request and Response information for all calls to the endpoints this filter gets applied to.
/// To use, call "AddRequestResponseLoggingEndpointFilter" instead of "AddEndpointFilter&lt;RequestResponseLogFilter&gt;"
/// </summary>
internal class RequestResponseLogFilter(ILogger<RequestResponseLogFilter> _p_Logger) : IEndpointFilter
{
    internal static bool LogRequestBody { get; set; }
    internal static LogLevel FilterLogLevel { get; set; }

    /// <inheritdoc />
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext _p_Context, EndpointFilterDelegate _p_Next)
    {
        var _result = await _p_Next(_p_Context);
        Guid _uniqueRequestId = Guid.NewGuid();
        await LogRequestAsync(_uniqueRequestId, _p_Context.HttpContext.Request);
        LogResponse(_uniqueRequestId, _p_Context.HttpContext.Response, _result);
        return _result;
    }

    private async Task LogRequestAsync(Guid _p_UniqueRequestId, HttpRequest _p_Request)
    {
        const string requestLogMessage =
            """

            =============================================================
            HTTP Request {UniqueRequestId}
            Method: {RequestMethod}
            Headers: {RequestHeaders}
            Body: {Body}
            =============================================================
            """;
        string _body = LogRequestBody ? await _p_Request.GetRequestBodyAsync() : "[Not Logged]";
        _p_Logger.Log(FilterLogLevel, null, requestLogMessage, _p_UniqueRequestId, _p_Request.Method, JsonSerializer.Serialize(_p_Request.Headers), _body);
    }

    private void LogResponse(Guid _p_UniqueRequestId, HttpResponse _p_Response, object? _p_Result)
    {
        const string responseLogMessage =
            """

            =============================================================
            HTTP Response {UniqueRequestId}
            Headers: {ResponseHeaders}
            StatusCode: {ResponseStatusCode}
            ContentType: {ResponseContentType}
            Result: {ResponseResult}
            =============================================================
            """;
        _p_Logger.Log(FilterLogLevel, null, responseLogMessage, _p_UniqueRequestId, JsonSerializer.Serialize(_p_Response.Headers), _p_Response.StatusCode,
            _p_Response.ContentType, _p_Result.ToStringFromAPIResult());
    }
}