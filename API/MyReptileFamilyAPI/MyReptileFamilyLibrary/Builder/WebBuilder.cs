using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using MyReptileFamilyLibrary.EndpointFilters;

namespace MyReptileFamilyLibrary.Builder;

public class WebBuilder() : BuilderBase<WebBuilder, WebApplication>
{
    private readonly WebApplicationBuilder _webApplicationBuilder = WebApplication.CreateBuilder();
    /// <inheritdoc />
    protected override IHostApplicationBuilder Builder => _webApplicationBuilder;
    /// <inheritdoc />
    protected override WebApplication Build() => _webApplicationBuilder.Build();

    protected override void RegisterAutofac(AutofacServiceProviderFactory _p_Factory, Action<ContainerBuilder> _p_Configure)
        => _webApplicationBuilder.Host.UseServiceProviderFactory(_p_Factory).ConfigureContainer(_p_Configure);

    /// <summary>
    /// Creates a new <see cref="WebBuilder"/>, with Serilog logging and dependencies registered
    /// with Autofac
    /// </summary>
    /// <param name="_p_AutofacModulesToRegister">Any custom Autofac modules that should also be registered</param>
    public static WebBuilder Create(params Module[] _p_AutofacModulesToRegister)
        => CreateManually()
            .WithLogging()
            .WithDependenciesRegistered(_p_AutofacModulesToRegister);

    /// <summary>
    /// Only call this if you want to configure logging and DI manually.
    /// IMPORTANT: Unless there's a very specific scenario to cater to, always call <see cref="Create"/> instead!
    /// </summary>
    public static WebBuilder CreateManually() => new();

    /// <inheritdoc cref="BuilderBase{TBuilder,THost}.BuildAndValidate"/>
    /// <param name="_p_LogRequestBody">
    /// Calls <see cref="HttpRequestRewindExtensions.EnableBuffering(HttpRequest)"/> when true,
    /// so that the request/response logger can log the request body.
    /// </param>
    public WebApplication BuildAndValidate(bool _p_LogRequestBody = false)
    {
        WebApplication _app = base.BuildAndValidate();
        RequestResponseLogFilter.LogRequestBody = _p_LogRequestBody;
        if (_p_LogRequestBody)
        {
            _app.Use(_p_Next => _p_Context =>
            {
                _p_Context.Request.EnableBuffering();
                return _p_Next(_p_Context);
            });
        }
        return _app;
    }
}