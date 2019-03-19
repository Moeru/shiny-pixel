// Copyright (c) 2019 Moeru. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ShinyPixel.Controls
{
    /// <summary>
    /// Interaction logic for DrawingUiControl.xaml
    /// </summary>
    public partial class DrawingUiControl : UserControl, INotifyPropertyChanged
    {
        private Image _imageControl;
        private ImageDrawing _imageDrawing;
        private WriteableBitmap _writeableBitmap;

        #region PropertyChangedEvents
        public event PropertyChangedEventHandler PropertyChanged;

        protected static void OnPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        => ((DrawingUiControl)d).PropertyChanged
            ?.Invoke(d, new PropertyChangedEventArgs(e.Property.Name));
        #endregion

        #region ZoomFactor
        public static readonly DependencyProperty ZoomFactorProperty =
            DependencyProperty.Register(
                nameof(ZoomFactor),
                typeof(int),
                typeof(DrawingUiControl),
                new PropertyMetadata(5, OnPropertyChanged));

        public int ZoomFactor
        {
            get { return (int)GetValue(ZoomFactorProperty); }
            set { SetValue(ZoomFactorProperty, value); }
        }
        #endregion

        public DrawingUiControl()
        {
            InitializeComponent();

            DataContext = this;

            _writeableBitmap = new WriteableBitmap(
                100 * ZoomFactor,
                100 * ZoomFactor,
                96,
                96,
                PixelFormats.Bgr32,
                null);

            _imageDrawing = new ImageDrawing
            {
                Rect = new Rect(0, 0, _writeableBitmap.Width, _writeableBitmap.Height),
                ImageSource = _writeableBitmap
            };

            _imageControl = new Image()
            {
                Stretch = Stretch.None,
                Source = _writeableBitmap
            };

            _imageControl.AddHandler(MouseDownEvent, new MouseButtonEventHandler(Image_MouseDown));
            PropertyChanged += new PropertyChangedEventHandler(ZoomFactor_PropertyChanged);

            var grid = Content as Grid;
            grid.Children.Add(_imageControl);
        }

        private void ZoomFactor_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var z = ZoomFactor;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(_imageControl);

            var column = Convert.ToInt32(position.X) / ZoomFactor;
            var row = Convert.ToInt32(position.Y) / ZoomFactor;

            try
            {
                _writeableBitmap.Lock();

                unsafe
                {
                    var color = 0x00FFFFFF; // White
                    var bytesPerPixel = _writeableBitmap.Format.BitsPerPixel / 8;

                    for (var i = 0; i < ZoomFactor; i++)
                    {
                        var backBufferPointer = _writeableBitmap.BackBuffer;

                        backBufferPointer += ((row * ZoomFactor) + i) * _writeableBitmap.BackBufferStride;
                        backBufferPointer += column * ZoomFactor * bytesPerPixel;

                        for (var j = 0; j < ZoomFactor; j++)
                        {
                            *((int*)(backBufferPointer + (j * bytesPerPixel))) = color;
                        }
                    }
                }

                _writeableBitmap.AddDirtyRect(new Int32Rect(column * ZoomFactor, row * ZoomFactor, ZoomFactor, ZoomFactor));
            }
            finally
            {
                _writeableBitmap.Unlock();
            }

            //            UpdateDrawing();
        }
    }
}
