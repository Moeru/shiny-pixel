// Copyright (c) 2019 Moeru. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ShinyPixel.ViewModels;

namespace ShinyPixel.Controls.UserControls
{
    /// <summary>
    /// Interaction logic for DrawingUiControl.xaml
    /// </summary>
    public partial class DrawingUiControl : UserControl
    {
        private DrawingAreaViewModel _drawingArea;
        private Image _imageControl;
        private WriteableBitmap _writeableBitmap;

        public DrawingUiControl()
        {
            InitializeComponent();

            _drawingArea = new DrawingAreaViewModel
            {
                ZoomFactor = 5
            };

            DataContext = _drawingArea;

            _writeableBitmap = new WriteableBitmap(
                100 * _drawingArea.ZoomFactor,
                100 * _drawingArea.ZoomFactor,
                96,
                96,
                PixelFormats.Bgr32,
                null);

            _imageControl = new Image()
            {
                Stretch = Stretch.None,
                Source = _writeableBitmap
            };

            var mouseButtonEventHandler = new MouseButtonEventHandler(Image_MouseDown);
            var propertyChangedEventHandler = new PropertyChangedEventHandler(ZoomFactor_PropertyChanged);

            _imageControl.MouseDown += mouseButtonEventHandler;
            _drawingArea.PropertyChanged += propertyChangedEventHandler;

            void unloadHandler(object sender, RoutedEventArgs args)
            {
                _imageControl.MouseDown -= mouseButtonEventHandler;
                _drawingArea.PropertyChanged -= propertyChangedEventHandler;

                Unloaded -= unloadHandler;
            }
            Unloaded += unloadHandler;

            var grid = Content as Grid;
            grid.Children.Add(_imageControl);
        }

        private void ZoomFactor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var z = _drawingArea.ZoomFactor;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(_imageControl);
            var zoom = _drawingArea.ZoomFactor;

            var column = Convert.ToInt32(position.X) / zoom;
            var row = Convert.ToInt32(position.Y) / zoom;

            try
            {
                _writeableBitmap.Lock();

                unsafe
                {
                    var color = 0x00FFFFFF; // White
                    var bytesPerPixel = _writeableBitmap.Format.BitsPerPixel / 8;

                    for (var i = 0; i < zoom; i++)
                    {
                        var backBufferPointer = _writeableBitmap.BackBuffer;

                        backBufferPointer += ((row * zoom) + i) * _writeableBitmap.BackBufferStride;
                        backBufferPointer += column * zoom * bytesPerPixel;

                        for (var j = 0; j < zoom; j++)
                        {
                            *((int*)(backBufferPointer + (j * bytesPerPixel))) = color;
                        }
                    }
                }

                _writeableBitmap.AddDirtyRect(
                    new Int32Rect(
                        column * zoom,
                        row * zoom,
                        zoom,
                        zoom));
            }
            finally
            {
                _writeableBitmap.Unlock();
            }
        }
    }
}
