//-----------------------------------------------------------------------
// <copyright file="BaseConverter.cs" company="Lifeprojects.de">
//     Class: BaseConverter
//     Copyright © Lifeprojects.de GmbH 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>26.06.2017</date>
//
// <summary>
// Typisierte Basisklasse für WPF Value Converter
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    /// <summary>
    /// WPF Converter Base Class
    /// </summary>
    /// <typeparam name="TSourceValue">Wert der Konvertiert werden soll</typeparam>
    /// <typeparam name="TResultType">Datentyp für den konvertieren Wert</typeparam>
    /// <typeparam name="TConverterParam">Converter Parameter</typeparam>
    public abstract class ConverterBase<TSourceValue, TResultType, TConverterParam> : MarkupExtension, IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value">Wert der Konvertiert werden soll</param>
        /// <param name="targetType">Datentyp für den konvertieren Wert</param>
        /// <param name="parameter">Converter Parameter</param>
        /// <param name="culture">CultureInfo</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                if (value.GetType().BaseType.Name != "Enum")
                {
                    if (value.GetType() != typeof(TSourceValue))
                    {
                        throw new ArgumentException(GetType().Name + ".Convert: value type not " + typeof(TSourceValue).Name);
                    }
                }
            }

            if (targetType != typeof(TResultType))
            {
                throw new ArgumentException(GetType().Name + ".Convert: target type not " + typeof(TResultType).Name);
            }

            return this.Convert((TSourceValue)value, (TConverterParam)parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value.GetType() != typeof(TResultType))
            {
                throw new ArgumentException(GetType().Name + ".ConvertBack: value type not " + typeof(TResultType).Name);
            }

            if (targetType != typeof(TSourceValue))
            {
                throw new ArgumentException(GetType().Name + ".ConvertBack: target type not " + typeof(TSourceValue).Name);
            }

            return this.ConvertBack((TResultType)value, (TConverterParam)parameter, culture);
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
            => this;

        protected virtual TResultType Convert(TSourceValue value, TConverterParam parameter, CultureInfo culture)
        {
            throw new NotImplementedException(GetType().Name + "Convert not implemented");
        }

        protected virtual TSourceValue ConvertBack(TResultType value, TConverterParam parameter, CultureInfo culture)
        {
            throw new NotImplementedException(GetType().Name + "ConvertBack not implemented");
        }
    }

    /// <summary>
    /// Base Class for WPF Converter without Parameter
    /// </summary>
    /// <typeparam name="TSourceValue">Convert from Value</typeparam>
    /// <typeparam name="TResultType">Convert to Value</typeparam>
    public abstract class ConverterBase<TSourceValue, TResultType> : ConverterBase<TSourceValue, TResultType, object>
    {
        protected virtual TResultType Convert(TSourceValue value, CultureInfo culture)
        {
            throw new NotImplementedException(GetType().Name + "Convert not implemented");
        }

        protected virtual TSourceValue ConvertBack(TResultType value, CultureInfo culture)
        {
            throw new NotImplementedException(GetType().Name + "ConvertBack not implemented");
        }

        protected sealed override TResultType Convert(TSourceValue value, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                throw new ArgumentException(GetType().Name + ".Convert: binding contains unexpected parameter");
            }

            return this.Convert(value, culture);
        }

        protected sealed override TSourceValue ConvertBack(TResultType value, object parameter, CultureInfo culture)
        {
            if (parameter != null)
            {
                throw new ArgumentException(GetType().Name + ".ConvertBack: binding contains unexpected parameter");
            }

            return this.ConvertBack(value, culture);
        }
    }
}