using Microsoft.UI.Xaml;
using SmortIOTThing.Desktop.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System;
using Windows.Devices.Geolocation;
using SmortIOTThing.Desktop.EventArguements;
using System.Collections.Generic;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using DjupvikCharts;

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
            WelcomeMessage.Text = _requestManager.GetWelcomeMessage();
            InitLineChart();
            allPoints.Add(new LinePoint { Value = 20, Timestamp = DateTimeOffset.Now.AddSeconds(-100) });
            Random random = new Random();
            for (int i = 1; i < 100; i++)
            {
                allPoints.Add(new LinePoint { Value = allPoints[^1].Value + (random.NextDouble() - 0.5) / 10, Timestamp = DateTimeOffset.Now.AddSeconds(i-100) });

            }
        }

        List<LinePoint> points = new();
        List<LinePoint> allPoints = new();
        LineChart chart = new();
        private async void UpdateStatus(object sender, object e)
        {
            TemperatureStatus.Text = _requestManager.GetTemperatureStatus();
            
            if (allPoints.Count < 100) 
            {
                if (allPoints.Count < 1)
                    points.Add(new LinePoint { Value = 20, Timestamp = DateTimeOffset.Now });
                points = allPoints;
            }
            else
            {
                points = allPoints.GetRange(allPoints.Count - 100, 100);
            }
            Random random = new Random();
            allPoints.Add(new LinePoint { Value = allPoints[^1].Value + (random.NextDouble() - 0.5) / 10, Timestamp = DateTimeOffset.Now });


            if (points.Count > 0)
            {
                
                List<Serie> series = new();
                series.Add(new Serie { SeriesPoints = points.ToArray(), Color = Colors.Purple, Name = "Series1" });

                await chart.CreateLineChart(series.ToArray());

            }
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
            chart.ColorProfile = profile;
            chart.ResolutionX = new TimeSpan(0, 0, 10);
            chart.ResolutionY = 0.1;
        }

        private async void Window_SizeChanged(object sender, WindowSizeChangedEventArgs args)
        {
            if (points.Count > 0)
            {
                List<Serie> series = new();
                await chart.CreateLineChart(series.ToArray());
            }
        }
    }


}
