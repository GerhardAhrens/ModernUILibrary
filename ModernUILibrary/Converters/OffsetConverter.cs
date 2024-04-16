﻿namespace ModernIU.Converters
{
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public class OffsetConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.FirstOrDefault(v => v == DependencyProperty.UnsetValue) != null)
            {
                return double.NaN;
            }

            double placementTargetWidth = (double)values[0];
            double toolTipWidth = (double)values[1];

            if("Center".Equals(parameter))
            {
                return (placementTargetWidth / 2.0) - (toolTipWidth / 2.0);
            }
            else if("Right".Equals(parameter))
            {
                return placementTargetWidth  - toolTipWidth;
            }

            return 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
