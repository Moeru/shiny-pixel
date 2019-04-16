// Copyright (c) 2019 Moeru. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace ShinyPixel.ViewModels
{
    public abstract class AutomaticDependencyObject : DependencyObject, INotifyPropertyChanged
    {
        private static Dictionary<Type, Dictionary<string, DependencyProperty>> _dependencyProperties
            = new Dictionary<Type, Dictionary<string, DependencyProperty>>();

        private readonly Dictionary<string, DependencyProperty> _properties;

        protected AutomaticDependencyObject()
        {
            var type = GetType();
            if (!_dependencyProperties.ContainsKey(type))
            {
                _properties = new Dictionary<string, DependencyProperty>();

                foreach (var propertyInfo in type.GetProperties())
                {
                    _properties.Add(
                        propertyInfo.Name,
                        DependencyProperty.Register(
                            propertyInfo.Name,
                            propertyInfo.PropertyType,
                            type,
                            new PropertyMetadata(OnPropertyChanged))
                    );
                }

                _dependencyProperties.Add(type, _properties);
            }
            else
            {
                _properties = _dependencyProperties[type];
            }
        }

        protected TValue GetValue<TValue>(string name)
            => (TValue)(GetValue(_properties[name]));

        protected void SetValue<TValue>(string name, TValue value)
            => SetValue(_properties[name], value);

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private static void OnPropertyChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e)
        => ((AutomaticDependencyObject)d).PropertyChanged
            ?.Invoke(d, new PropertyChangedEventArgs(e.Property.Name));
        #endregion
    }
}
