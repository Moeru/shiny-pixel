// Copyright (c) 2019 Moeru. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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

            _writeableBitmap = new WriteableBitmap(800, 450, 96, 96, PixelFormats.Bgr24, null);

            _imageDrawing = new ImageDrawing()
            {
                Rect = new Rect(0, 0, 800, 450),
                ImageSource = _writeableBitmap
            };

            var drawingImageSource = new DrawingImage(_imageDrawing);

            _imageControl = new Image()
            {
                Stretch = Stretch.None,
                Source = drawingImageSource
            };

            grid.Children.Add(_imageControl);

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

                unsafe
                {
                    var backBufferPointer = _writeableBitmap.BackBuffer;

                    backBufferPointer += row * _writeableBitmap.BackBufferStride;
                    backBufferPointer += column * 4;

                    var color_data = 255 << 16; // Red
                    color_data |= 150 << 8; // Green
                    color_data |= 200 << 0; // Blue

                    *((int*)backBufferPointer) = color_data;

                    backBufferPointer -= 4;

                    *((int*)(backBufferPointer)) = color_data;


                    for (var i = 0; i < 12; i++)
                    {
                        var x = *((byte*)(backBufferPointer + i));
                        Console.WriteLine($"X: {x}");
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
            _imageDrawing.Rect = new Rect(0, 0, _writeableBitmap.Width, _writeableBitmap.Height);
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
