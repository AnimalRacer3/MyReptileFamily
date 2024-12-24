using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using MyReptileFamilyLibrary.EndpointFilters;

namespace MyReptileFamilyLibrary.Builder;

public class WebBuilder : BuilderBase<WebBuilder, WebApplication>
{
    private readonly WebApplicationBuilder _webApplicationBuilder = WebApplication.CreateBuilder();

    /// <inheritdoc />
    protected override IHostApplicationBuilder Builder => _webApplicationBuilder;

    /// <inheritdoc />
    protected override WebApplication Build()
    {
        return _webApplicationBuilder.Build();
    }

    protected override void RegisterAutofac(AutofacServiceProviderFactory Factory, Action<ContainerBuilder> Configure)
    {
        _webApplicationBuilder.Host.UseServiceProviderFactory(Factory).ConfigureContainer(Configure);
    }

    /// <summary>
    ///     Creates a new <see cref="WebBuilder" />, with Serilog logging and dependencies registered
    ///     with Autofac
    /// </summary>
    /// <param name="AutofacModulesToRegister">Any custom Autofac modules that should also be registered</param>
    public static WebBuilder Create(params Module[] AutofacModulesToRegister)
    {
        return CreateManually()
            .WithLogging()
            .WithDependenciesRegistered(AutofacModulesToRegister);
    }

    /// <summary>
    ///     Only call this if you want to configure logging and DI manually.
    ///     IMPORTANT: Unless there's a very specific scenario to cater to, always call <see cref="Create" /> instead!
    /// </summary>
    public static WebBuilder CreateManually()
    {
        return new WebBuilder();
    }

    /// <inheritdoc cref="BuilderBase{TBuilder,THost}.BuildAndValidate" />
    /// <param name="LogRequestBody">
    ///     Calls <see cref="HttpRequestRewindExtensions.EnableBuffering(HttpRequest)" /> when true,
    ///     so that the request/response logger can log the request body.
    /// </param>
    public WebApplication BuildAndValidate(bool LogRequestBody = false)
    {
        WebApplication _app = base.BuildAndValidate();
        RequestResponseLogFilter.LogRequestBody = LogRequestBody;
        if (LogRequestBody)
            _app.Use(Next => Context =>
            {
                Context.Request.EnableBuffering();
                return Next(Context);
            });
        return _app;
    }
}