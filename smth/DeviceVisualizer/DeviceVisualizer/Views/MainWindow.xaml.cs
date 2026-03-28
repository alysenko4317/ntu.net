using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using DeviceVisualizer.ViewModels;

namespace DeviceVisualizer.Views;

public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
{
    public MainWindow()
    {
        InitializeComponent();

        ViewModel = Locator.Current.GetService<MainWindowViewModel>();
        DataContext = ViewModel;

        this.WhenActivated(disposables =>
        {
            this.Bind(ViewModel, vm => vm.TargetValue, v => v.TargetValueTextBox.Text)
                .DisposeWith(disposables);

            this.OneWayBind(ViewModel, vm => vm.DeviceInfo, v => v.DeviceInfoGrid.ItemsSource)
                .DisposeWith(disposables);

            this.OneWayBind(ViewModel, vm => vm.CurrentPressure, v => v.CurrentPressureText.Text,
                    value => $"Current pressure: {value:F2}")
                .DisposeWith(disposables);

            this.OneWayBind(ViewModel, vm => vm.Series, v => v.Plot.Series)
                .DisposeWith(disposables);

            this.OneWayBind(ViewModel, vm => vm.XAxes, v => v.Plot.XAxes)
                .DisposeWith(disposables);

            this.OneWayBind(ViewModel, vm => vm.YAxes, v => v.Plot.YAxes)
                .DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.StartCommand, v => v.StartButton)
                .DisposeWith(disposables);

            this.BindCommand(ViewModel, vm => vm.StopCommand, v => v.StopButton)
                .DisposeWith(disposables);

            this.OneWayBind(ViewModel, vm => vm.ValidationError, v => v.ValidationErrors.Text)
                .DisposeWith(disposables);

            this.WhenAnyValue(v => v.ViewModel!.ValidationError)
                .Select(text => string.IsNullOrWhiteSpace(text) ? Visibility.Collapsed : Visibility.Visible)
                .BindTo(this, v => v.ValidationErrors.Visibility)
                .DisposeWith(disposables);
        });
    }
}