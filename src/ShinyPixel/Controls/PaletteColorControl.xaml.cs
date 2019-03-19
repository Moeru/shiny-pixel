// Copyright (c) 2019 Moeru. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ShinyPixel.Controls
{
    /// <summary>
    /// Interaction logic for PaletteColorControl.xaml
    /// </summary>
    public partial class PaletteColorControl : UserControl, INotifyPropertyChanged
    {
        private SolidColorBrush _previewColorBrush;

        #region PropertyChangedEvents
        public event PropertyChangedEventHandler PropertyChanged;

        protected static void OnPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        => ((PaletteColorControl)d).PropertyChanged
            ?.Invoke(d, new PropertyChangedEventArgs(e.Property.Name));
        #endregion

        #region DependencyPropertyHelpers
        private static DependencyProperty MakeDependencyProperty<TProperty>(
            string propertyName,
            TProperty defaultValue)
            => DependencyProperty.Register(
                propertyName,
                typeof(TProperty),
                typeof(PaletteColorControl),
                new PropertyMetadata(
                    defaultValue,
                    OnPropertyChanged));
        #endregion

        #region Red
        public static readonly DependencyProperty RedProperty =
            MakeDependencyProperty<byte>(nameof(Red), 0);

        public byte Red
        {
            get { return (byte)GetValue(RedProperty); }
            set { SetValue(RedProperty, value); }
        }
        #endregion

        #region Green
        public static readonly DependencyProperty GreenProperty =
            MakeDependencyProperty<byte>(nameof(Green), 0);

        public byte Green
        {
            get { return (byte)GetValue(GreenProperty); }
            set { SetValue(GreenProperty, value); }
        }
        #endregion

        #region Blue
        public static readonly DependencyProperty BlueProperty =
            MakeDependencyProperty<byte>(nameof(Blue), 0);

        public byte Blue
        {
            get { return (byte)GetValue(BlueProperty); }
            set { SetValue(BlueProperty, value); }
        }
        #endregion

        public PaletteColorControl()
        {
            InitializeComponent();

            DataContext = this;

            _previewColorBrush = new SolidColorBrush(Color.FromRgb(Red, Green, Blue));

            var canvas = ControlUtils.FindControl<Canvas>(Content as DependencyObject, "ColorCanvas");
            canvas.Background = _previewColorBrush;

            PropertyChanged += new PropertyChangedEventHandler(Color_PropertyChanged);
        }

        private void Color_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            _previewColorBrush.Color = Color.FromRgb(Red, Green, Blue);
        }
    }
}
