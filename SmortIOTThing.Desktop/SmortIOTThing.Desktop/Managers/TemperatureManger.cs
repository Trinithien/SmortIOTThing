using SmortIOTThing.Desktop.Interfaces;
﻿using SmortIOTThing.Desktop.EventArguements;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmortIOTThing.Desktop.Managers
{
    public class TemperatureManger : ITemperatureManager
    {
        List<SensorSerie> _sensors = new();

        public TemperatureManger(ITemperatureSensorStatus temperatureSensorStatus)
        {
            temperatureSensorStatus.StatusChanged += UpdateStatus;
        }
        private void UpdateStatus(object sender,object e)
        {
            if(e is TemperatureSensorEventArgs args)
            {
                var names = args.SensorSeries.Select(serie => serie.Name).ToList();
                var sensors = _sensors.Where(sensor => names.Contains(sensor.Name));
                //Update old series
                foreach (var sensor in sensors)
                {
                    var newPoints = args.SensorSeries.Where(serie => serie.Name == sensor.Name)
                                                     .Single().SensorPoints;
                    sensor.SensorPoints.AddRange(newPoints);
                    names.Remove(sensor.Name);

                }
                //Add missing series
                _sensors.AddRange(args.SensorSeries.Where(serie => names.Contains(serie.Name)));
                
            }
        }
        public float GetAverageTemperature(string name, DateTimeOffset from, DateTimeOffset to)
        {
            var sensor = _sensors.Where(sensor => sensor.Name == name).FirstOrDefault();
            var points = sensor.SensorPoints.Where(point => point.Timestamp >= from && point.Timestamp <= to).ToArray();
            return (float)(points.Average(point => point.Value));
        }
        public float GetCurrentTemperature(string name)
        {
            var sensor = _sensors.Where(sensor => sensor.Name == name).SingleOrDefault();
            if(sensor == null)
            {
                throw new Exception("No sensor with that name");
            }

            return (float)sensor.SensorPoints[^1].Value;
        }

        public float GetMaxTemperature(string name, DateTimeOffset from, DateTimeOffset to)
        {
            var sensor = _sensors.Where(sensor => sensor.Name == name).FirstOrDefault();
            if(sensor == null)
            {
                throw new("No sensor with that name");
            }
            var points = sensor.SensorPoints.Where(point => point.Timestamp >= from && point.Timestamp <= to).ToArray();
            return (float)(points.Max(point => point.Value));
        }

        public float GetMinTemperature(string name, DateTimeOffset from, DateTimeOffset to)
        {
            var sensor = _sensors.Where(sensor => sensor.Name == name).FirstOrDefault();
            if (sensor == null)
            {
                throw new("No sensor with that name");
            }
            var points = sensor.SensorPoints.Where(point => point.Timestamp >= from && point.Timestamp <= to).ToArray();
            return (float)(points.Min(point => point.Value));
        }

        public async Task<SensorSerie> GetSensorSeriesAsync(string name)
        {
            return await GetSensorSeriesAsync(name,DateTimeOffset.MinValue);
        }

        public async Task<SensorSerie> GetSensorSeriesAsync(string name, DateTimeOffset from)
        {
            return await GetSensorSeriesAsync(name, from, DateTimeOffset.MaxValue);
        }

        public async Task<SensorSerie> GetSensorSeriesAsync(string name, DateTimeOffset from, DateTimeOffset to)
        {
            var sensor = _sensors.Where(sensor => sensor.Name == name).FirstOrDefault();
            await Task.Delay(1);
            var points = sensor.SensorPoints.Where(point => point.Timestamp >= from && point.Timestamp <= to).ToArray();
            return new SensorSerie { Name = sensor.Name, Unit =sensor.Unit, SensorPoints = points.ToList()};
        }

        public SensorSerie[] GetSensors()
        {
            return _sensors.Select(sensor => new SensorSerie { Name = sensor.Name, Unit = sensor.Unit }).ToArray();
        }
    }
}
