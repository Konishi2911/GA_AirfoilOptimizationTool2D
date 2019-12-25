using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GA_AirfoilOptimizationTool2D.Chart
{
    /// <summary>
    /// LineChart.xaml の相互作用ロジック
    /// </summary>
    public partial class LineChart : UserControl
    {
        private const double xMargine = 10;
        private const double yMargine = 10;

        static LineChart()
        {
            PointsProperty = DependencyProperty.Register
                (
                "PointsCollection",
                typeof(Point[]),
                typeof(LineChart),
                new FrameworkPropertyMetadata(null, PointsArrayChanged)
                );

            ChartTitleProperty = DependencyProperty.Register
                (
                "Title",
                typeof(String),
                typeof(LineChart),
                new FrameworkPropertyMetadata("Title", ChartTitleChanged)
                );

            XAxisProperty = DependencyProperty.Register
                (
                "XAxis",
                typeof(AxisStyle),
                typeof(LineChart),
                new FrameworkPropertyMetadata(new AxisStyle())
                );

            YAxisProperty = DependencyProperty.Register
                (
                "YAxis",
                typeof(AxisStyle),
                typeof(LineChart),
                new FrameworkPropertyMetadata(new AxisStyle())
                );
        }

        public LineChart()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PointsProperty;
        public static readonly DependencyProperty ChartTitleProperty;
        public static readonly DependencyProperty XAxisProperty;
        public static readonly DependencyProperty YAxisProperty;

        public Point[] PointsCollection
        {
            get => (Point[])GetValue(PointsProperty);
            set => SetValue(PointsProperty, value);
        }

        public String ChartTitle
        {
            get => (String)GetValue(ChartTitleProperty);
            set => SetValue(ChartTitleProperty, value);
        }

        public AxisStyle XAxis
        {
            get => (AxisStyle)GetValue(XAxisProperty);
            set => SetValue(XAxisProperty, value);
        }

        public AxisStyle YAxis
        {
            get => (AxisStyle)GetValue(YAxisProperty);
            set => SetValue(YAxisProperty, value);
        }

        private static void PointsArrayChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            LineChart lchart = obj as LineChart;
            if (lchart != null)
            {
                lchart.CharacLine.Points.Clear();
                lchart.DrawChart();
            }
        }

        private static void ChartTitleChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            LineChart lchart = obj as LineChart;
            if (lchart != null)
            {
                lchart.ChartTitleText.Text = lchart.ChartTitle;
            }
        }

        private void DrawAxisLine()
        {
            XAxisLine.X1 = xMargine;
            XAxisLine.X2 = ChartWindow.ActualWidth - xMargine;
            XAxisLine.Y1 = XAxisLine.Y2 = ChartWindow.ActualHeight - yMargine;

            YAxisLine.X1 = YAxisLine.X2 = xMargine;
            YAxisLine.Y1 = yMargine;
            YAxisLine.Y2 = ChartWindow.ActualHeight - yMargine;
        }

        private bool DrawChart()
        {
            if (PointsCollection == null)
            {
                return false;
            }

            double xMagnification = (ChartWindow.ActualWidth - 2 * xMargine) / XAxis.Width;
            double yMagnifidation = (ChartWindow.ActualHeight - 2 * yMargine) / YAxis.Width;

            foreach (var p in PointsCollection)
            {
                double x = xMargine + xMagnification * p.X;
                double y = ChartWindow.ActualHeight - yMargine - yMagnifidation * p.Y;

                CharacLine.Points.Add(new Point(x, y));
            }

            return true;
        }

        private void ChartWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawAxisLine();
            DrawChart();
        }
    }
}
