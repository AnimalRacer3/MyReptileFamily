﻿using MyReptileFamilyLibrary.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Refit;

namespace MyReptileFamilyLibrary.Factories;

/// <summary>
///     Static class that returns bearer tokens. Used when calling
///     <see cref="BuilderBase{TBuilder,THost}.WithRefitClient{TClientInterface}" />
///     when setting <see cref="RefitSettings.AuthorizationHeaderValueGetter" /> (i.e. passing "useAuthHeaderGetter" as
///     true).
///     After building the <see cref="IHost" />, call <see cref="SetBearerTokenGetterFunc" /> so this factory knows how to
///     get
///     the bearer token. If that doesn't happen, then <see cref="InvalidOperationException" /> will be thrown.
/// </summary>
public static class AuthBearerTokenFactory
{
    private static Func<CancellationToken, Task<string>>? _getBearerTokenAsyncFunc;

    /// <summary>
    ///     Returns bearer token string to be added to Authorization headers.
    ///     Use the returned token like so: "Authorization: Bearer {token}"
    /// </summary>
    /// <returns>Token string to use in Authorization Bearer header</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when <see cref="SetBearerTokenGetterFunc" /> not called prior to calling this method
    /// </exception>
    public static Task<string> GetBearerTokenAsync(CancellationToken _p_CancellationToken)
    {
        VerifyBearerTokenGetterFuncIsSet();
        return _getBearerTokenAsyncFunc!(_p_CancellationToken);
    }

    /// <summary>
    ///     Checks if the delegate for fetching bearer tokens is set.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    ///     Thrown when <see cref="SetBearerTokenGetterFunc" /> not called prior to calling this method
    /// </exception>
    public static void VerifyBearerTokenGetterFuncIsSet(ILogger? _p_Logger = null)
    {
        if (_getBearerTokenAsyncFunc is not null) return;
        try
        {
            const string msg =
                $"Cannot call {nameof(AuthBearerTokenFactory)}.{nameof(GetBearerTokenAsync)} without calling {nameof(AuthBearerTokenFactory)}.{nameof(SetBearerTokenGetterFunc)} first";
            throw new InvalidOperationException(msg);
        }
        catch (Exception _ex)
        {
            // Logger has to be invoked manually due to where this is called in the application lifetime
            _p_Logger?.LogError(_ex,
                "Auth Bearer Token Factory attempted to be used before its getter function was set");
            throw;
        }
    }

    /// <summary>
    ///     Provide a delegate that returns a bearer token to use for authorization
    /// </summary>
    public static void SetBearerTokenGetterFunc(Func<CancellationToken, Task<string>> _p_GetBearerTokenAsyncFunc)
    {
        _getBearerTokenAsyncFunc = _p_GetBearerTokenAsyncFunc;
    }
}