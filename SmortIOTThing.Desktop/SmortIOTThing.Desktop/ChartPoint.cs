using System;

namespace DjupvikCharts
{
    public class SpiderPoint
    {
        public double Value { get; set; }
        public string Text { get; set; }
    }
    public class LinePoint
    {
        public double Value { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Text { get; set; }
    }
}
