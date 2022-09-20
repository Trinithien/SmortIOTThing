using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace DjupvikCharts
{
    public class Serie
    {
        public string Name { get; set; }
        public LinePoint[] SeriesPoints { get; set; }
        public Color Color { get; set; }

    }
}
