//-----------------------------------------------------------------------
// <copyright file="ShortFolderConverter.cs" company="Lifeprojects">
//     Class: ShortFolderConverter
//     Copyright © Lifeprojects 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects</author>
// <email>gerhard.ahrens@lifeprojects</email>
// <date>11.10.2019</date>
//
// <summary>Class for WPF Converter, FileTargetFolderView</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Windows.Data;

    [ValueConversion(typeof(string), typeof(string))]
    public class ShortFolderNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if (value != null && value is string)
            {
                string fullPath = Path.GetFullPath(value.ToString()).TrimEnd(Path.DirectorySeparatorChar);
                string projectName = fullPath.Split(Path.DirectorySeparatorChar).Last();
                result = projectName;
            }


            return result;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}