using System.Reflection;
using Autofac;
using MyReptileFamilyLibrary.Extensions;
using MyReptileFamilyLibrary.Services;
using AutofacModule = Autofac.Module;

namespace MyReptileFamilyLibrary;

/// <summary>
///     Registers dependencies inside this class library
/// </summary>
public class MRFAutofacModule : AutofacModule
{
    protected override void Load(ContainerBuilder _p_Builder)
    {
        var _thisAssembly = GetType().Assembly;
        var _workerAssembly = Assembly.GetEntryAssembly()!; // Confirmed working for EXE and when running as a Service
        _p_Builder.RegisterAssemblyTypes(_thisAssembly, _workerAssembly)
            .Where(_p_Type => _p_Type.HasNonSystemInterfaces())
            .Where(_p_Type => _p_Type.BaseType != typeof(EmailServiceBase))
            .Where(_p_Type => !_p_Type.FullName?.StartsWith("Refit.Implementation") ?? false)
            .Where(_p_Type => !_p_Type.FullName?.EndsWith("Settings") ?? false)
            .Where(_p_Type => _p_Type.OnlyHasDirectInterfaces())
            .Where(_p_Type => !_p_Type.Namespace?.Contains("LegacyServices") ?? false)
            .Where(_p_Type => _p_Type.DoesNotImplementDapperInterfaces())
            .Where(_p_Type => !_p_Type.Namespace?.EndsWith("StringTemplateParsing.DTO") ?? false)
            .AsImplementedInterfaces(); // Automatic registration of IFoo -> Foo etc.

        // Singleton registration + registering .NET 8's TimeProvider
        _p_Builder
            .RegisterInstance(TimeProvider.System)
            .As<TimeProvider>()
            .SingleInstance();

        // Application will not start if this is left uncommented!
        // var _registeredComponents = _p_Builder.Build().ComponentRegistry.Registrations; // Uncomment and debug to view all the things Autofac will do
    }
}