using System;

namespace SmortIOTThing.Desktop.EventArguements
{
    public class TemperatureSensorEventArgs : EventArgs
    {
        public SensorSerie[] SensorSeries { get; set; }
        
    }
}
