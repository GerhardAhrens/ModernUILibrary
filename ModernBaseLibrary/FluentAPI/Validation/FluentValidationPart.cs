//-----------------------------------------------------------------------
// <copyright file="FluentValidationPart.cs" company="Lifeprojects.de">
//     Class: FluentValidation
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>30.09.2020</date>
//
// <summary>
// Partial Class for FluentValidation
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Globalization;

    public partial class FluentValidation<TObject>
    {
        public FluentValidation<TObject> IsOutRange<TTyp>(TTyp fromValue, TTyp toValue)
        {
            if (validationBuilder.PropertyValue == null)
            {
                validationBuilder.ValidResult = true;
                return this;
            }

            TTyp convertResult = (TTyp)Convert.ChangeType(validationBuilder.PropertyValue, typeof(TTyp), CultureInfo.InvariantCulture);

            bool result = this.IsOutRangeInternal<TTyp>(convertResult, fromValue, toValue);
            if (result == false)
            {
                validationBuilder.FromValue = fromValue;
                validationBuilder.ToValue = toValue;
                validationBuilder.ValidResult = false;
                return this;
            }

            validationBuilder.ValidResult = true;
            return this;
        }

        public FluentValidation<TObject> IsOutRangeOrEqual<TTyp>(TTyp fromValue, TTyp toValue)
        {
            if (validationBuilder.PropertyValue == null)
            {
                validationBuilder.ValidResult = true;
                return this;
            }

            TTyp convertResult = (TTyp)Convert.ChangeType(validationBuilder.PropertyValue, typeof(TTyp), CultureInfo.InvariantCulture);

            bool result = this.IsOutRangeEqualInternal<TTyp>(convertResult, fromValue, toValue);
            if (result == false)
            {
                validationBuilder.FromValue = fromValue;
                validationBuilder.ToValue = toValue;
                validationBuilder.ValidResult = false;
                return this;
            }

            validationBuilder.ValidResult = true;
            return this;
        }

        public FluentValidation<TObject> IsInRange<TTyp>(TTyp fromValue, TTyp toValue)
        {
            if (validationBuilder.PropertyValue == null)
            {
                validationBuilder.ValidResult = true;
                return this;
            }

            TTyp convertResult = (TTyp)Convert.ChangeType(validationBuilder.PropertyValue, typeof(TTyp), CultureInfo.InvariantCulture);

            bool result = this.IsInRangeInternal<TTyp>(convertResult, fromValue, toValue);
            if (result == false)
            {
                validationBuilder.FromValue = fromValue;
                validationBuilder.ToValue = toValue;
                validationBuilder.ValidResult = false;
                return this;
            }

            validationBuilder.ValidResult = true;
            return this;
        }

        public FluentValidation<TObject> IsInRangeOrEqual<TTyp>(TTyp fromValue, TTyp toValue)
        {
            if (validationBuilder.PropertyValue == null)
            {
                validationBuilder.ValidResult = true;
                return this;
            }

            TTyp convertResult = (TTyp)Convert.ChangeType(validationBuilder.PropertyValue, typeof(TTyp), CultureInfo.InvariantCulture);

            bool result = this.IsInRangeEqualInternal<TTyp>(convertResult, fromValue, toValue);
            if (result == false)
            {
                validationBuilder.FromValue = fromValue;
                validationBuilder.ToValue = toValue;
                validationBuilder.ValidResult = false;
                return this;
            }

            validationBuilder.ValidResult = true;
            return this;
        }

        public FluentValidation<TObject> IsNull()
        {
            if (validationBuilder.PropertyValue == null)
            {
                validationBuilder.ValidResult = true;
                return this;
            }
            else
            {
                validationBuilder.ValidResult = false;
                return this;
            }
        }

        public FluentValidation<TObject> IsNotNull()
        {
            if (validationBuilder.PropertyValue != null)
            {
                validationBuilder.ValidResult = true;
                return this;
            }

            validationBuilder.ValidResult = false;

            return this;
        }

        public FluentValidation<TObject> IsEqual(object compareValue)
        {
            if (validationBuilder.PropertyValue == null)
            {
                validationBuilder.ValidResult = true;
                return this;
            }

            bool result = validationBuilder.PropertyValue.Equals(compareValue);
            if (result == false)
            {
                validationBuilder.FromValue = compareValue;
                validationBuilder.ValidResult = false;
                return this;
            }

            validationBuilder.ValidResult = true;
            return this;
        }

        public FluentValidation<TObject> IsNotEqual(object compareValue)
        {
            if (validationBuilder.PropertyValue == null)
            {
                validationBuilder.ValidResult = true;
                return this;
            }

            bool result = validationBuilder.PropertyValue.Equals(compareValue) == false;
            if (result == false)
            {
                validationBuilder.FromValue = compareValue;
                validationBuilder.ValidResult = false;
                return this;
            }

            validationBuilder.ValidResult = true;
            return this;
        }

        public FluentValidation<TObject> IsEnumEqual(Enum enumType)
        {
            if (validationBuilder.PropertyValue == null)
            {
                validationBuilder.ValidResult = true;
                return this;
            }

            bool result = validationBuilder.PropertyValue.Equals(enumType);
            if (result == false)
            {
                validationBuilder.ValidResult = false;
                return this;
            }

            validationBuilder.ValidResult = true;
            return this;
        }

        public FluentValidation<TObject> IsEnumNotEqual(Enum enumType)
        {
            if (validationBuilder.PropertyValue == null)
            {
                validationBuilder.ValidResult = true;
                return this;
            }

            bool result = validationBuilder.PropertyValue.Equals(enumType) == false;
            if (result == false)
            {
                validationBuilder.ValidResult = false;
                return this;
            }

            validationBuilder.ValidResult = true;
            return this;
        }

        public FluentValidation<TObject> AndWhere(Func<TObject, bool> expression)
        {
            if (validationBuilder.ValidResult == true)
            {
                if (expression(internalObject) == true)
                {
                    validationBuilder.ValidResult = true;
                    return this;
                }
                else
                {
                    validationBuilder.ValidResult = false;
                    return this;
                }
            }
            else
            {
                validationBuilder.ValidResult = false;
                return this;
            }
        }

        public FluentValidation<TObject> OrWhere(Func<TObject, bool> expression)
        {
            if (validationBuilder.ValidResult == false)
            {
                if (expression(internalObject))
                {
                    validationBuilder.ValidResult = true;
                    return this;
                }
                else
                {
                    validationBuilder.ValidResult = false;
                    return this;
                }
            }

            validationBuilder.ValidResult = true;
            return this;
        }
    }
}
