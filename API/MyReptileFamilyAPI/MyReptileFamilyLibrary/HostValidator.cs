using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MyReptileFamilyLibrary.AppSettings;
using MyReptileFamilyLibrary.Exceptions;
using MySqlConnector;
using Serilog.Debugging;

namespace MyReptileFamilyLibrary;

internal class HostValidator
{
    /// <summary>
    ///     Validates each of the settings types given (DataAnnotations, and any database connection strings);
    ///     also validates that Serilog does not throw when logging (i.e. has permissions to write to file);
    ///     also warns of any legacy services in use
    /// </summary>
    /// <param name="Host">The created <see cref="IHost" /> instance</param>
    /// <param name="SettingsTypesToValidate">Collection of types that represent settings, to be validated</param>
    /// <param name="CustomValidator">Custom validation that's invoked upon validation</param>
    /// <exception cref="ApplicationException">Thrown if any validation error occurs</exception>
    internal static void Validate(IHost Host, IEnumerable<Type> SettingsTypesToValidate,
        Func<IHost, ILogger, bool> CustomValidator)
    {
        // Validates Serilog
        EnableSerilogValidation();
        ILogger<HostValidator> _logger = Host.Services.GetRequiredService<ILogger<HostValidator>>();
        _logger.LogDebug("[{Validator}] Beginning validation", nameof(HostValidator));
        SelfLog.Disable();

        // Validates all settings
        bool _allSettingsValid =
            SettingsTypesToValidate.All(T => ValidateSettings(T, Host.Services, _logger));
        _allSettingsValid = _allSettingsValid && CustomValidator(Host, _logger);
        if (!_allSettingsValid) throw new HostValidationException("Not all settings were found to be valid");
    }

    private static bool ValidateSettings(Type SettingsToValidate, IServiceProvider Services, ILogger Logger)
    {
        Type _optionsType = typeof(IOptions<>).MakeGenericType(SettingsToValidate);
        object? _resolvedService = Services.GetService(_optionsType);
        if (_resolvedService is not IOptions<object> _options) return true;
        object _settings;
        try
        {
            _settings = _options.Value;
        }
        catch (Exception _ex)
        {
            Logger.LogError(_ex, "[{Validator}] [{SettingsClass}] validation error", nameof(HostValidator),
                SettingsToValidate.Name);
            return false;
        }

        bool _isValid = true;
        if (_settings is IMySQLConnectionString _settingsWithConnectionString)
            _isValid = ValidateConnectionString(Logger, _settingsWithConnectionString.MySQLConnectionString,
                           "SQL connection string", SettingsToValidate.Name)
                       && _isValid;
        if (_isValid)
            Logger.LogDebug("[{Validator}] [{SettingsClass}] Valid", nameof(HostValidator),
                SettingsToValidate.Name);
        return _isValid;
    }

    private static bool ValidateConnectionString(ILogger Logger, string ConnectionString,
        string ConnectionStringName, string SettingsName)
    {
        try
        {
            using MySqlConnection _connection = new MySqlConnection(ConnectionString);
            _connection.Open();
            Logger.LogDebug("[{Validator}] [{SettingsClass}] {ConnectionStringName} connected successfully",
                nameof(HostValidator), SettingsName, ConnectionStringName);
            return true;
        }
        catch (Exception _ex)
        {
            Logger.LogError(_ex,
                "[{Validator}] [{SettingsClass}] Validation Failed - {ConnectionStringName} failed to connect: {ConnectionString}",
                nameof(HostValidator), SettingsName, ConnectionStringName, ConnectionString);
            return false;
        }
    }

    private static void EnableSerilogValidation()
    {
        SelfLog.Enable(Message =>
        {
            string _appIdentifier = Assembly.GetExecutingAssembly().FullName?.Split(",").FirstOrDefault() ?? "Program";
            string _logPath = Path.Join(AppContext.BaseDirectory,
                $"[{DateTimeOffset.UtcNow:yyyy-MM-dd HH.mm.ss}]-[{_appIdentifier}]-SerilogError.txt");
            string _messageToLog = $"[{nameof(HostValidator)}] Serilog Validation Error - {Message}";
            try
            {
                File.AppendAllText(_logPath, _messageToLog);
                throw new HostValidationException(
                    $"[{nameof(HostValidator)}] Serilog Validation Error - Check [{_logPath}] for errors");
            }
            catch (Exception _ex)
            {
                // Last resort. Most likely means we won't see these errors when running as a Windows service
                Console.WriteLine($"[{nameof(HostValidator)}] Serilog Validation Error: {Message}");
                Console.WriteLine($"[{nameof(HostValidator)}] COULD NOT WRITE THIS TO FILE: {_ex}");
                throw new HostValidationException(
                    $"Serilog Validation Error: {Message}{Environment.NewLine}COULD NOT WRITE THIS EXCEPTION TO FILE: {_ex}");
            }
        });
    }
}