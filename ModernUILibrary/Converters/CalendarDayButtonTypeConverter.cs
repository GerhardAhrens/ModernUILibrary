﻿namespace ModernIU.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Data;

    public class CalendarDayButtonTypeConverter : IMultiValueConverter
    {
        public object Convert(object[] value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string str = string.Empty;
            List<DateTime> list = (List<DateTime>)value[0];
            if (list.Count > 1)
            {
                DateTime dtStart = ((List<DateTime>)value[0])[0];
                DateTime dtEnd = ((List<DateTime>)value[0])[1];
                DateTime dtDayButton = (DateTime)value[1];
                if (dtDayButton == dtStart)
                {
                    str = "Left";
                }
                else if(dtDayButton > dtStart && dtDayButton < dtEnd)
                {
                    str = "Middle";
                }
                else if(dtDayButton == dtEnd)
                {
                    str = "Right";
                }
            }
            return str;
        }
        public object[] ConvertBack(object value, Type[] targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
