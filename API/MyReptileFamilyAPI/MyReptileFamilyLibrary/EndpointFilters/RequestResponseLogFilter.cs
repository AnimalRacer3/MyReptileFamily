using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MyReptileFamilyLibrary.Extensions.Internal;

namespace MyReptileFamilyLibrary.EndpointFilters;

/// <summary>
///     Writes HTTP Request and Response information for all calls to the endpoints this filter gets applied to.
///     To use, call "AddRequestResponseLoggingEndpointFilter" instead of "AddEndpointFilter&lt;RequestResponseLogFilter
///     &gt;"
/// </summary>
internal class RequestResponseLogFilter(ILogger<RequestResponseLogFilter> Logger) : IEndpointFilter
{
    internal static bool LogRequestBody { get; set; }
    internal static LogLevel FilterLogLevel { get; set; }

    /// <inheritdoc />
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext Context, EndpointFilterDelegate Next)
    {
        object? _result = await Next(Context);
        Guid _uniqueRequestId = Guid.NewGuid();
        await LogRequestAsync(_uniqueRequestId, Context.HttpContext.Request);
        LogResponse(_uniqueRequestId, Context.HttpContext.Response, _result);
        return _result;
    }

    private async Task LogRequestAsync(Guid UniqueRequestId, HttpRequest Request)
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
        string _body = LogRequestBody ? await Request.GetRequestBodyAsync() : "[Not Logged]";
        Logger.Log(FilterLogLevel, null, requestLogMessage, UniqueRequestId, Request.Method,
            JsonSerializer.Serialize(Request.Headers), _body);
    }

    private void LogResponse(Guid UniqueRequestId, HttpResponse Response, object? Result)
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
        Logger.Log(FilterLogLevel, null, responseLogMessage, UniqueRequestId,
            JsonSerializer.Serialize(Response.Headers), Response.StatusCode,
            Response.ContentType, Result.ToStringFromAPIResult());
    }
}