using Microsoft.Extensions.DependencyInjection;

namespace MyReptileFamilyLibrary.Extensions.Internal;

internal static class HttpClientBuilderInternalExtensions
{
    /// <summary>
    ///     If you want redirects to not happen for your Refit client, this method will
    ///     try to set <see cref="HttpClientHandler.AllowAutoRedirect" /> to false.
    /// </summary>
    internal static IHttpClientBuilder DisableAutoRedirect(this IHttpClientBuilder _p_HttpClientBuilder)
    {
        return _p_HttpClientBuilder.ConfigurePrimaryHttpMessageHandler((_p_Handler, _) =>
        {
            switch (_p_Handler)
            {
                // When getting bearer tokens, the actual client handler is one layer deeper
                case DelegatingHandler { InnerHandler: HttpClientHandler _inner }:
                    _inner.AllowAutoRedirect = false;
                    break;
                case HttpClientHandler _clientHandler:
                    _clientHandler.AllowAutoRedirect = false;
                    break;
            }
        });
    }
}