using System;
using System.Globalization;
using System.Threading;
using System.Windows.Data;

namespace MoyuTokei.Converters
{
    internal abstract class ConverterBase<TConverter> : IValueConverter
    {
        private static readonly Lazy<TConverter> _instance = new(LazyThreadSafetyMode.ExecutionAndPublication);

        public static TConverter Instance => _instance.Value;

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);
        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);
    }
}