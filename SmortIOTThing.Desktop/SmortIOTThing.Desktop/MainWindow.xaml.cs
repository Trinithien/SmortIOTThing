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
            Random random = new Random();
            for (int i = 0; i < 10; i++)
            {
                points.Add(new ChartPoint { Value = 15 + i, TimeStamp = DateTimeOffset.Now.AddSeconds(i) });
            }
            for (int i = 10; i < 100; i++)
            {
                points.Add(new ChartPoint { Value = random.NextDouble() * 10 + 15, TimeStamp = DateTimeOffset.Now.AddSeconds(i) });
            }
        }

        List<ChartPoint> points = new List<ChartPoint>();
        private async void UpdateStatus(object sender, object e)
        {
            TemperatureStatus.Text = _requestManager.GetTemperatureStatus();
            Random random = new Random();
            points.Clear();
            points.Add(new ChartPoint { Value = 20, TimeStamp = DateTimeOffset.Now });

            for (int i = 1; i < 100; i++)
            {
                points.Add(new ChartPoint { Value = points[i - 1].Value + (random.NextDouble() - 0.5) / 10, TimeStamp = DateTimeOffset.Now.AddSeconds(i) });
            }

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
        private async void Window_SizeChanged(object sender, WindowSizeChangedEventArgs args)
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
