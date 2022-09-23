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
            var minTime = DateTimeOffset.Now.AddSeconds(-100);
            var maxTime = DateTimeOffset.Now;
            TimeSpan time = new TimeSpan(0, (maxTime - minTime).Minutes, (maxTime-minTime).Seconds);
            float current, average, min, max;
            current = _temperatureManager.GetCurrentTemperature(name);
            average = _temperatureManager.GetAverageTemperature(name, from: minTime,to: maxTime);
            min = _temperatureManager.GetMinTemperature(name, from: minTime, to: maxTime);
            max = _temperatureManager.GetMaxTemperature(name, from: minTime, to: maxTime);

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
