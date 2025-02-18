//-----------------------------------------------------------------------
// <copyright file="EnumDescriptionConverter.cs" company="Lifeprojects.de">
//     Class: EnumDescriptionConverter
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.04.2020</date>
//
// <summary>
// Die Converter Class gibt eine Enum Description als String zurück
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Reflection;
    using System.Windows.Data;

    [ValueConversion(typeof(Enum), typeof(string))]
    public sealed class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = string.Empty;

            if(value != null)
            {
                Enum myEnum = (Enum)value;
                result = GetEnumDescription(myEnum);
                return result;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Empty;
        }

        private string GetEnumDescription(Enum enumObj)
        {
            FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());
            object[] attribArray = fieldInfo.GetCustomAttributes(false);

            if (attribArray.Length == 0)
            {
                return enumObj.ToString();
            }
            else
            {
                DescriptionAttribute attrib = null;

                foreach (var att in attribArray)
                {
                    if (att is DescriptionAttribute)
                    {
                        attrib = att as DescriptionAttribute;
                    }
                }

                if (attrib != null)
                {
                    return attrib.Description;
                }

                return enumObj.ToString();
            }
        }
    }
}
