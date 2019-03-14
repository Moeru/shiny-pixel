// Copyright (c) 2019 Moeru. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
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
        private DrawingGroup _drawingGroup;
        private DrawingVisual _backgroundDrawingVisual;
        private Image _imageControl;
        private SolidColorBrush _brush;
        private SolidColorBrush _brush2;
        private RenderTargetBitmap _renderTargetBitmap;

        public MainWindow()
        {
            InitializeComponent();

            var geometryGroupA = new GeometryGroup();
            var geometryGroupB = new GeometryGroup();

            for (var i = 0; i < 100; i++)
            {
                for (var j = 0; j < 100; j++)
                {
                    var rect = new Rect(5 * i, 5 * j, 5, 5);

                    if ((i + j) % 2 == 0)
                    {
                        geometryGroupA.Children.Add(new RectangleGeometry(rect));
                    }
                    else
                    {
                        geometryGroupB.Children.Add(new RectangleGeometry(rect));
                    }
                }
            }

            _drawingGroup = new DrawingGroup();

            _brush = new SolidColorBrush(Colors.Black);
            var geometryDrawingA = new GeometryDrawing();
            geometryDrawingA.Geometry = geometryGroupA;
            geometryDrawingA.Brush = _brush;
            _drawingGroup.Children.Add(geometryDrawingA);

            _brush2 = new SolidColorBrush(Colors.Red);
            var geometryDrawingB = new GeometryDrawing();
            geometryDrawingB.Geometry = geometryGroupB;
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
                Source = _renderTargetBitmap // new DrawingImage(_drawingGroup)
            };

            var dockPanel = Content as DependencyObject;
            var grid = FindControl<Grid>(dockPanel, "MainWindowGrid");
            grid.Children.Add(_imageControl);

            _imageControl.AddHandler(MouseDownEvent, new MouseButtonEventHandler(Image_MouseDown));
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(_imageControl);

            var column = Convert.ToInt32(position.X);
            var row = Convert.ToInt32(position.Y);

            _brush.Color = Color.FromRgb(
                (byte)((column * row) % 256),
                (byte)((2 * column * row) % 256),
                (byte)((3 * column * row) % 256));

            using (var drawingContext = _backgroundDrawingVisual.RenderOpen())
            {
                drawingContext.DrawDrawing(_drawingGroup);
            }

            _renderTargetBitmap.Render(_backgroundDrawingVisual);

            //try
            //{
            //    _writeableBitmap.Lock();

            //    unsafe
            //    {
            //        var backBufferPointer = _writeableBitmap.BackBuffer;

            //        backBufferPointer += row * _writeableBitmap.BackBufferStride;
            //        backBufferPointer += column * 4;

            //        var color_data = 255 << 16; // Red
            //        color_data |= 150 << 8; // Green
            //        color_data |= 200 << 0; // Blue

            //        *((int*)backBufferPointer) = color_data;

            //        backBufferPointer -= 4;

            //        *((int*)(backBufferPointer)) = color_data;


            //        for (var i = 0; i < 12; i++)
            //        {
            //            var x = *((byte*)(backBufferPointer + i));
            //            Console.WriteLine($"X: {x}");
            //        }

            //    }

            //    _writeableBitmap.AddDirtyRect(new Int32Rect(column - 1, row, 2, 2));
            //}
            //finally
            //{
            //    _writeableBitmap.Unlock();
            //}
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
