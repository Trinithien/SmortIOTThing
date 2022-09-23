

using System;
using System.Threading.Tasks;

namespace SmortIOTThing.Desktop.Interfaces
{
    public interface IRequestManager
    {
        public string GetWelcomeMessage();
        public string GetTemperatureStatus(string name);
        public double GetTemperature(string name = "");
        public Task<SensorSerie> GetSensorSeriesAsync(string name);
        public Task<SensorSerie> GetSensorSeriesAsync(string name, DateTimeOffset from);
        public Task<SensorSerie> GetSensorSeriesAsync(string name, DateTimeOffset from, DateTimeOffset to);
        public SensorSerie[] GetSensors();
    }
}
