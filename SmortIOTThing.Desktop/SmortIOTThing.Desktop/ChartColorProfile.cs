using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace DjupvikCharts
{
    public class ChartColorProfile
    {
        public Color BacklineColor { get; set; }
        public Color Outline { get; set; }
        public Color Background { get; set; }

        public Brush TextForeground { get; set; }
        public Brush TextForegroundAlarm { get; set; }
    }
}
