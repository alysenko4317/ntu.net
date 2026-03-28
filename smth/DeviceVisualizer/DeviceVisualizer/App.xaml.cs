using DeviceVisualizer.Infrastructure;
using ReactiveUI;
using Splat;
using System.Windows;

namespace DeviceVisualizer;

public partial class App : Application
{
    private Infrastructure.ILogger? _logger;

    public App()
    {
        Bootstrapper.BuildIoC();
        _logger = Locator.Current.GetService<Infrastructure.ILogger>();
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        RxApp.DefaultExceptionHandler = new AppExceptionHandler(_logger!);

        DispatcherUnhandledException += (_, args) =>
        {
            _logger?.Error("DispatcherUnhandledException", args.Exception);
            args.Handled = true;
        };
    }
}