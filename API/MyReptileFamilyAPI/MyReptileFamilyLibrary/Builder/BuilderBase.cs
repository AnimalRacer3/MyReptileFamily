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
///     A base class suited for setting up builders of various kinds of applications
/// </summary>
/// <typeparam name="TBuilder">
///     The derived class; used so that the various "With" methods can return itself, and return the derived class type
/// </typeparam>
/// <typeparam name="THost">
///     The kind of <see cref="IHost" /> that is created upon building
/// </typeparam>
public abstract class BuilderBase<TBuilder, THost> where TBuilder : BuilderBase<TBuilder, THost>
    where THost : IHost
{
    private readonly List<Type> _settingsTypes = [];

    private bool _emailServiceActivated;
    private Func<IHost, CancellationToken, Task<string>>? _getBearerTokenAsyncFunc;

    /// <summary>
    ///     Provide the instance of the builder class being used
    /// </summary>
    protected abstract IHostApplicationBuilder Builder { get; }

    /// <summary>
    ///     Because <see cref="IHostApplicationBuilder" /> does not expose a "Build" method, call it by overriding this
    /// </summary>
    protected abstract THost Build();

    /// <summary>
    ///     How dependencies are registered varies by builder. Provide the registration implementation here for Autofac.
    /// </summary>
    /// <param name="Factory">The provider factory to assign</param>
    /// <param name="Configure">When configuring <see cref="ContainerBuilder" />, pass this in</param>
    protected abstract void RegisterAutofac(AutofacServiceProviderFactory Factory, Action<ContainerBuilder> Configure);

    /// <summary>
    ///     Creates an <see cref="IHost" /> and performs validation on configured settings (DataAnnotations, connection
    ///     strings)
    ///     and the configured logger (make sure it can write to file if configured to do so)
    /// </summary>
    /// <returns>An <see cref="IHost" /> that can be run</returns>
    /// <exception cref="ApplicationException">Thrown when a validation error occurs</exception>
    public THost BuildAndValidate()
    {
        THost _host = Build();
        if (_getBearerTokenAsyncFunc is not null)
            AuthBearerTokenFactory.SetBearerTokenGetterFunc(Token => _getBearerTokenAsyncFunc(_host, Token));

        HostValidator.Validate(_host, _settingsTypes, ValidateBuild);
        return _host;
    }

    /// <summary>
    ///     Method invoked during validation. Can be overridden by builder implementations
    ///     to do builder-specific validation
    /// </summary>
    /// <param name="Host">Host instance to get services and other data from</param>
    /// <param name="Logger">Instance of the logger that can be used for logging</param>
    /// <returns>true when valid, false otherwise</returns>
    protected virtual bool ValidateBuild(IHost Host, ILogger Logger)
    {
        return true;
    }

    /// <summary>
    ///     Registers dependencies for this library as well as for the calling service's
    /// </summary>
    /// <param name="AutofacModulesToRegister">Any additional Autofac modules that should also be registered</param>
    public TBuilder WithDependenciesRegistered(Module[] AutofacModulesToRegister)
    {
        Builder.ConfigureContainer(new AutofacServiceProviderFactory(),
            ContainerBuilder =>
            {
                ContainerBuilder.RegisterModule<MRFAutofacModule>();
                foreach (Module _autofacModule in AutofacModulesToRegister)
                    ContainerBuilder.RegisterModule(_autofacModule);
            });
        return (TBuilder)this;
    }

    /// <summary>
    ///     Enables use of <see cref="IEmailService" />, using SendGrid for e-mail
    /// </summary>
    /// <param name="EmailSettings">Settings that provide <see cref="ISendGridSettings.SendGridApiKey" /></param>
    /// <exception cref="ApplicationException">
    ///     Thrown when <see cref="ISendGridSettings.SendGridApiKey" /> is not set or if an e-mail service has already been
    ///     added
    /// </exception>
    public TBuilder WithEmailService(ISendGridSettings EmailSettings)
    {
        if (_emailServiceActivated) throw new ApplicationException("An e-mail service has already been added");
        _emailServiceActivated = true;
        EmailSettings.SendGridApiKey = EmailSettings.EmailEnabled switch
        {
            true when string.IsNullOrWhiteSpace(EmailSettings.SendGridApiKey) => throw new ApplicationException(
                $"{nameof(ISendGridSettings.SendGridApiKey)} required. Provide it when calling {nameof(WithEmailService)}."),
            false => "blank-key",
            _ => EmailSettings.SendGridApiKey
        };

        Builder.Services.AddSingleton(EmailSettings);
        Builder.Services.AddSendGrid(Options => Options.ApiKey = EmailSettings.SendGridApiKey);
        Builder.Services.AddTransient<IEmailService, SendGridEmailService>();
        return (TBuilder)this;
    }

    /// <summary>
    ///     Adds Serilog logging, configured from file
    /// </summary>
    public TBuilder WithLogging()
    {
        Builder.Configuration.AddLoggingConfig(Builder.Environment);
        Builder.Services.AddSerilog(LoggerConfiguration =>
        {
            // consider replacing this
            // need to get Datadog host & service to automatically populate
            LoggerConfiguration
                .ReadFrom.Configuration(Builder.Configuration)
                .Enrich.WithProperty("service", Builder.Environment.ApplicationName);
        });
        return (TBuilder)this;
    }

    /// <summary>
    ///     Configures <typeparamref name="TClientInterface" /> as a Refit client, setting other conventional defaults.
    /// </summary>
    /// <param name="ConfigureHttpClient">Action to take when configuring the <see cref="HttpClient" /></param>
    /// <param name="GetBearerTokenAsyncFunc">
    ///     When provided, will configure Refit to attempt to fetch a bearer token from
    ///     <see cref="AuthBearerTokenFactory" />
    /// </param>
    /// <param name="EnableRequestResponseLogging">
    ///     When true, will log every single HTTP request and response using
    ///     <see cref="HttpLoggingHandler" />
    /// </param>
    /// <param name="DisableAutoRedirect">
    ///     When true, will attempt to disable the ability for this client to do redirects when making HTTP calls.
    ///     Typically, one might do this to prevent redirects to login pages when trying to use an API.
    /// </param>
    /// <param name="HandlerLifetimeInMinutes">
    ///     How long the handler should live for before it is renewed. In long-running applications and services,
    ///     set this to a low value because they're meant to be short-lived. Default is 10 minutes.
    /// </param>
    /// <param name="HttpContentSerializer">
    ///     Provide a customer serializer for Refit, if you want to configure, or use something different,
    ///     than the default <see cref="SystemTextJsonContentSerializer" /> instance
    /// </param>
    /// <typeparam name="TClientInterface">A Refit client interface.</typeparam>
    /// <returns>The builder used to build this client, to allow for further customization if needed.</returns>
    public TBuilder WithRefitClient<TClientInterface>(Action<HttpClient> ConfigureHttpClient,
        Func<IHost, CancellationToken, Task<string>>? GetBearerTokenAsyncFunc = null,
        bool EnableRequestResponseLogging = false,
        bool DisableAutoRedirect = false, int HandlerLifetimeInMinutes = 10,
        IHttpContentSerializer? HttpContentSerializer = null)
        where TClientInterface : class
    {
        _getBearerTokenAsyncFunc = GetBearerTokenAsyncFunc;
        RefitSettings? _refitSettings = GetRefitSettings(HttpContentSerializer);
        IHttpClientBuilder _builder = Builder.Services.AddRefitClient<TClientInterface>(_refitSettings);
        if (DisableAutoRedirect) _builder = _builder.DisableAutoRedirect();
        _builder
            .ConfigureHttpClient(ConfigureHttpClient)
            .SetHandlerLifetime(TimeSpan.FromMinutes(HandlerLifetimeInMinutes));
        if (!EnableRequestResponseLogging)
            return (TBuilder)this;

        // Cannot use builder.AddHttpMessageHandler<HttpLoggingHandler>, because Refit and DI mingling throws an exception.
        // See https://github.com/reactiveui/refit/issues/1403#issuecomment-1499380557 for workaround source
        _builder.AddHttpMessageHandler(Svc =>
            new HttpLoggingHandler(Svc.GetRequiredService<ILogger<HttpLoggingHandler>>()));
        _builder.Services.AddSingleton<HttpLoggingHandler>(); // Calling this multiple times seems to be fine.

        return (TBuilder)this;
    }

    /// <summary>
    ///     Configures settings for a particular type, setting up DataAnnotations validation
    ///     for them and making them available throughout the application by injecting either
    ///     IOptions&lt;<typeparamref name="TSettings" />&gt; or <typeparamref name="TSettings" /> into a constructor
    /// </summary>
    /// <returns>
    ///     A <typeparamref name="TSettings" /> instance hydrated with settings from configuration.
    ///     These are not validated, but are necessary for use when configuring the service on startup.
    /// </returns>
    public TSettings WithSettings<TSettings>()
        where TSettings : class
    {
        Builder.Services.AddAndValidateSettings<TSettings>();
        _settingsTypes.Add(typeof(TSettings));
        TSettings _settings = Builder.Configuration.GetRequiredSettings<TSettings>();
        if (_settings is IMySQLConnectionString _sqlConnectionString)
            Builder.Services.AddSingleton(_sqlConnectionString);
        Builder.Services.AddSingleton(Provider => Provider.GetService<IOptions<TSettings>>()!.Value);
        return _settings;
    }

    /// <summary>
    ///     Exposes <see cref="IHostApplicationBuilder.Services" /> Configure method to allow
    ///     registration of an action used to configure a particular type of options.
    /// </summary>
    public TBuilder Configure<TOptions>(Action<TOptions> ConfigureAction)
        where TOptions : class
    {
        Builder.Services.Configure(ConfigureAction);
        return (TBuilder)this;
    }

    private RefitSettings? GetRefitSettings(IHttpContentSerializer? HttpContentSerializer)
    {
        bool _useAuthHeaderGetter = _getBearerTokenAsyncFunc is not null;
        if (!_useAuthHeaderGetter && HttpContentSerializer is null) return null;
        RefitSettings _settings = new RefitSettings();
        if (_useAuthHeaderGetter)
            _settings.AuthorizationHeaderValueGetter = (_, CancellationToken) =>
                AuthBearerTokenFactory.GetBearerTokenAsync(CancellationToken);
        if (HttpContentSerializer is not null) _settings.ContentSerializer = HttpContentSerializer;
        return _settings;
    }
}