using System;
using System.Globalization;
using System.Windows;

namespace MoyuTokei.Converters
{
    internal class ReverseBoolConverter : ConverterBase<ReverseBoolConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool b ? !b : DependencyProperty.UnsetValue;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool b ? !b : DependencyProperty.UnsetValue;
        }
    }
}