using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyReptileFamilyLibrary.Extensions.Internal;

internal static class LoggingExtensions
{
    public static string ToStringFromAPIResult<T>(this T _p_Response, string _p_DefaultResponse = "<No Result>") => _p_Response switch
    {
        IValueHttpResult<object> { Value: ProblemDetails _problem } => _problem.Detail ?? _problem.ToString() ?? _p_DefaultResponse,
        IValueHttpResult<object> _valueResult => _valueResult.Value?.ToString() ?? _p_DefaultResponse,
        _ => _p_Response?.ToString() ?? _p_DefaultResponse
    };
}
