// Copyright (c) 2019 Moeru. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Windows;
using System.Windows.Media;

namespace ShinyPixel.Controls
{
    public static class ControlUtils
    {
        public static TControl FindControl<TControl>(DependencyObject root, string controlName)
            where TControl : FrameworkElement
        {
            if (root == null)
            {
                throw new ArgumentException();
            }

            var childCount = VisualTreeHelper.GetChildrenCount(root);

            for (var i = 0; i < childCount; i++)
            {
                var dependencyObject = VisualTreeHelper.GetChild(root, i);

                var control = dependencyObject as TControl;

                if (control != null && control.Name == controlName)
                {
                    return control;
                }

                control = FindControl<TControl>(control, controlName);

                if (control != null)
                {
                    return control;
                }
            }

            return null;
        }
    }
}
