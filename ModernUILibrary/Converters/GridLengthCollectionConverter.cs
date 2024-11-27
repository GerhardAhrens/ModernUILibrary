//-----------------------------------------------------------------------
// <copyright file="SimpleGrid.cs" company="Lifeprojects.de">
//     Class: SimpleGrid
//     Copyright © Gerhard Ahrens, 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>22.08.2019</date>
//
// <summary>
// Class for UI Control SimpleGrid
// </summary>
// < Website >
// https://thomaslevesque.com/tag/wpf/page/2/
// </ Website >
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Linq;
    using System.Windows;

    using ModernIU.Controls;

    public class GridLengthCollectionConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            string s = value as string;
            if (s != null)
                return ParseString(s, culture);
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is GridLengthCollection)
                return ToString((GridLengthCollection)value, culture);
            return base.ConvertTo(context, culture, value, destinationType);
        }

        private string ToString(GridLengthCollection value, CultureInfo culture)
        {
            var converter = new GridLengthConverter();
            return string.Join(",", value.Select(v => converter.ConvertToString(v)));
        }

        private GridLengthCollection ParseString(string s, CultureInfo culture)
        {
            var converter = new GridLengthConverter();
            var lengths = s.Split(',').Select(p => (GridLength)converter.ConvertFromString(p.Trim()));
            return new GridLengthCollection(lengths.ToArray());
        }
    }
}