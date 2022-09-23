using System;
namespace SmortIOTThing.Desktop;
public class SensorResponse
{
    public double Value { get; set; }
    public DateTimeOffset Timestamp { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
}
