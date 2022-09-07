using SmortIOTThing.Desktop.EventArguements;
using SmortIOTThing.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmortIOTThing.Desktop.Events
{
    public class TemperatureSensorStatus : ITemperatureSensorStatus
    {
        public event EventHandler StatusChanged = null!;
        private DateTimeOffset time = DateTimeOffset.Now;
        public TemperatureSensorStatus()
        {
            _ = CheckStatusAsync();
        }
        public virtual void OnStatusChanged(TemperatureSensorEventArgs e)
        {
            EventHandler handler = StatusChanged;
            handler?.Invoke(this, e);
        }

        private async Task CheckStatusAsync()
        {
            var periodicTimer = new PeriodicTimer(new TimeSpan(0, 0, 1));
            while (await periodicTimer.WaitForNextTickAsync())
            {
                var e = new TemperatureSensorEventArgs { Names = new[] { "Temparature Sensor 1" } };
                OnStatusChanged(e);
            }
        }

    }
}
