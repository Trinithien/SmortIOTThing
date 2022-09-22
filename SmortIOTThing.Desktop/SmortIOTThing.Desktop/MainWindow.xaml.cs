﻿using DjupvikCharts;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using SmortIOTThing.Desktop.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SmortIOTThing.Desktop
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        readonly IRequestManager _requestManager;
        public MainWindow(IRequestManager requestManager, ITemperatureSensorStatus temperatureSensorStatus)
        {
            _requestManager = requestManager;
            temperatureSensorStatus.StatusChanged += UpdateStatus;
            this.InitializeComponent();
            InitLineChart();
            
        }

        List<LinePoint> points = new();
        List<LinePoint> allPoints = new();
        LineChart chart = new();
        private async void UpdateStatus(object sender, object e)
        {
            TemperatureStatus.Text = _requestManager.GetTemperatureStatus();
            await ShowChart();
        }
        private Serie ConvertToSerie(SensorSerie sensorSerie,Color color)
        {
            var serie = new Serie
            {
                Name = sensorSerie.Name,
                SeriesPoints = sensorSerie.SensorPoints.Select(point => 
                                           new LinePoint 
                                           { 
                                               Value = point.Value, 
                                               Timestamp = point.Timestamp 
                                           }).ToArray(), 
                Color = color
            };
            return serie;
        }
        private async Task ShowChart()
        {

            List<Serie> series = new();
            var minTime = DateTimeOffset.Now.AddSeconds(-100);
            var maxTime = DateTimeOffset.Now;
            var alarmPointsLower = CreateAlarmLine(minTime, maxTime, 15);
            var alarmPointsUpper = CreateAlarmLine(minTime, maxTime, 30);
            foreach (var name in _requestManager.GetSensors().Select(sensor => sensor.Name))
            {
                var sensorSerie = await _requestManager.GetSensorSeriesAsync(name, from: DateTimeOffset.Now.AddSeconds(-100));
                var serie = ConvertToSerie(sensorSerie, Colors.Purple);
                series.Add(serie);
            }
            series.Add(new Serie { SeriesPoints = alarmPointsLower.ToArray(), Color = Colors.Red, Name = "Alarm" });
            series.Add(new Serie { SeriesPoints = alarmPointsUpper.ToArray(), Color = Colors.Red, Name = "Alarm" });
            await chart.Draw(series.ToArray());

        }

        private static List<LinePoint> CreateAlarmLine(DateTimeOffset minTime, DateTimeOffset maxTime, double alarmValueUpper)
        {
            return new List<LinePoint>
            {
                new LinePoint{Timestamp = minTime , Value =alarmValueUpper},
                new LinePoint{Timestamp = maxTime , Value =alarmValueUpper}
            };
        }

        private void InitLineChart()
        {
            var profile = new ChartColorProfile
            {
                Background = Colors.Pink,
                Outline = Colors.Gray,
                BacklineColor = Colors.White,
                TextForeground = new SolidColorBrush(Colors.Gold),
                TextForegroundAlarm = new SolidColorBrush(Colors.MediumVioletRed)
            };
            chart.Root = root;
            chart.SegmentsX = 5;
            chart.SegmentsY = 5;
            chart.ResolutionY = 2;
            chart.ColorProfile = profile;
            chart.UseResolution = false;
        }

        private async void Window_SizeChanged(object sender, WindowSizeChangedEventArgs args)
        {
            await ShowChart();
        }
    }


}
