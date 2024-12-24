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
    internal static TSettings GetRequiredSettings<TSettings>(this IConfiguration Config)
        where TSettings : class
    {
        string _settingsName = typeof(TSettings).Name;
        TSettings _settings = Config.GetSection(_settingsName).Get<TSettings>() ??
                              throw new ApplicationException($"{_settingsName} required, but missing!");
        return _settings;
    }

    internal static void AddLoggingConfig(this IConfigurationBuilder Config,
        IHostEnvironment Environment, string SettingsFileName = "loggingSettings")
    {
        System.Environment.SetEnvironmentVariable("APP_BASE_DIRECTORY",
            WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : Directory.GetCurrentDirectory());
        Config.AddJsonFile($"{SettingsFileName}.json", false);
        Config.AddJsonFile($"{SettingsFileName}.{Environment.EnvironmentName}.json", true);
    }
}