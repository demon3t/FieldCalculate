using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WpfFieldCalculate.Infrastructure
{

    /// <summary>
    /// Дополнительные свойства Control.
    /// </summary>
    internal class AspectRatioHelper : Control
    {
        #region Соотношение сторон

        public static readonly DependencyProperty AspectRatioProperty =
        DependencyProperty.RegisterAttached(
            "AspectRatio",
            typeof(string),
            typeof(AspectRatioHelper),
            new PropertyMetadata("1:1", OnAspectRatioChanged));

        public static string GetAspectRatio(DependencyObject obj)
        {
            return (string)obj.GetValue(AspectRatioProperty);
        }

        public static void SetAspectRatio(DependencyObject obj, string value)
        {
            obj.SetValue(AspectRatioProperty, value);
        }

        private static void OnAspectRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Control control && e.NewValue is string aspectRatio)
            {
                control.SetBinding(HeightProperty, new Binding(nameof(ActualWidth))
                {
                    Source = control,
                    Converter = new AspectRatioConverter(aspectRatio)
                });
            }
        }

        #endregion
    }
    public class AspectRatioConverter : IValueConverter
    {
        private readonly string _aspectRatio;

        public AspectRatioConverter(string aspectRatio)
        {
            _aspectRatio = aspectRatio;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var temp = _aspectRatio.Split(":");

            var w = double.Parse(temp[0]);
            var h = double.Parse(temp[1]);

            var ratio = h / w;

            if (value is double width)
            {
                return width * ratio;
            }
            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
