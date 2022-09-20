using SmortIOTThing.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmortIOTThing.Desktop.Managers
{
    public class TemperatureManger : ITemperatureManager
    {
        public float GetAverageTemperature(string name, TimeSpan time)
        {
            Random random = new Random();
            return (float)(15 + random.NextDouble()*10); //Gir ett random tall mellom 15-25
        }

        public float GetCurrentTemperature(string name)
        {
            Random random = new Random();
            return (float)(15 + random.NextDouble() * 10); //Gir ett random tall mellom 15-25
        }

        public float GetMaxTemperature(string name, TimeSpan time)
        {
            Random random = new Random();
            return (float)(15 + random.NextDouble() * 10); //Gir ett random tall mellom 15-25
        }

        public float GetMinTemperature(string name, TimeSpan time)
        {
            Random random = new Random();
            return (float)(15 + random.NextDouble() * 10); //Gir ett random tall mellom 15-25
        }
    }
}
