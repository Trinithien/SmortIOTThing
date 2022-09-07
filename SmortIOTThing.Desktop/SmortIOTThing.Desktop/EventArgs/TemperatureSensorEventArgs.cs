using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmortIOTThing.Desktop.EventArguements
{
    public class TemperatureSensorEventArgs : EventArgs
    {
        public string[] Names { get; set; }
    }
}
