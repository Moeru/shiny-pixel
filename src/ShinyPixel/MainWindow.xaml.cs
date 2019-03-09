using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ShinyPixel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Line> Lines { get; set; }
        private Canvas _canvas;

        private byte AverageColorBytes(byte first, byte second, double blend)
        {
            if ((blend > 1.0) || (blend < 0.0))
            {
                throw new ArgumentException();
            }

            double a = Convert.ToDouble(first);
            double b = Convert.ToDouble(second);

            return Convert.ToByte(Math.Sqrt((a * a) * blend + (b * b) * (1.0 - blend)));
        }

        private Color Blend(Color first, Color second, double blend)
        {
            return new Color
            {
                R = AverageColorBytes(first.R, second.R, blend),
                G = AverageColorBytes(first.G, second.G, blend),
                B = AverageColorBytes(first.B, second.B, blend),
                A = AverageColorBytes(first.A, second.A, blend)
            };

        }

        public MainWindow()
        {
            InitializeComponent();

            _canvas = new Canvas()
            {
                Name = "MyCanvas",
                Background = new SolidColorBrush(Colors.White)
            };

            var gradientBrush = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0.5),
                EndPoint = new Point(1, 0.5)
            };

            gradientBrush.GradientStops.Add(new GradientStop(Colors.Red, 0.0));
            gradientBrush.GradientStops.Add(new GradientStop(Colors.Green, 1.0));


            var secondGradientBrush = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0.5),
                EndPoint = new Point(1, 0.5)
            };

            secondGradientBrush.GradientStops.Add(new GradientStop(Colors.Red, 0.0));
            secondGradientBrush.GradientStops.Add(new GradientStop(Colors.Green, 1.0));

            var steps = 10;
            for (int i = 1; i <= steps; i++)
            {
                var progress = i / (steps + 1.0);
                var blend = 1.0 - progress;

                secondGradientBrush.GradientStops.Add(
                    new GradientStop(
                        Blend(Colors.Red, Colors.Green, blend),
                        progress));
            }

            var rectangle = new Rectangle()
            {
                Width = 800,
                Height = 200,
                Fill = gradientBrush
            };

            var secondRectangle = new Rectangle()
            {
                Width = 800,
                Height = 200,
                Fill = secondGradientBrush
            };

            _canvas.Children.Add(rectangle);
            _canvas.Children.Add(secondRectangle);
            Canvas.SetTop(secondRectangle, 200);

            _canvas.AddHandler(MouseDownEvent, new MouseButtonEventHandler(Canvas_MouseDown));

            Content = _canvas;
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(this);

            _canvas.Children.Add(new Line()
            {
                X1 = 0,
                Y1 = 0,
                X2 = position.X,
                Y2 = position.Y,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 4.0
            });
        }
    }
}
