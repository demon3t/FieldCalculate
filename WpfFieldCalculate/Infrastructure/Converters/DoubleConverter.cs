﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace WpfFieldCalculate.Infrastructure.Converters
{
    [ValueConversion(typeof(object), typeof(double))]
    internal class DoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                return doubleValue.ToString().Replace('.', ',');
            }

            return DependencyProperty.UnsetValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value.ToString()?.Replace('.', ','), out double doubleValue))
            {
                return doubleValue;
            }

            return 0;
        }
    }
}
