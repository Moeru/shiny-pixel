// Copyright (c) 2019 Moeru. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace ShinyPixel.ViewModels
{
    public class DrawingAreaViewModel : AutomaticDependencyObject
    {
        public int ZoomFactor
        {
            get => GetValue<int>(nameof(ZoomFactor));
            set => SetValue(nameof(ZoomFactor), value);
        }
    }
}
