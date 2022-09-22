using SmortIOTThing.Desktop.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmortIOTThing.Desktop.Managers
{
    public class RequestManager: IRequestManager
    {
        readonly ITemperatureManager _temperatureManager;
        public RequestManager(ITemperatureManager temperatureManager)
        {
            _temperatureManager = temperatureManager;
        }
        public string GetWelcomeMessage()
        {
            return "Welcome";
        }

        public string GetTemperatureStatus()
        {
            string name = "Temparature Sensor 1";
            TimeSpan time = new TimeSpan(0,0,5); //Fem sekunder
            float current, average, min, max;
            current = _temperatureManager.GetCurrentTemperature(name);
            average = _temperatureManager.GetAverageTemperature(name, time);
            min = _temperatureManager.GetMinTemperature(name, time);
            max = _temperatureManager.GetMaxTemperature(name, time);

            string status =
                $"Sensor: {name}\r\n" +
                $"Timespan for calculations: {time}\r\n" +
                $"Current Temperature: {current}\r\n" +
                $"Average Temperature: {average}\r\n" +
                $"Min Temperature:     {min}\r\n" +
                $"Max Temperature:     {max}";
            return status;
        }

        public SensorSerie[] GetSensors()
            => _temperatureManager.GetSensors();
        public double GetTemperature(string name = "")
        {
            if (name == "")
            {
                var sensors = _temperatureManager.GetSensors().Select(sensor => sensor.Name);
                if (sensors.Count() > 0)
                {
                    return _temperatureManager.GetCurrentTemperature(sensors.ElementAt(0));
                }
            }
            return _temperatureManager.GetCurrentTemperature(name);
                
        }

        public Task<SensorSerie> GetSensorSeriesAsync(string name)
            => _temperatureManager.GetSensorSeriesAsync(name);

        public Task<SensorSerie> GetSensorSeriesAsync(string name, DateTimeOffset from)
            => _temperatureManager.GetSensorSeriesAsync(name, from);

        public Task<SensorSerie> GetSensorSeriesAsync(string name, DateTimeOffset from, DateTimeOffset to)
            => _temperatureManager.GetSensorSeriesAsync(name, from, to);
    }
}
