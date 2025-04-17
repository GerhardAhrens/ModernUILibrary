namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    [ValueConversion(typeof(SolidColorBrush), typeof(SolidColorBrush))]
    public class BrushOpacityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var opacity = System.Convert.ToDouble(parameter);
        var checkBrush = value as SolidColorBrush;

        return new SolidColorBrush(checkBrush.Color)
        {
            Opacity = opacity
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
}
