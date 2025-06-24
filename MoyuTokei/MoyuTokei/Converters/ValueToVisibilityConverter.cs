using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MoyuTokei.Converters
{
    internal class ValueToVisibilityConverter : IValueConverter
    {
        private static readonly Lazy<ValueToVisibilityConverter> _presetDefaultBehavior = new();
        private static readonly Lazy<ValueToVisibilityConverter> _presetReverseDefaultBehavior = new(() => new()
        {
            DefaultAs = Visibility.Visible,
            NonDefaultAs = Visibility.Collapsed
        });

        public Visibility DefaultAs { get; set; } = Visibility.Collapsed;

        public Visibility NonDefaultAs { get; set; } = Visibility.Visible;

        public static ValueToVisibilityConverter DefaultBehavior => _presetDefaultBehavior.Value;

        public static ValueToVisibilityConverter ReverseDefaultBehavior => _presetReverseDefaultBehavior.Value;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == default ? DefaultAs : NonDefaultAs;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}