// Copyright (c) 2019 Moeru. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Windows;
using ShinyPixel.Controls.Dialogs;

namespace ShinyPixel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region BITMAP_FILE_OPEN_DEAD_CODE
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
        #endregion

        private void MenuNew_Click(object sender, RoutedEventArgs e)
        {
            var newImageDialog = new NewImageDialog();

            var result = newImageDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                Console.WriteLine("OK!");
            }
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
