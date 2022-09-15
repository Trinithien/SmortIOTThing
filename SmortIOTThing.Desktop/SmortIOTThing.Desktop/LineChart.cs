using Microsoft.UI;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using SmortIOTThing.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Radios;
using Windows.Foundation;
using Windows.UI;

namespace SmortIOTThing.Desktop
{
    public class LineChart
    {
        double chartWidth = 0;
        double chartHeight = 0;
        double deltaY = 0;
        TimeSpan deltaX;
        double segmentsY = 0;
        double segmentsX;
        DateTimeOffset minX;
        double minY;
        DateTimeOffset maxX;
        double maxY;

        private void InitChart(ChartPoint[] points,TimeSpan resolutionX, double resolutionY, double width,double height, Brush textForeground, out double[] values, out TextBlock[] blocksX,out TextBlock[] blocksY)
        {
            chartWidth = width;
            chartHeight = height;
            values = points.Select(goal => goal.Value).ToArray();
            minY = values.Min();
            maxY = values.Max();
            deltaY = (maxY - minY);
            segmentsY = deltaY / resolutionY;
            List<TextBlock> blockList = new();
            for (int i = 0; i < segmentsY; i++)
            {
                blockList.Add(new TextBlock {Text = (minY+i*deltaY/segmentsY).ToString(),Foreground =  textForeground });
            }
            blocksY = blockList.ToArray();

            var times = points.Select(goal => goal.TimeStamp).ToList();
            blockList = new();
            minX = times.Min();
            maxX = times.Max();
            deltaX = (maxX - minX);
            segmentsX = deltaX / resolutionX;
            for (int i = 0; i < segmentsX; i++)
            {
                blockList.Add(new TextBlock {Text = (minX+i*deltaX/segmentsX).ToString(),Foreground =  textForeground });
            }
            blocksX = blockList.ToArray();
        }

        private Shape CreateBorder(double width, double height, Color borderColor)
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(0,0));
            points.Add(new Point(width,0));
            points.Add(new Point(width,height));
            points.Add(new Point(0,height));
            points.Add(new Point(0,0));
            


            var border = new Polyline();
            border.Stroke = new SolidColorBrush(borderColor);
            border.StrokeThickness = 5;
            border.Opacity = 1;
            var pointsPolygon = new PointCollection();
            points.ForEach(point => pointsPolygon.Add(point));

            border.Points = pointsPolygon;
            return border;
        }

        private Shape CreateBackground(double width, double height, Color backgroundColor)
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(0,0));
            points.Add(new Point(width,0));
            points.Add(new Point(width,height));
            points.Add(new Point(0,height));
            points.Add(new Point(0,0));
            


            var background = new Polygon();
            background.Fill = new SolidColorBrush(backgroundColor);
            background.Opacity = 1;
            var pointsPolygon = new PointCollection();
            points.ForEach(point => pointsPolygon.Add(point));

            background.Points = pointsPolygon;
            return background;
        }
        private Shape CreateLine(Point a,Point b,Color lineColor)
        {
            List<Point> points = new List<Point> {a,b};
            var line = new Polyline();
            line.Stroke = new SolidColorBrush(lineColor);
            line.StrokeThickness = 2;
            line.Opacity = 1;
            var pointsPolygon = new PointCollection();
            points.ForEach(point => pointsPolygon.Add(point));

            line.Points = pointsPolygon;
            return line;

        }
        private Shape[] CreateSegments(double width,double height,double lineLenght, Color lineColor)
        {
            var shapes = new List<Shape>();
            var segmentLenghtX = width / segmentsX;
            var halfLineLength = lineLenght / 2;
            for (int i = 1; i < segmentsX; i++)
            {
                shapes.Add(CreateLine(new Point((float)segmentLenghtX*i,height+ halfLineLength), new Point((float)segmentLenghtX*i,height- halfLineLength),lineColor));
            }
            var segmentLengthY = height / segmentsY;
            for (int i = 1; i < segmentsY; i++)
            {
                shapes.Add(CreateLine(new Point((float)halfLineLength,segmentLengthY*i),new Point((float)-halfLineLength,segmentLengthY*i),lineColor));
            }
            return shapes.ToArray();
        }

        private Shape CreateMain(ChartPoint[] points,double width,double height,Color lineColor)
        {
            var pointsList = new List<Point>();

            foreach (var point in points)
            {
                var relativeValueY = point.Value - minY;
                var percentileY = relativeValueY / deltaY;

                var relativeValueX = point.TimeStamp - minX;
                var percentileX = relativeValueX / deltaX;

                pointsList.Add(new Point((float)(percentileX*width),(float)(percentileY*height)));
            }


            var line = new Polyline();
            line.Stroke = new SolidColorBrush(lineColor);
            line.StrokeThickness = 5;
            line.Opacity = 1;
            var pointsPolygon = new PointCollection();
            pointsList.ForEach(point => pointsPolygon.Add(point));

            line.Points = pointsPolygon;
            return line;
        }

        public async Task CreateLineChart(Grid root,ChartPoint[] points, TimeSpan resolutionX, double resolutionY, double width, double height , Color backlineColor, Color outline, Color lineColor,Color background, Brush textForeground)
        {
            InitChart(points,resolutionX,resolutionY,width,height, textForeground, out var values, out var blocksX,out var blocksY);

            List<Shape> shapes = new List<Shape>();

            shapes.Add(CreateBackground(width, height, background));
            shapes.Add(CreateBorder(width, height, outline));
            foreach (var shape in CreateSegments(width,height,20,backlineColor))
            {
                shapes.Add(shape);
            }
            shapes.Add(CreateMain(points,width,height,lineColor));
            root.Children.Clear();
            shapes.ForEach(shape => root.Children.Add(shape));
        }

    }
}
