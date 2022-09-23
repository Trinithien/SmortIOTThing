using SmortIOTThing.Desktop.EventArguements;
using SmortIOTThing.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace SmortIOTThing.Desktop.Events
{
    public class FakeTemperatureSensorStatus : ITemperatureSensorStatus
    {
        public event EventHandler StatusChanged = null!;
        public FakeTemperatureSensorStatus()
        {
            _ = CheckStatusAsync();
        }
        public virtual void OnStatusChanged(TemperatureSensorEventArgs e)
        {
            EventHandler handler = StatusChanged;
            handler?.Invoke(this, e);
        }
        private double Disturbence()
        {
            Random random = new Random();
            var test = random.Next(0, 10000);
            if (test < 10)
            {
                return -random.NextDouble() * 2;
            }
            return 0;
        }
        private double Regulate(double setPoint)
        {
            //var p = 1.1;
            //var i = 1000000;
            //var delta = setPoint - current;
            //ki = ki + delta / i;
            //return delta * p + ki;
            if (lastValues.Count > 50)
            {
                var val = lastValues[0];
                lastValues.RemoveAt(0);
                if (val < setPoint)
                {
                    return 0.015;
                }
            }
            return 0;
        }
        List<double> lastValues = new();
        double lastCurrent = 20;
        double floor = 11;
        private double GenerateValue()
        {
            Random random = new Random();
            lastValues.Add(lastCurrent);
            lastCurrent = (float)(lastCurrent - (lastCurrent - floor) / 1000 + Regulate(16) + Disturbence());
            if (lastCurrent < 11)
            {
                lastCurrent = 11;
            }
            return lastCurrent;
        }
        private async Task CheckStatusAsync()
        {
            var periodicTimer = new PeriodicTimer(new TimeSpan(0, 0, 0, 0, 10));
            while (await periodicTimer.WaitForNextTickAsync())
            {
                var e = new TemperatureSensorEventArgs
                {
                    SensorSeries = new[]
                    {
                        new SensorSerie
                        {
                            Name = "Temparature Sensor 1",
                            SensorPoints = new List<SensorPoint>
                            {
                                new SensorPoint
                                {
                                    Value = GenerateValue(),
                                    Timestamp = DateTimeOffset.Now
                                }
                            }
                        }
                    }
                };
                OnStatusChanged(e);
            }
        }


        
    }
}
