// Copyright (c) 2019 Moeru. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ShinyPixel.ViewModels;

namespace ShinyPixel.Controls.UserControls
{
    /// <summary>
    /// Interaction logic for PaletteColorControl.xaml
    /// </summary>
    public partial class PaletteColorControl : UserControl
    {
        private ColorViewModel _color;
        private SolidColorBrush _previewColorBrush;

        public PaletteColorControl()
        {
            InitializeComponent();

            _color = new ColorViewModel();

            DataContext = _color;

            _previewColorBrush = new SolidColorBrush(
                Color.FromRgb(
                    _color.Red,
                    _color.Green,
                    _color.Blue));

            var canvas = ControlUtils.FindControl<Canvas>(Content as DependencyObject, "ColorCanvas");
            canvas.Background = _previewColorBrush;


            var changeHandler = new PropertyChangedEventHandler(Color_PropertyChanged);
            _color.PropertyChanged += changeHandler;
            void handler(object sender, RoutedEventArgs args)
            {
                _color.PropertyChanged -= changeHandler;
                Unloaded -= handler;
            }

            Unloaded += handler;
        }

        private void Color_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _previewColorBrush.Color =
                Color.FromRgb(
                    _color.Red,
                    _color.Green,
                    _color.Blue);
        }
    }
}
