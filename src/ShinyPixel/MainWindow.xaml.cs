// Copyright (c) 2019 Moeru. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Win32;
using System;
using System.Linq;
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
        private Image _imageControl;
        private ImageDrawing _imageDrawing;
        private WriteableBitmap _writeableBitmap;

        public MainWindow()
        {
            InitializeComponent();

            var dockPanel = Content as DependencyObject;

            var grid = FindControl<Grid>(dockPanel, "MainWindowGrid");

            _writeableBitmap = new WriteableBitmap(800, 450, 96, 96, PixelFormats.Rgb24, null);

            _imageDrawing = new ImageDrawing()
            {
                Rect = new Rect(0, 0, 800, 450),
                ImageSource = _writeableBitmap
            };


            _imageControl = new Image()
            {
                Stretch = Stretch.None,
                Source = new DrawingImage(_imageDrawing)
            };

            var scrollViewer = new ScrollViewer();
            scrollViewer.Content = _imageControl;

            grid.Children.Add(scrollViewer);

            _imageControl.AddHandler(MouseDownEvent, new MouseButtonEventHandler(Image_MouseDown));
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(_imageControl);

            var column = Convert.ToInt32(position.X);
            var row = Convert.ToInt32(position.Y);

            try
            {
                _writeableBitmap.Lock();

                Console.WriteLine($"X: {position.X} Y: {position.Y}");
                Console.WriteLine($"row: {row} col: {column}");
                Console.WriteLine($"Bits: {_writeableBitmap.Format.BitsPerPixel}");

                foreach (var mask in _writeableBitmap.Format.Masks)
                {
                    Console.WriteLine($"Masks: {string.Join(", ", mask.Mask.Select(b => b.ToString()))}");
                }

                unsafe
                {
                    var backBufferPointer = _writeableBitmap.BackBuffer;

                    backBufferPointer += row * _writeableBitmap.BackBufferStride;
                    backBufferPointer += column * _writeableBitmap.Format.BitsPerPixel / 8;

                    *((byte*)backBufferPointer) = 255; // Red
                    *((byte*)backBufferPointer + 1) = 150; // Green
                    *((byte*)backBufferPointer + 2) = 200; // Blue

                    for (var i = 0; i < 3; i++)
                    {
                        var x = *((byte*)(backBufferPointer + i));
                        Console.WriteLine($"Bit: {x}");
                    }

                }

                _writeableBitmap.AddDirtyRect(new Int32Rect(column - 1, row, 2, 2));
            }
            finally
            {
                _writeableBitmap.Unlock();
            }
        }

        private void MenuOpen_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();

            dialog.DefaultExt = ".bmp";
            dialog.Filter = "Bitmap Files (*.bmp)|*.bmp";

            var result = dialog.ShowDialog();

            if (!result.HasValue)
            {
                throw new InvalidOperationException();
            }
            else if (!result.Value)
            {
                throw new InvalidOperationException();
            }
            var fileName = dialog.FileName;
            var bitmap = new BitmapImage(new Uri(fileName));
            _writeableBitmap = new WriteableBitmap(bitmap);
            _imageDrawing.ImageSource = _writeableBitmap;
            _imageDrawing.Rect = new Rect(0, 0, _writeableBitmap.PixelWidth, _writeableBitmap.PixelHeight);
        }

        private void MenuSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Save");
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
