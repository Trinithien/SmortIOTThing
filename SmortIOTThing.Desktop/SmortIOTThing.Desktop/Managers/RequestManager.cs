using SmortIOTThing.Desktop.Interfaces;
using System;

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
    }
}
