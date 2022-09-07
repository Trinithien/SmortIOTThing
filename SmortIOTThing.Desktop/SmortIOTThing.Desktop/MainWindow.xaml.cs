using Microsoft.UI.Xaml;
using SmortIOTThing.Desktop.Interfaces;
using System.Threading.Tasks;
using System.Threading;
using System;
using Windows.Devices.Geolocation;

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
        public MainWindow(IRequestManager requestManager)
        {
            _requestManager = requestManager;
            this.InitializeComponent();
            WelcomeMessage.Text = _requestManager.GetWelcomeMessage();
        }

        private void UpdateStatus(object sender, object e)
        {
            TemperatureStatus.Text = _requestManager.GetTemperatureStatus();
        }
        
    }
    
    
}
