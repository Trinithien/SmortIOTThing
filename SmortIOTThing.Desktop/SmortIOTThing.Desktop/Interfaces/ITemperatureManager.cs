using System;
using System.Threading.Tasks;

namespace SmortIOTThing.Desktop.Interfaces
{
    public interface ITemperatureManager
    {
        public SensorSerie[] GetSensors();
        public float GetCurrentTemperature(string name);
        public float GetAverageTemperature(string name, TimeSpan time);
        public float GetMaxTemperature(string name, TimeSpan time);
        public float GetMinTemperature(string name, TimeSpan time);
        public Task<SensorSerie> GetSensorSeriesAsync(string name);
        public Task<SensorSerie> GetSensorSeriesAsync(string name, DateTimeOffset from);
        public Task<SensorSerie> GetSensorSeriesAsync(string name, DateTimeOffset from,DateTimeOffset to);
        
    }
}
