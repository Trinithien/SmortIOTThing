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
            allPoints.Add(new ChartPoint { Value = 20, TimeStamp = DateTimeOffset.Now.AddSeconds(-100) });
            Random random = new Random();
            for (int i = 1; i < 100; i++)
            {
                allPoints.Add(new ChartPoint { Value = allPoints[^1].Value + (random.NextDouble() - 0.5) / 10, TimeStamp = DateTimeOffset.Now.AddSeconds(i-100) });

            }
        }

        List<ChartPoint> points = new List<ChartPoint>();
        List<ChartPoint> allPoints = new();
        private async void UpdateStatus(object sender, object e)
        {
            TemperatureStatus.Text = _requestManager.GetTemperatureStatus();
            
            if (allPoints.Count <= 100) 
            {
                if (allPoints.Count < 1)
                    points.Add(new ChartPoint { Value = 20, TimeStamp = DateTimeOffset.Now });
                points = allPoints;
            }
            else
            {
                points = allPoints.GetRange(allPoints.Count - 101, 100);
            }
            Random random = new Random();
            allPoints.Add(new ChartPoint { Value = allPoints[^1].Value + (random.NextDouble() - 0.5) / 10, TimeStamp = DateTimeOffset.Now });


            if (points.Count > 0)
            {
                var chart = new LineChart();
                await chart.CreateLineChart(root,
                            points.ToArray(),
                            resolutionX: new TimeSpan(0, 0, 10),
                            resolutionY: 0.1,
                            backlineColor: Colors.White,
                            outline: Colors.Gray,
                            lineColor: Colors.Purple,
                            background: Colors.Pink,
                            new SolidColorBrush(Colors.Gold));

            }
        }
        private async void Window_SizeChanged(object sender, WindowSizeChangedEventArgs args)
        {
            if (points.Count > 0)
            {
                var chart = new LineChart();
                await chart.CreateLineChart(root,
                    points.ToArray(),
                    resolutionX: new TimeSpan(0, 0, 10),
                    resolutionY: 0.1,
                    backlineColor: Colors.White,
                    outline: Colors.Gray,
                    lineColor: Colors.Purple,
                    background: Colors.Pink,
                    new SolidColorBrush(Colors.Gold));
            }
        }
    }


}
