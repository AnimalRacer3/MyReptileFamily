using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;

namespace MyReptileFamilyLibrary.Extensions.Internal;

internal static class ConfigurationInternalExtensions
{
    /// <summary>
    ///     Gets strongly-typed <typeparamref name="TSettings" /> from configuration
    /// </summary>
    /// <exception cref="ApplicationException">Thrown when expected configuration section is missing</exception>
    internal static TSettings GetRequiredSettings<TSettings>(this IConfiguration _p_Config)
        where TSettings : class
    {
        var _settingsName = typeof(TSettings).Name;
        var _settings = _p_Config.GetSection(_settingsName).Get<TSettings>() ?? throw new ApplicationException($"{_settingsName} required, but missing!");
        return _settings;
    }

    internal static void AddLoggingConfig(this IConfigurationBuilder _p_Config,
        IHostEnvironment _p_Environment, string _p_SettingsFileName = "loggingSettings")
    {
        Environment.SetEnvironmentVariable("APP_BASE_DIRECTORY",
            WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : Directory.GetCurrentDirectory());
        _p_Config.AddJsonFile($"{_p_SettingsFileName}.json", false);
        _p_Config.AddJsonFile($"{_p_SettingsFileName}.{_p_Environment.EnvironmentName}.json", true);
    }
}