using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI;
using Microsoft.UI;

namespace DjupvikCharts
{
    public class LineChart
    {
        public Panel Root { get; set; }
        public ChartColorProfile ColorProfile { get; set; } = 
            new ChartColorProfile 
            { 
                Background = Colors.White, 
                BacklineColor = Colors.LightGray, 
                Outline = Colors.Gray,
                TextForeground = new SolidColorBrush(Colors.LightGray),
                TextForegroundAlarm = new SolidColorBrush(Colors.MediumVioletRed)
            };
        public TimeSpan ResolutionX { get; set; } = new TimeSpan(0,0,25);
        public double ResolutionY { get; set; } = 1;
        double deltaY = 0;
        TimeSpan deltaX;
        double segmentsY = 0;
        double segmentsX;
        DateTimeOffset _minX;
        double _minY;
        DateTimeOffset _maxX;
        double _maxY;
        double padding = 0.2;
        double paddingWidth;
        double paddingHeight;
        double strokeThickness;
        private Shape MakeBounds()
        {
            List<Point> points = new List<Point>();

            points.Add(new Point(1000000, 1000000));
            points.Add(new Point(-1000000, -1000000));


            var polyline1 = new Polyline();
            polyline1.StrokeThickness = 1;
            polyline1.Opacity = 0;
            var pointsPolygon = new PointCollection();
            points.ForEach(point => pointsPolygon.Add(point));
            pointsPolygon.Add(points[0]);

            polyline1.Points = pointsPolygon;
            return polyline1;
        }


        private void InitChart(Serie[] series, double width,double height, Brush textForeground, out TextBlock[] blocksX,out TextBlock[] blocksY)
        {
            strokeThickness = Math.Sqrt(width * height) / 120;
            paddingWidth = 60;
            paddingHeight = 30;
            _maxX = DateTimeOffset.MinValue;
            _minX = DateTimeOffset.MaxValue;
            _maxY = double.MinValue;
            _minY = double.MaxValue;
            foreach (var serie in series)
            {
                var values = serie.SeriesPoints.Select(goal => goal.Value).ToArray();
                var minY = values.Min();
                if (_minY > minY) _minY = minY;
                var maxY = values.Max();
                if (_maxY < maxY) _maxY = maxY;
                var times = serie.SeriesPoints.Select(goal => goal.Timestamp).ToList();
                var minX = times.Min();
                if(_minX > minX)_minX = minX;
                var maxX = times.Max();
                if(_maxX < maxX) _maxX = maxX;
            }
            deltaY = (_maxY - _minY);
            segmentsY = deltaY / ResolutionY;
            List<TextBlock> blockList = new();

            for (int i = 0; i <= segmentsY; i++)
            {
                blockList.Add(new TextBlock {Text = (_minY+i*deltaY/segmentsY).ToString(),Foreground =  textForeground });
            }
            blocksY = blockList.ToArray();

            
            blockList = new();
            deltaX = (_maxX - _minX);
            segmentsX = deltaX / ResolutionX;
            for (int i = 0; i <= segmentsX; i++)
            {
                blockList.Add(new TextBlock {Text = (_minX+i*deltaX/segmentsX).ToString(),Foreground =  textForeground });
            }
            blocksX = blockList.ToArray();
        }

        private Shape CreateBorder(double width, double height, Color borderColor)
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(paddingWidth, paddingHeight));
            points.Add(new Point(width - paddingWidth, paddingHeight));
            points.Add(new Point(width - paddingWidth, height - paddingHeight));
            points.Add(new Point(paddingWidth, height - paddingHeight));
            points.Add(new Point(paddingWidth, paddingHeight));


            var border = new Polyline();
            border.Stroke = new SolidColorBrush(borderColor);
            border.StrokeThickness = strokeThickness;
            border.Opacity = 1;
            var pointsPolygon = new PointCollection();
            points.ForEach(point => pointsPolygon.Add(point));

            border.Points = pointsPolygon;
            return border;
        }

        private Shape CreateBackground(double width, double height, Color backgroundColor)
        {
            List<Point> points = new List<Point>();
            points.Add(new Point(paddingWidth, paddingHeight));
            points.Add(new Point(width- paddingWidth, paddingHeight));
            points.Add(new Point(width- paddingWidth, height-paddingHeight));
            points.Add(new Point(paddingWidth, height-paddingHeight));
            points.Add(new Point(paddingWidth, paddingHeight));
            


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
            line.StrokeThickness = strokeThickness*2/5;
            line.Opacity = 1;
            var pointsPolygon = new PointCollection();
            points.ForEach(point => pointsPolygon.Add(point));

            line.Points = pointsPolygon;
            return line;

        }
        private async Task<Shape[]> CreateSegments(double width,double height,double lineLenght, Color lineColor, TextBlock[] blocksX, TextBlock[] blocksY,string textFormat)
        {
            var shapes = new List<Shape>();
            var segmentLenghtX = (width-2*paddingWidth) / segmentsX;
            var halfLineLength = lineLenght / 2;
            for (int i = 0; i < segmentsX; i++)
            {
                var lowerPoint = new Point((float)segmentLenghtX * i + paddingWidth, -paddingHeight + height + halfLineLength);
                var higherPoint = new Point((float)segmentLenghtX * i + paddingWidth,-paddingHeight + height - halfLineLength);
                shapes.Add(CreateLine(lowerPoint, higherPoint, lineColor));
                blocksX[i].Translation = new Vector3 { X = lowerPoint._x, Y = lowerPoint._y };
                blocksX[i].Text = $"{(_minX + i*deltaX/segmentsX).ToString(textFormat)}";
            }
            var segmentLengthY = ((height -2*paddingHeight)*(1-2*padding)) / segmentsY;
            for (int i = 0; i < blocksY.Length; i++)
            {
                var leftPoint = new Point((float)-halfLineLength + paddingWidth, -paddingHeight - padding* (height - 2 * paddingHeight) + height - segmentLengthY * i);
                var rightPoint = new Point((float)halfLineLength + paddingWidth, -paddingHeight - padding* (height - 2 * paddingHeight) + height - segmentLengthY * i);
                shapes.Add(CreateLine(leftPoint,rightPoint,lineColor));
                blocksY[i].Text = $"{Math.Round(_minY+i*deltaY/segmentsY,2)}°C";
                blocksY[i].Measure(new Size(0,0));
                await Task.Delay(10);
                blocksY[i].Translation = new Vector3 { X = leftPoint._x - (float)blocksY[i].ActualWidth, Y = leftPoint._y - (float)blocksY[i].ActualHeight/2 };
            }
            return shapes.ToArray();
        }

        private Shape CreateMain(LinePoint[] points,double width,double height,Color lineColor)
        {
            var pointsList = new List<Point>();

            foreach (var point in points)
            {
                var relativeValueY = _maxY - point.Value;
                var percentileY = relativeValueY / deltaY;
                percentileY *= (1 - 2 * paddingHeight/height);
                percentileY += paddingHeight / height;
                percentileY *= (1 - 2 * padding);
                percentileY += padding;
                var relativeValueX = point.Timestamp - _minX;
                var percentileX = relativeValueX / deltaX;
                percentileX *= 1-2*(paddingWidth / width);
                percentileX += paddingWidth / width;
                pointsList.Add(new Point((float)(percentileX*width),(float)(percentileY*height)));
            }


            var line = new Polyline();
            line.Stroke = new SolidColorBrush(lineColor);
            line.StrokeThickness = strokeThickness;
            line.Opacity = 1;
            var pointsPolygon = new PointCollection();
            pointsList.ForEach(point => pointsPolygon.Add(point));

            line.Points = pointsPolygon;
            return line;
        }

        public async Task CreateLineChart(Serie[] series,string textFormat = "hh:mm:ss")
        {
            if (Root == null)
            {
                throw new("No Root supplied");
            }
            Root.Children.Add(MakeBounds());
            Root.Measure(new Size(0, 0));
            double width = Root.ActualWidth, height = Root.ActualHeight;
            InitChart(series,width,height, ColorProfile.TextForeground, out var blocksX,out var blocksY);

            var shapes = new List<UIElement>();
            foreach (var block in blocksX)
            {
                shapes.Add(block);
            }
            foreach (var block in blocksY)
            {
                shapes.Add(block);
            }

            shapes.Add(CreateBackground(width, height, ColorProfile.Background));
            foreach (var shape in await CreateSegments(width,height,20, ColorProfile.BacklineColor, blocksX,blocksY,textFormat))
            {
                shapes.Add(shape);
            }
            foreach (var serie in series)
            {
                shapes.Add(CreateMain(serie.SeriesPoints,width,height,serie.Color));
            }
            shapes.Add(CreateBorder(width, height, ColorProfile.Outline));
            Root.Children.Clear();
            shapes.ForEach(shape => Root.Children.Add(shape));
        }

    }
}
