using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyReptileFamilyLibrary.Extensions.Internal;

internal static class LoggingExtensions
{
    public static string ToStringFromAPIResult<T>(this T Response, string DefaultResponse = "<No Result>")
    {
        return Response switch
        {
            IValueHttpResult<object> { Value: ProblemDetails problem } => problem.Detail ??
                                                                          problem.ToString() ?? DefaultResponse,
            IValueHttpResult<object> valueResult => valueResult.Value?.ToString() ?? DefaultResponse,
            _ => Response?.ToString() ?? DefaultResponse
        };
    }
}