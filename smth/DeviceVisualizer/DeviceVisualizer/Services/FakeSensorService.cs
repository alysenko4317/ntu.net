using System;
using System.Reactive.Linq;

namespace DeviceVisualizer.Services;

public sealed class FakeSensorService : IFakeSensorService
{
    public IObservable<double> CreatePressureStream()
    {
        var random = new Random();

        return Observable
            .Interval(TimeSpan.FromSeconds(1))
            .Select(_ => 40 + random.NextDouble() * 20);
    }
}