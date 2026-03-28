using System;

namespace DeviceVisualizer.Models;

public sealed class SensorPoint
{
    public DateTime Time { get; set; }
    public double Value { get; set; }
}