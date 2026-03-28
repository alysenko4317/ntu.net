using System;
using DynamicData;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using DeviceVisualizer.Infrastructure;
using DeviceVisualizer.Models;
using DeviceVisualizer.Services;

namespace DeviceVisualizer.ViewModels;

public sealed class MainWindowViewModel : ReactiveObject, IDisposable
{
    private readonly IFakeSensorService _sensorService;
    private readonly ILogger _logger;
    private readonly CompositeDisposable _disposables = new();

    private readonly SourceCache<VariableInfo, string> _deviceInfoSource = new(x => x.Key);
    private readonly ObservableAsPropertyHelper<bool> _canStart;
    private readonly ObservableAsPropertyHelper<string> _validationError;

    public ReadOnlyObservableCollection<VariableInfo> DeviceInfo { get; }

    [Reactive]
    public string TargetValue { get; set; } = "50";

    [Reactive]
    public double CurrentPressure { get; set; }

    [Reactive]
    public bool IsRunning { get; set; }

    public bool CanStart => _canStart.Value;

    public string ValidationError => _validationError.Value;

    public ReactiveCommand<Unit, Unit> StartCommand { get; }
    public ReactiveCommand<Unit, Unit> StopCommand { get; }

    public ObservableCollection<ISeries> Series { get; } = [];
    public Axis[] XAxes { get; }
    public Axis[] YAxes { get; }

    private readonly ObservableCollection<ObservablePoint> _chartValues = [];

    public MainWindowViewModel(IFakeSensorService sensorService, ILogger logger)
    {
        _sensorService = sensorService;
        _logger = logger;

        _validationError = this.WhenAnyValue(vm => vm.TargetValue)
            .Select(value => double.TryParse(value, out _) ? string.Empty : "Only numeric input is allowed")
            .ToProperty(this, x => x.ValidationError);

        _deviceInfoSource.Connect()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Bind(out var deviceInfo)
            .Subscribe()
            .DisposeWith(_disposables);

        DeviceInfo = deviceInfo;

        Series.Add(new LineSeries<ObservablePoint>
        {
            Values = _chartValues
        });

        XAxes =
        [
            new Axis()
        ];

        YAxes =
        [
            new Axis()
        ];

        _canStart = this.WhenAnyValue(
                x => x.TargetValue,
                x => x.IsRunning,
                (target, isRunning) => !string.IsNullOrWhiteSpace(target) &&
                                       double.TryParse(target, out _) &&
                                       !isRunning)
            .ToProperty(this, x => x.CanStart);

        StartCommand = ReactiveCommand.Create(Start, this.WhenAnyValue(x => x.CanStart));
        StopCommand = ReactiveCommand.Create(Stop, this.WhenAnyValue(x => x.IsRunning));

        StartCommand
            .Subscribe(_ => _logger.Info("Monitoring started"))
            .DisposeWith(_disposables);

        StopCommand
            .Subscribe(_ => _logger.Info("Monitoring stopped"))
            .DisposeWith(_disposables);

        this.WhenAnyValue(x => x.IsRunning)
            .DistinctUntilChanged()
            .SelectMany(running =>
                running
                    ? _sensorService.CreatePressureStream()
                    : Observable.Empty<double>())
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(UpdatePressure)
            .DisposeWith(_disposables);

        MessageBus.Current.Listen<ApplicationLog>()
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe()
            .DisposeWith(_disposables);

        FillInfo();
    }

    private void Start()
    {
        IsRunning = true;
    }

    private void Stop()
    {
        IsRunning = false;
    }

    private void UpdatePressure(double pressure)
    {
        CurrentPressure = pressure;

        _deviceInfoSource.AddOrUpdate(new VariableInfo { Key = "Pressure", Value = pressure.ToString("F2") });
        _deviceInfoSource.AddOrUpdate(new VariableInfo { Key = "Updated", Value = DateTime.Now.ToString("HH:mm:ss") });

        _chartValues.Add(new ObservablePoint(DateTime.Now.Ticks, pressure));
        if (_chartValues.Count > 30)
            _chartValues.RemoveAt(0);
    }

    private void FillInfo()
    {
        _deviceInfoSource.AddOrUpdate(new VariableInfo { Key = "Device", Value = "Demo Sensor" });
        _deviceInfoSource.AddOrUpdate(new VariableInfo { Key = "Mode", Value = "Simulation" });
        _deviceInfoSource.AddOrUpdate(new VariableInfo { Key = "Status", Value = "Ready" });
    }

    public void Dispose()
    {
        _disposables.Dispose();
        _deviceInfoSource.Dispose();
    }
}