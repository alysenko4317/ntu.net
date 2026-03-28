using Autofac;
using ReactiveUI;
using Splat;
using Splat.Autofac;

using DeviceVisualizer.Services;
using DeviceVisualizer.ViewModels;
using DeviceVisualizer.Views;

namespace DeviceVisualizer.Infrastructure;

public static class Bootstrapper
{
    public static void BuildIoC()
    {
        var builder = new ContainerBuilder();

        RegisterServices(builder);
        RegisterViews(builder);

        var resolver = builder.UseAutofacDependencyResolver();
        builder.RegisterInstance(resolver);

        resolver.InitializeReactiveUI();

        var scope = builder.Build();
        resolver.SetLifetimeScope(scope);
    }

    private static void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterType<AppLogger>().As<ILogger>().SingleInstance();
        builder.RegisterType<FakeSensorService>().As<IFakeSensorService>().SingleInstance();
        builder.RegisterType<MainWindowViewModel>().SingleInstance();
    }

    private static void RegisterViews(ContainerBuilder builder)
    {
        builder.RegisterType<MainWindow>().As<IViewFor<MainWindowViewModel>>().SingleInstance();
    }
}
