using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyReptileFamilyLibrary.Abstractions;
using MyReptileFamilyLibrary.AppSettings;
using MyReptileFamilyLibrary.Extensions;
using MyReptileFamilyLibrary.Extensions.Internal;
using MyReptileFamilyLibrary.Factories;
using MyReptileFamilyLibrary.Services;
using Refit;
using SendGrid.Extensions.DependencyInjection;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Module = Autofac.Module;

namespace MyReptileFamilyLibrary.Builder;

/// <summary>
/// A base class suited for setting up builders of various kinds of applications
/// </summary>
/// <typeparam name="TBuilder">
/// The derived class; used so that the various "With" methods can return itself, and return the derived class type
/// </typeparam>
/// <typeparam name="THost">
/// The kind of <see cref="IHost"/> that is created upon building
/// </typeparam>
public abstract class BuilderBase<TBuilder, THost>() 
    where TBuilder : BuilderBase<TBuilder, THost>
    where THost : IHost
{
    /// <summary>
    /// Provide the instance of the builder class being used
    /// </summary>
    protected abstract IHostApplicationBuilder Builder { get; }

    /// <summary>
    /// Because <see cref="IHostApplicationBuilder"/> does not expose a "Build" method, call it by overriding this
    /// </summary>
    protected abstract THost Build();

    /// <summary>
    /// How dependencies are registered varies by builder. Provide the registration implementation here for Autofac.
    /// </summary>
    /// <param name="_p_Factory">The provider factory to assign</param>
    /// <param name="_p_Configure">When configuring <see cref="ContainerBuilder"/>, pass this in</param>
    protected abstract void RegisterAutofac(AutofacServiceProviderFactory _p_Factory, Action<ContainerBuilder> _p_Configure);

    private bool _EmailServiceActivated;
    private readonly List<Type> _SettingsTypes = new();
    private Func<IHost, CancellationToken, Task<string>>? _GetBearerTokenAsyncFunc;

    /// <summary>
    /// Creates an <see cref="IHost" /> and performs validation on configured settings (DataAnnotations, connection strings)
    /// and the configured logger (make sure it can write to file if configured to do so)
    /// </summary>
    /// <returns>An <see cref="IHost"/> that can be run</returns>
    /// <exception cref="ApplicationException">Thrown when a validation error occurs</exception>
    public THost BuildAndValidate()
    {
        THost _host = Build();
        if (_GetBearerTokenAsyncFunc is not null)
        {
            AuthBearerTokenFactory.SetBearerTokenGetterFunc(_p_Token => _GetBearerTokenAsyncFunc(_host, _p_Token));
        }

        HostValidator.Validate(_host, _SettingsTypes, ValidateBuild);
        return _host;
    }

    /// <summary>
    /// Method invoked during validation. Can be overridden by builder implementations
    /// to do builder-specific validation
    /// </summary>
    /// <param name="_p_Host">Host instance to get services and other data from</param>
    /// <param name="_p_Logger">Instance of the logger that can be used for logging</param>
    /// <returns>true when valid, false otherwise</returns>
    protected virtual bool ValidateBuild(IHost _p_Host, ILogger _p_Logger) => true;

    /// <summary>
    /// Registers dependencies for this library as well as for the calling service's
    /// </summary>
    /// <param name="_p_AutofacModulesToRegister">Any additional Autofac modules that should also be registered</param>
    public TBuilder WithDependenciesRegistered(Module[] _p_AutofacModulesToRegister)
    {
        Builder.ConfigureContainer<ContainerBuilder>(new AutofacServiceProviderFactory(),
            _p_ContainerBuilder =>
            {
                _p_ContainerBuilder.RegisterModule<MRFAutofacModule>();
                foreach (var _autofacModule in _p_AutofacModulesToRegister)
                {
                    _p_ContainerBuilder.RegisterModule(_autofacModule);
                }
            });
        return (TBuilder)this;
    }

    /// <summary>
    /// Enables use of <see cref="IEmailService"/>, using SendGrid for e-mail
    /// </summary>
    /// <param name="_p_EmailSettings">Settings that provide <see cref="ISendGridSettings.SendGridApiKey"/></param>
    /// <exception cref="ApplicationException">
    /// Thrown when <see cref="ISendGridSettings.SendGridApiKey"/> is not set or if an e-mail service has already been added
    /// </exception>
    public TBuilder WithEmailService(ISendGridSettings _p_EmailSettings)
    {
        if (_EmailServiceActivated)
        {
            throw new ApplicationException("An e-mail service has already been added");
        }
        _EmailServiceActivated = true;
        if (_p_EmailSettings.EmailEnabled && string.IsNullOrWhiteSpace(_p_EmailSettings.SendGridApiKey))
        {
            throw new ApplicationException($"{nameof(ISendGridSettings.SendGridApiKey)} required. Provide it when calling {nameof(WithEmailService)}.");
        }
        if (!_p_EmailSettings.EmailEnabled) _p_EmailSettings.SendGridApiKey = "blank-key";

        Builder.Services.AddSingleton(_p_EmailSettings);
        Builder.Services.AddSendGrid(_p_Options => _p_Options.ApiKey = _p_EmailSettings.SendGridApiKey);
        Builder.Services.AddTransient<IEmailService, SendGridEmailService>();
        return (TBuilder)this;
    }

    /// <summary>
    /// Adds Serilog logging, configured from file
    /// </summary>
    public TBuilder WithLogging()
    {
        Builder.Configuration.AddLoggingConfig(Builder.Environment);
        Builder.Services.AddSerilog(_p_LoggerConfiguration =>
        {
            // consider replacing this
            // need to get Datadog host & service to automatically populate
            _p_LoggerConfiguration
                .ReadFrom.Configuration(Builder.Configuration)
                .Enrich.WithProperty("service", Builder.Environment.ApplicationName);
        });
        return (TBuilder)this;
    }

    /// <summary>
    /// Configures <typeparamref name="TClientInterface"/> as a Refit client, setting other conventional defaults.
    /// </summary>
    /// <param name="_p_ConfigureHttpClient">Action to take when configuring the <see cref="HttpClient"/></param>
    /// <param name="_p_GetBearerTokenAsyncFunc">When provided, will configure Refit to attempt to fetch a bearer token from <see cref="AuthBearerTokenFactory"/></param>
    /// <param name="_p_EnableRequestResponseLogging">When true, will log every single HTTP request and response using <see cref="HttpLoggingHandler"/></param>
    /// <param name="_p_DisableAutoRedirect">
    /// When true, will attempt to disable the ability for this client to do redirects when making HTTP calls.
    /// Typically, one might do this to prevent redirects to login pages when trying to use an API.
    /// </param>
    /// <param name="_p_HandlerLifetimeInMinutes">
    /// How long the handler should live for before it is renewed. In long-running applications and services,
    /// set this to a low value because they're meant to be short-lived. Default is 10 minutes.
    /// </param>
    /// <param name="_p_HttpContentSerializer">
    /// Provide a customer serializer for Refit, if you want to configure, or use something different,
    /// than the default <see cref="SystemTextJsonContentSerializer"/> instance
    /// </param>
    /// <typeparam name="TClientInterface">A Refit client interface.</typeparam>
    /// <returns>The builder used to build this client, to allow for further customization if needed.</returns>
    public TBuilder WithRefitClient<TClientInterface>(Action<HttpClient> _p_ConfigureHttpClient,
        Func<IHost, CancellationToken, Task<string>>? _p_GetBearerTokenAsyncFunc = null,
        bool _p_EnableRequestResponseLogging = false,
        bool _p_DisableAutoRedirect = false, int _p_HandlerLifetimeInMinutes = 10,
        IHttpContentSerializer? _p_HttpContentSerializer = null)
        where TClientInterface : class
    {
        _GetBearerTokenAsyncFunc = _p_GetBearerTokenAsyncFunc;
        var _refitSettings = GetRefitSettings(_p_HttpContentSerializer);
        var _builder = Builder.Services.AddRefitClient<TClientInterface>(_refitSettings);
        if (_p_DisableAutoRedirect) _builder = _builder.DisableAutoRedirect();
        _builder
            .ConfigureHttpClient(_p_ConfigureHttpClient)
            .SetHandlerLifetime(TimeSpan.FromMinutes(_p_HandlerLifetimeInMinutes));
        if (!_p_EnableRequestResponseLogging)
            return (TBuilder)this;

        // Cannot use builder.AddHttpMessageHandler<HttpLoggingHandler>, because Refit and DI mingling throws an exception.
        // See https://github.com/reactiveui/refit/issues/1403#issuecomment-1499380557 for workaround source
        _builder.AddHttpMessageHandler(_p_Svc =>
            new HttpLoggingHandler(_p_Svc.GetRequiredService<ILogger<HttpLoggingHandler>>()));
        _builder.Services.AddSingleton<HttpLoggingHandler>(); // Calling this multiple times seems to be fine.

        return (TBuilder)this;
    }

    /// <summary>
    /// Configures settings for a particular type, setting up DataAnnotations validation
    /// for them and making them available throughout the application by injecting either
    /// IOptions&lt;<typeparamref name="TSettings"/>&gt; or <typeparamref name="TSettings"/> into a constructor
    /// </summary>
    /// <returns>
    /// A <typeparamref name="TSettings"/> instance hydrated with settings from configuration.
    /// These are not validated, but are necessary for use when configuring the service on startup.
    /// </returns>
    public TSettings WithSettings<TSettings>()
        where TSettings : class
    {
        Builder.Services.AddAndValidateSettings<TSettings>();
        _SettingsTypes.Add(typeof(TSettings));
        TSettings _settings = Builder.Configuration.GetRequiredSettings<TSettings>();
        if (_settings is IMySQLConnectionString _sqlConnectionString)
        {
            Builder.Services.AddSingleton(_sqlConnectionString);
        }
        Builder.Services.AddSingleton(_p_Provider => _p_Provider.GetService<IOptions<TSettings>>()!.Value);
        return _settings;
    }

    /// <summary>
    /// Exposes <see cref="IHostApplicationBuilder.Services"/> Configure method to allow
    /// registration of an action used to configure a particular type of options.
    /// </summary>
    public TBuilder Configure<TOptions>(Action<TOptions> _p_ConfigureAction)
        where TOptions : class
    {
        Builder.Services.Configure(_p_ConfigureAction);
        return (TBuilder)this;
    }

    private RefitSettings? GetRefitSettings(IHttpContentSerializer? _p_HttpContentSerializer)
    {
        var _useAuthHeaderGetter = _GetBearerTokenAsyncFunc is not null;
        if (!_useAuthHeaderGetter && _p_HttpContentSerializer is null) return null;
        var _settings = new RefitSettings();
        if (_useAuthHeaderGetter)
            _settings.AuthorizationHeaderValueGetter = (_, _p_CancellationToken) =>
                AuthBearerTokenFactory.GetBearerTokenAsync(_p_CancellationToken);
        if (_p_HttpContentSerializer is not null) _settings.ContentSerializer = _p_HttpContentSerializer;
        return _settings;
    }
}