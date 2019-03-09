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

        public MainWindow()
        {
            InitializeComponent();

            _canvas = new Canvas()
            {
                Name = "MyCanvas",
                Background = new SolidColorBrush(Colors.Green)
            };

            var line = new Line()
            {
                X1 = 600,
                Y1 = 200,
                X2 = 30,
                Y2 = 30,
                Stroke = new SolidColorBrush(Colors.Black),
                StrokeThickness = 4.0
            };

            _canvas.Children.Add(line);

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
