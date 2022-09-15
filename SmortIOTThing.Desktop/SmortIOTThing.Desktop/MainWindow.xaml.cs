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
        public MainWindow(IRequestManager requestManager,ITemperatureSensorStatus temperatureSensorStatus)
        {
            _requestManager = requestManager;
            temperatureSensorStatus.StatusChanged += UpdateStatus;
            this.InitializeComponent();
            WelcomeMessage.Text = _requestManager.GetWelcomeMessage();

        }

        private async void UpdateStatus(object sender, object e)
        {
            TemperatureStatus.Text = _requestManager.GetTemperatureStatus(); var lineChart = new LineChart();
            var points = new List<ChartPoint>();
            Random random = new Random();
            for (int i = 0; i < 100; i++)
            {
                points.Add(new ChartPoint { Value = random.NextDouble() * 10 + 15, TimeStamp = DateTimeOffset.Now.AddSeconds(i) });
            }
            root.Measure(new Windows.Foundation.Size(0,0));
            await lineChart.CreateLineChart(root, points.ToArray(), new TimeSpan(0, 0, 10), 2, root.ActualWidth, root.ActualHeight, backlineColor: Colors.White, outline: Colors.Gray, lineColor: Colors.Purple,background: Colors.Pink, new SolidColorBrush(Colors.Gold));

        }

    }
    
    
}
