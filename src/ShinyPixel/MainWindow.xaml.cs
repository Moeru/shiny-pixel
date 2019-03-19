// Copyright (c) 2019 Moeru. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShinyPixel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //public ColorSelection _colorSelection;
        //private RectangleGeometry[,] _renderRectangles;
        //private GeometryGroup _geometryGroupA;
        //private GeometryGroup _geometryGroupB;

        //private DrawingGroup _drawingGroup;
        //private DrawingVisual _backgroundDrawingVisual;
        //private Image _imageControl;
        //private SolidColorBrush _brush;
        //private SolidColorBrush _brush2;
        //private RenderTargetBitmap _renderTargetBitmap;

        //public class ColorSelection : INotifyPropertyChanged
        //{
        //    public event PropertyChangedEventHandler PropertyChanged;

        //    protected void OnPropertyChanged(string propertyName)
        //    {
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    }

        //    private byte _red;
        //    private byte _green;
        //    private byte _blue;
        //    private int _zoom;

        //    public byte Red
        //    {
        //        get { return _red; }
        //        set { _red = value; OnPropertyChanged(nameof(Red)); }
        //    }
        //    public byte Green
        //    {
        //        get { return _green; }
        //        set { _green = value; OnPropertyChanged(nameof(Green)); }
        //    }
        //    public byte Blue
        //    {
        //        get { return _blue; }
        //        set { _blue = value; OnPropertyChanged(nameof(Blue)); }
        //    }
        //    public int Zoom
        //    {
        //        get { return _zoom; }
        //        set { _zoom = value; OnPropertyChanged(nameof(Zoom)); }
        //    }
        //}

        public MainWindow()
        {
            InitializeComponent();

            //_colorSelection = new ColorSelection
            //{
            //    Red = 0,
            //    Green = 10,
            //    Blue = 50,
            //    Zoom = 5
            //};

            //this.DataContext = _colorSelection;

            //_colorSelection.PropertyChanged += new PropertyChangedEventHandler(ColorSelection_PropertyChanged);

            /*
            _renderRectangles = new RectangleGeometry[100, 100];

            _geometryGroupA = new GeometryGroup();
            _geometryGroupB = new GeometryGroup();

            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {
                    _renderRectangles[i, j] = new RectangleGeometry(
                        new Rect(5 * i, 5 * j, 5, 5));

                    _geometryGroupA.Children.Add(_renderRectangles[i, j]);
                }
            }

            _drawingGroup = new DrawingGroup();

            _brush = new SolidColorBrush(Colors.Black);
            var geometryDrawingA = new GeometryDrawing();
            geometryDrawingA.Geometry = _geometryGroupA;
            geometryDrawingA.Brush = _brush;
            _drawingGroup.Children.Add(geometryDrawingA);

            _brush2 = new SolidColorBrush(Colors.Red);
            var geometryDrawingB = new GeometryDrawing();
            geometryDrawingB.Geometry = _geometryGroupB;
            geometryDrawingB.Brush = _brush2;
            _drawingGroup.Children.Add(geometryDrawingB);


            _backgroundDrawingVisual = new DrawingVisual();

            using (var drawingContext = _backgroundDrawingVisual.RenderOpen())
            {
                drawingContext.DrawDrawing(_drawingGroup);
            }

            _renderTargetBitmap = new RenderTargetBitmap(500, 500, 96, 96, PixelFormats.Pbgra32);
            _renderTargetBitmap.Render(_backgroundDrawingVisual);

            _imageControl = new Image()
            {
                Stretch = Stretch.None,
                Source = _renderTargetBitmap
            };

            var dockPanel = Content as DependencyObject;
            var stackPanel = FindControl<StackPanel>(dockPanel, "MainStackPanel");
            var grid = FindControl<Grid>(stackPanel, "MainWindowGrid");
            grid.Children.Add(_imageControl);

            _imageControl.AddHandler(MouseDownEvent, new MouseButtonEventHandler(Image_MouseDown));
            */
        }

        private void UpdateDrawing()
        {
            //var drawingVisual = (_colorSelection.Zoom == 0)
            //    ? _backgroundDrawingVisual
            //    : new DrawingVisual();

            //using (var drawingContext = drawingVisual.RenderOpen())
            //{
            //    drawingContext.DrawDrawing(_drawingGroup);
            //}

            //_renderTargetBitmap.Clear();
            //_renderTargetBitmap.Render(drawingVisual);
        }

        private void ColorSelection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //switch(e.PropertyName)
            //{
                //case nameof(ColorSelection.Red):
                //case nameof(ColorSelection.Green):
                //case nameof(ColorSelection.Blue):
                    //_brush2.Color = Color.FromRgb(
                    //    _colorSelection.Red,
                    //    _colorSelection.Green,
                    //    _colorSelection.Blue);
            //        UpdateDrawing();
            //        break;
            //    case nameof(ColorSelection.Zoom):
            //        UpdateDrawing();
            //        break;
            //    default:
            //        break;
            //}
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //var position = e.GetPosition(_imageControl);

            //var column = Convert.ToInt32(position.X) / 5;
            //var row = Convert.ToInt32(position.Y) / 5;

            //_geometryGroupA.Children.Remove(_renderRectangles[column, row]);
            //_geometryGroupB.Children.Add(_renderRectangles[column, row]);

            UpdateDrawing();
        }

        private void MenuNew_Click(object sender, RoutedEventArgs e)
        {
            //var dialog = new OpenFileDialog();

            //dialog.DefaultExt = ".bmp";
            //dialog.Filter = "Bitmap Files (*.bmp)|*.bmp";

            //var result = dialog.ShowDialog();

            //if (!result.HasValue)
            //{
            //    throw new InvalidOperationException();
            //}
            //else if (!result.Value)
            //{
            //    throw new InvalidOperationException();
            //}
            //var fileName = dialog.FileName;
            //var bitmap = new BitmapImage(new Uri(fileName));
            //_writeableBitmap = new WriteableBitmap(bitmap);
            //_imageDrawing.ImageSource = _writeableBitmap;
            //_imageDrawing.Rect = new Rect(0, 0, _writeableBitmap.Width, _writeableBitmap.Height);
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private static T FindControl<T>(DependencyObject root, string controlName)
            where T : DependencyObject
        {
            if (root == null)
            {
                throw new ArgumentException();
            }

            T control = null;

            var childCount = VisualTreeHelper.GetChildrenCount(root);

            for (var i = 0; i < childCount; i++)
            {
                control = VisualTreeHelper.GetChild(root, i) as T;

                if (control != null)
                {
                    var frameworkElement = control as FrameworkElement;

                    if ((frameworkElement != null) && frameworkElement.Name == controlName)
                    {
                        return control;
                    }
                }
            }

            return null;
        }
    }
}
