using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace MyReptileFamilyLibrary.Extensions;

/// <summary>
///     Extensions for <see cref="IServiceCollection" /> that are NOT for use in the published NuGet package
/// </summary>
internal static class ServiceCollectionInternalExtensions
{
    /// <summary>
    ///     All types that were passed into <see cref="AddAndValidateSettings{TSettings}" />
    /// </summary>
    internal static readonly List<Type> RegisteredSettingsTypes = [];

    internal static OptionsBuilder<TSettings> AddAndValidateSettings<TSettings>(this IServiceCollection Services)
        where TSettings : class
    {
        RegisteredSettingsTypes.Add(typeof(TSettings));
        return Services.AddOptions<TSettings>()
            .BindConfiguration(typeof(TSettings).Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}