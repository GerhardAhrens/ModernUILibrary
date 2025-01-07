//-----------------------------------------------------------------------
// <copyright file="TypeConverters.cs" company="Lifeprojects.de">
//     Class: TypeConverters
//     Copyright � Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>06.10.2023 20:16:02</date>
//
// <summary>
// Klasse f�r 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Globalization;

    internal static class TypeConverters
    {
        internal static bool CanConvertFrom<T>(Type sourceType)
        {
            if (sourceType == null)
            {
                throw new ArgumentNullException("sourceType");
            }
            return sourceType == typeof(string) || typeof(T).IsAssignableFrom(sourceType);
        }

        internal static object ConvertFrom<T>(TypeConverter converter, object value)
        {
            Debug.Assert(converter != null, "converter should not be null!");
            if (value is T)
            {
                return value;
            }
            throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "Invalid Type Converter {0} {1}", new object[]
            {
                converter.GetType().Name,
                (value != null) ? value.GetType().FullName : "(null)"
            }));
        }

        internal static bool CanConvertTo<T>(Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            return destinationType == typeof(string) || destinationType.IsAssignableFrom(typeof(T));
        }

        internal static object ConvertTo(TypeConverter converter, object value, Type destinationType)
        {
            Debug.Assert(converter != null, "converter should not be null!");
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            object result;
            if (value == null && !destinationType.IsValueType)
            {
                result = null;
            }
            else
            {
                if (value == null || !destinationType.IsInstanceOfType(value))
                {
                    throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, "TypeConversionNotSupported {0} {1} {2}", new object[]
                    {
                        converter.GetType().Name,
                        (value != null) ? value.GetType().FullName : "(null)",
                        destinationType.GetType().Name
                    }));
                }
                result = value;
            }
            return result;
        }
    }
}
