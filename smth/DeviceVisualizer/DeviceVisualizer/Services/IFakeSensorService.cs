using System;

namespace DeviceVisualizer.Services;

public interface IFakeSensorService
{
    IObservable<double> CreatePressureStream();
}