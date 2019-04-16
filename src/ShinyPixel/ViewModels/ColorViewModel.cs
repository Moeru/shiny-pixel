// Copyright (c) 2019 Moeru. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

namespace ShinyPixel.ViewModels
{
    public class ColorViewModel : AutomaticDependencyObject
    {
        public byte Red
        {
            get => GetValue<byte>(nameof(Red));
            set => SetValue(nameof(Red), value);
        }

        public byte Green
        {
            get { return GetValue<byte>(nameof(Green)); }
            set { SetValue(nameof(Green), value); }
        }

        public byte Blue
        {
            get { return GetValue<byte>(nameof(Blue)); }
            set { SetValue(nameof(Blue), value); }
        }
    }
}
