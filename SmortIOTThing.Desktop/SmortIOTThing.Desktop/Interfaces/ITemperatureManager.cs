using System;

namespace SmortIOTThing.Desktop.Interfaces
{
    public interface ITemperatureManager
    {
        public float GetCurrentTemperature(string name);
        public float GetAverageTemperature(string name, TimeSpan time);
        public float GetMaxTemperature(string name, TimeSpan time);
        public float GetMïnTemperature(string name, TimeSpan time);
        
    }
}
