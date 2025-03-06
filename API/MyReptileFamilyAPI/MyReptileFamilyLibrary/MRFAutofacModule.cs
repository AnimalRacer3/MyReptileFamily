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
    protected override void Load(ContainerBuilder Builder)
    {
        Assembly _thisAssembly = GetType().Assembly;
        Assembly _workerAssembly = Assembly.GetEntryAssembly()!; // Confirmed working for EXE and when running as a Service
        Builder.RegisterAssemblyTypes(_thisAssembly, _workerAssembly)
            .Where(Type => Type.HasNonSystemInterfaces())
            .Where(Type => Type.BaseType != typeof(EmailServiceBase))
            .Where(Type => !Type.FullName?.StartsWith("Refit.Implementation") ?? false)
            .Where(Type => !Type.FullName?.EndsWith("Settings") ?? false)
            .Where(Type => Type.OnlyHasDirectInterfaces())
            .Where(Type => !Type.Namespace?.Contains("LegacyServices") ?? false)
            .Where(Type => Type.DoesNotImplementDapperInterfaces())
            .Where(Type => !Type.Namespace?.EndsWith("StringTemplateParsing.DTO") ?? false)
            .AsImplementedInterfaces(); // Automatic registration of IFoo -> Foo etc.

        // Singleton registration + registering .NET 8's TimeProvider
        Builder
            .RegisterInstance(TimeProvider.System)
            .As<TimeProvider>()
            .SingleInstance();

        // Application will not start if this is left uncommented!
        // var _registeredComponents = Builder.Build().ComponentRegistry.Registrations; // Uncomment and debug to view all the things Autofac will do
    }
}