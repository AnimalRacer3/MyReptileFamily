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
    /// <param name="_p_Host">The created <see cref="IHost" /> instance</param>
    /// <param name="_p_SettingsTypesToValidate">Collection of types that represent settings, to be validated</param>
    /// <param name="_p_CustomValidator">Custom validation that's invoked upon validation</param>
    /// <exception cref="ApplicationException">Thrown if any validation error occurs</exception>
    internal static void Validate(IHost _p_Host, IEnumerable<Type> _p_SettingsTypesToValidate, Func<IHost, ILogger, bool> _p_CustomValidator)
    {
        // Validates Serilog
        EnableSerilogValidation();
        var _logger = _p_Host.Services.GetRequiredService<ILogger<HostValidator>>();
        _logger.LogDebug("[{Validator}] Beginning validation", nameof(HostValidator));
        SelfLog.Disable();

        // Validates all settings
        var _allSettingsValid = _p_SettingsTypesToValidate.All(_p_T => ValidateSettings(_p_T, _p_Host.Services, _logger));
        _allSettingsValid = _allSettingsValid && _p_CustomValidator(_p_Host, _logger);
        if (!_allSettingsValid) throw new HostValidationException("Not all settings were found to be valid");
    }

    private static bool ValidateSettings(Type _p_SettingsToValidate, IServiceProvider _p_Services, ILogger _p_Logger)
    {
        var _optionsType = typeof(IOptions<>).MakeGenericType(_p_SettingsToValidate);
        var _resolvedService = _p_Services.GetService(_optionsType);
        if (_resolvedService is not IOptions<object> _options) return true;
        object _settings;
        try
        {
            _settings = _options.Value;
        }
        catch (Exception _ex)
        {
            _p_Logger.LogError(_ex, "[{Validator}] [{SettingsClass}] validation error", nameof(HostValidator),
                _p_SettingsToValidate.Name);
            return false;
        }

        bool _isValid = true;
        if (_settings is IMySQLConnectionString _settingsWithConnectionString)
        {
            _isValid = ValidateConnectionString(_p_Logger, _settingsWithConnectionString.MySQLConnectionString, "SQL connection string", _p_SettingsToValidate.Name)
                && _isValid;
        }
        if (_isValid)
        {
            _p_Logger.LogDebug("[{Validator}] [{SettingsClass}] Valid", nameof(HostValidator), _p_SettingsToValidate.Name);
        }
        return _isValid;
    }

    private static bool ValidateConnectionString(ILogger _p_Logger, string _p_ConnectionString, string _p_ConnectionStringName, string _p_SettingsName)
    {
        try
        {
            using var _connection = new MySqlConnection(_p_ConnectionString);
            _connection.Open();
            _p_Logger.LogDebug("[{Validator}] [{SettingsClass}] {ConnectionStringName} connected successfully",
                nameof(HostValidator), _p_SettingsName, _p_ConnectionStringName);
            return true;
        }
        catch (Exception _ex)
        {
            _p_Logger.LogError(_ex, "[{Validator}] [{SettingsClass}] Validation Failed - {ConnectionStringName} failed to connect: {ConnectionString}",
                nameof(HostValidator), _p_SettingsName, _p_ConnectionStringName, _p_ConnectionString);
            return false;
        }
    }

    private static void EnableSerilogValidation()
    {
        SelfLog.Enable(_p_Message =>
        {
            var _appIdentifier = Assembly.GetExecutingAssembly().FullName?.Split(",").FirstOrDefault() ?? "Program";
            var _logPath = Path.Join(AppContext.BaseDirectory,
                $"[{DateTimeOffset.UtcNow:yyyy-MM-dd HH.mm.ss}]-[{_appIdentifier}]-SerilogError.txt");
            var _messageToLog = $"[{nameof(HostValidator)}] Serilog Validation Error - {_p_Message}";
            try
            {
                File.AppendAllText(_logPath, _messageToLog);
                throw new HostValidationException(
                    $"[{nameof(HostValidator)}] Serilog Validation Error - Check [{_logPath}] for errors");
            }
            catch (Exception _ex)
            {
                // Last resort. Most likely means we won't see these errors when running as a Windows service
                Console.WriteLine($"[{nameof(HostValidator)}] Serilog Validation Error: {_p_Message}");
                Console.WriteLine($"[{nameof(HostValidator)}] COULD NOT WRITE THIS TO FILE: {_ex}");
                throw new HostValidationException(
                    $"Serilog Validation Error: {_p_Message}{Environment.NewLine}COULD NOT WRITE THIS EXCEPTION TO FILE: {_ex}");
            }
        });
    }
}