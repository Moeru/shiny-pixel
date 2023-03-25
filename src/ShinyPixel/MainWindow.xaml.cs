using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private double _top;


        private struct HsvColor
        {
            int H { get; set; }
            int S { get; set; }
            int V { get; set; }



            public static HsvColor FromColor(Color color)
            {
                var hsv = new HsvColor();

                var min = Math.Min(color.R, Math.Min(color.G, color.B));
                var max = Math.Max(color.R, Math.Max(color.G, color.B));
                var range = max - min;

                // Hue
                if (range == 0)
                {
                    hsv.H = 0;
                }
                else if (max == color.R)
                {
                    hsv.H = ((color.G - color.B) / range) % 6;
                }
                else if (max == color.G)
                {
                    hsv.H = ((color.B - color.R) / range) + 2;
                }
                else
                {

                }




                return hsv;
            }

            public Color ToColor()
            {
                return Colors.White;
            }

        }



        private Color Blend(Color first, Color second, float blend)
        {
            if ((blend > 1.0) || (blend < 0.0))
            {
                throw new ArgumentException();
            }

            //return first * blend + second * (1.0f - blend);

            return new Color
            {
                A = Convert.ToByte(first.A * blend + second.A * (1.0f - blend)),
                R = Convert.ToByte(first.R * blend + second.R * (1.0f - blend)),
                G = Convert.ToByte(first.G * blend + second.G * (1.0f - blend)),
                B = Convert.ToByte(first.B * blend + second.B * (1.0f - blend)),
            };
        }

        private Color Brightness(Color color, ColorInterpolationMode colorInterpolationMode)
        {
            if (colorInterpolationMode == ColorInterpolationMode.ScRgbLinearInterpolation)
            {
                var gammaBrightness = (color.ScR + color.ScG + color.ScB) / 3;
                return Color.FromScRgb(color.ScA, gammaBrightness, gammaBrightness, gammaBrightness);
            }

            var brightness = Convert.ToByte((color.R + color.G + color.B) / 3);
            return Color.FromArgb(color.A, brightness, brightness, brightness);
        }

        private IReadOnlyList<int> addGradient(double width, double height, IReadOnlyList<Color> colors)
        {
            List<int> elements = new List<int>();

            var rectangleWidth = width / colors.Count;

            for (int i = 0; i < colors.Count; i++)
            {
                var rectangle = new Rectangle()
                {
                    Width = rectangleWidth,
                    Height = height,
                    Fill = new SolidColorBrush(colors[i])
                };

                elements.Add(_canvas.Children.Add(rectangle));
                Canvas.SetTop(rectangle, _top);
                Canvas.SetLeft(rectangle, i * rectangleWidth);
            }
            _top += height;

            return elements;
        }


        public MainWindow()
        {
            InitializeComponent();

            _canvas = new Canvas()
            {
                Name = "MyCanvas",
                Background = new SolidColorBrush(Colors.White)
            };

            _top = 0;

            var standardGradient = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0.5),
                EndPoint = new Point(1, 0.5),
                ColorInterpolationMode = ColorInterpolationMode.ScRgbLinearInterpolation
            };

            var start = new Color { R = 1, G = 127, B = 127, A = 255 };
            var middle = new Color { R = 255, G = 0, B = 0, A = 255 };
            var end = new Color { R = 0, G = 255, B = 0, A = 255 };

            standardGradient.GradientStops.Add(new GradientStop(start, 0.0));
            standardGradient.GradientStops.Add(new GradientStop(middle, 0.5));
            standardGradient.GradientStops.Add(new GradientStop(end, 1.0));

            var standardGradientRectangle = new Rectangle()
            {
                Width = 540,
                Height = 50,
                Fill = standardGradient
            };
            _canvas.Children.Add(standardGradientRectangle);
            _top += 50;

            var gradient = new List<Color>();

            var steps = 270;
            for (int i = 0; i < steps; i++)
            {
                var progress = i / (steps - 1.0f);
                var blend = 1.0f - progress;

                gradient.Add(Blend(start, middle, blend));
            }
            for (int i = 0; i < steps; i++)
            {
                var progress = i / (steps - 1.0f);
                var blend = 1.0f - progress;

                gradient.Add(Blend(middle, end, blend));
            }

            addGradient(540, 50, gradient);

            var brightness = gradient.Select(c => Brightness(c, ColorInterpolationMode.SRgbLinearInterpolation)).ToList();
            addGradient(540, 50, brightness);

            var gammaBrightness = gradient.Select(c => Brightness(c, ColorInterpolationMode.ScRgbLinearInterpolation)).ToList();
            addGradient(540, 50, gammaBrightness);

            /*
            var gamaBasedGradient = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0.5),
                EndPoint = new Point(1, 0.5)
            };

            gamaBasedGradient.GradientStops.Add(new GradientStop(Colors.Red, 0.0));
            gamaBasedGradient.GradientStops.Add(new GradientStop(Colors.Green, 1.0));

            var brightnessGradient = new LinearGradientBrush()
            {
                StartPoint = new Point(0, 0.5),
                EndPoint = new Point(1, 0.5)
            };

            var green = new Color { R = 0, G = 255, B = 0, A = 255 }; // Colors.Green?
            brightnessGradient.GradientStops.Add(new GradientStop(ToBrightness(Colors.Red), 0.0));
            brightnessGradient.GradientStops.Add(new GradientStop(ToBrightness(green), 1.0));


            steps = 10;
            for (int i = 1; i <= steps; i++)
            {
                var progress = i / (steps + 20.0);
                var blend = 1.0 - progress;

                var blendedColor = Blend(Colors.Red, green, blend);

                gamaBasedGradient.GradientStops.Add(new GradientStop(blendedColor, progress));
                brightnessGradient.GradientStops.Add(new GradientStop(ToBrightness(blendedColor), progress));
            }

            var gamaBasedRectangle = new Rectangle()
            {
                Width = 800,
                Height = 100,
                Fill = gamaBasedGradient
            };

            _canvas.Children.Add(gamaBasedRectangle);
            Canvas.SetTop(gamaBasedRectangle, 200);

            var brightnessRectangle = new Rectangle()
            {
                Width = 800,
                Height = 100,
                Fill = brightnessGradient
            };

            _canvas.Children.Add(brightnessRectangle);
            Canvas.SetTop(brightnessRectangle, 300);

            var alphaBasedRectangle = new Rectangle()
            {
                Width = 800,
                Height = 200,
                Fill = new SolidColorBrush(Colors.Red)
            };

            _canvas.Children.Add(alphaBasedRectangle);
            Canvas.SetTop(alphaBasedRectangle, 400);


            for (int i = 1; i <= steps; i++)
            {
                var xmin = i * 80;
                var alpha = 255 * ((i * 1.0) / steps);

                var alphaColor = Colors.Green;
                alphaColor.A = Convert.ToByte(alpha);

                alphaBasedRectangle = new Rectangle()
                {
                    Width = 80,
                    Height = 200,
                    Fill = new SolidColorBrush(alphaColor)
                };

                _canvas.Children.Add(alphaBasedRectangle);
                Canvas.SetTop(alphaBasedRectangle, 400);
                Canvas.SetLeft(alphaBasedRectangle, xmin);
            }
            */


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
