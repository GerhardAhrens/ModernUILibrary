/*
 * <copyright file="ObjectToExtension.cs" company="Lifeprojects.de">
 *     Class: ObjectExtension
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>29.09.2020</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Erweiterung Methoden zum Typ Object
 * </summary>
 *
 * <WebLink>
 * </WebLink>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernBaseLibrary.Extension
{
    using System.ComponentModel;
    using System.Linq;

    public static partial class ObjectExtension
    {
        /// <summary>
        /// Die Methode konvertiert ein Objekt in den übergeben Typ
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">this.</param>
        /// <returns>Gibt das konvertierte Objekt mit dem übergeben Typ zurück</returns>
        public static T To<T>(this object @this)
        {
            if (@this != null)
            {
                Type targetType = typeof(T);

                if (@this.GetType() == targetType)
                {
                    return (T)@this;
                }

                TypeConverter converter = TypeDescriptor.GetConverter(@this);
                if (converter != null)
                {
                    if (converter.CanConvertTo(targetType))
                    {
                        return (T)converter.ConvertTo(@this, targetType);
                    }
                }

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null)
                {
                    if (converter.CanConvertFrom(@this.GetType()))
                    {
                        return (T)converter.ConvertFrom(@this);
                    }
                }

                if (@this == DBNull.Value)
                {
                    return (T)(object)null;
                }
            }

            return (T)@this;
        }

        /// <summary>
        /// Die Methode konvertiert ein Objekt in den übergeben Typ
        /// </summary>
        /// <param name="this">this.</param>
        /// <param name="type">The type.</param>
        /// <returns>Gibt das konvertierte Objekt mit dem übergeben Typ zurück</returns>
        public static object To(this object @this, Type type)
        {
            if (@this != null)
            {
                Type targetType = type;

                if (@this.GetType() == targetType)
                {
                    return @this;
                }

                TypeConverter converter = TypeDescriptor.GetConverter(@this);
                if (converter != null)
                {
                    if (converter.CanConvertTo(targetType))
                    {
                        return converter.ConvertTo(@this, targetType);
                    }
                }

                converter = TypeDescriptor.GetConverter(targetType);
                if (converter != null)
                {
                    if (converter.CanConvertFrom(@this.GetType()))
                    {
                        return converter.ConvertFrom(@this);
                    }
                }

                if (@this == DBNull.Value)
                {
                    return null;
                }
            }

            return @this;
        }

        /// <summary>
        /// Die Methode konvertiert ein Objekt in den übergeben Typ oder einen Default Wert zurück
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">this.</param>
        /// <param name="defaultValue">The default value factory.</param>
        /// <returns>Gibt das konvertierte Objekt mit dem übergeben Typ zurück.</returns>
        public static T ToOrDefault<T>(this object @this, Func<object, T> defaultValue)
        {
            try
            {
                if (@this != null)
                {
                    Type targetType = typeof(T);

                    if (@this.GetType() == targetType)
                    {
                        return (T)@this;
                    }

                    TypeConverter converter = TypeDescriptor.GetConverter(@this);
                    if (converter != null)
                    {
                        if (converter.CanConvertTo(targetType))
                        {
                            return (T)converter.ConvertTo(@this, targetType);
                        }
                    }

                    converter = TypeDescriptor.GetConverter(targetType);
                    if (converter != null)
                    {
                        if (converter.CanConvertFrom(@this.GetType()))
                        {
                            return (T)converter.ConvertFrom(@this);
                        }
                    }

                    if (@this == DBNull.Value)
                    {
                        return (T)(object)null;
                    }
                }

                return (T)@this;
            }
            catch (Exception)
            {
                return defaultValue(@this);
            }
        }

        /// <summary>
        /// Die Methode konvertiert ein Objekt in den übergeben Typ oder einen Default Wert zurück
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this"></param>
        /// <param name="defaultValue">The default value factory.</param>
        /// <returns></returns>
        public static T ToOrDefault<T>(this object @this, Func<T> defaultValue)
        {
            return @this.ToOrDefault(x => defaultValue());
        }

        /// <summary>
        ///     A System.Object extension method that converts this object to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">this.</param>
        /// <returns>The given data converted to a T.</returns>
        public static T ToOrDefault<T>(this object @this)
        {
            return @this.ToOrDefault(x => default(T));
        }

        /// <summary>
        ///     A System.Object extension method that converts this object to an or default.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="this">this.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns>The given data converted to a T.</returns>
        public static T ToOrDefault<T>(this object @this, T defaultValue)
        {
            return @this.ToOrDefault(x => defaultValue);
        }

        /// <summary>
        /// Es wird geprüft ob der übergebene String einem Bool-Wert entspricht<br/>
        /// Gültige Werte für True: 1,y,yes,true,ja, j, wahr<br/>
        /// Gültige Werte für False: 0,n,no,false,nein,falsch<br/>
        /// Groß- und Kleinschrebung wird ignoriert<br/>
        /// </summary>
        /// <param name="this">Übergebener String</param>
        /// <param name="ignorException">True = es wird keine Exception bei einem falschen Wert ausgelöst,<br/>False = Es wird eine InvalidCastException alsgelöst bei einem Fehler</param>
        /// <returns>Wenn der Wert einem entsprechendem Bool-Wert entspricht, wird True oder False zurückgegeben.</returns>
        public static bool ToBool(this object @this, bool ignorException = false)
        {
            string[] trueStrings = { "1", "y", "yes", "true", "ja", "j", "wahr" };
            string[] falseStrings = { "0", "n", "no", "false", "nein", "falsch" };

            if (@this != null && trueStrings.Contains(@this.ToString(), StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }

            if (@this != null && falseStrings.Contains(@this.ToString(), StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            if (ignorException == true)
            {
                return false;
            }
            else
            {
                string msg = "only the following are supported for converting strings to boolean: ";
                throw new InvalidCastException($"{msg} {string.Join(",", trueStrings)} and {string.Join(",", falseStrings)}");
            }
        }
    }
}
