//-----------------------------------------------------------------------
// <copyright file="FluentValidation.cs" company="Lifeprojects.de">
//     Class: FluentValidation
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>20.07.2020</date>
//
// <summary>
//      Class for FluentValidation
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Runtime.Versioning;
    using System.Text.RegularExpressions;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    public partial class FluentValidation<TObject>
    {
        private static ValidationBuilder validationBuilder = null;
        private static TObject internalObject;

        public FluentValidation(TObject obj)
        {
            if (validationBuilder != null)
            {
                internalObject = obj;
                validationBuilder.PropertyType = null;
                validationBuilder.ValidMessage = string.Empty;
                validationBuilder.ValidResult = false;
            }
            else
            {
                internalObject = obj;
                validationBuilder = new ValidationBuilder();
                validationBuilder.PropertyType = null;
                validationBuilder.ValidMessage = string.Empty;
                validationBuilder.ValidResult = false;
            }
        }

        public FluentValidation<TObject>RuleFor(Expression<Func<TObject, object>> expression)
        {
            string propertyName = ExpressionPropertyName.For<TObject>(expression);
            var propertyValue = internalObject.GetType().GetProperty(propertyName).GetValue(internalObject, null);

            validationBuilder.PropertyName = propertyName;
            validationBuilder.PropertyValue = propertyValue;
            validationBuilder.PropertyType = propertyValue?.GetType();

            return this;
        }

        public FluentValidation<TObject> RuleFor(Func<TObject, bool> predicate)
        {
            if (predicate(internalObject))
            {
                validationBuilder.ValidResult = false;
            }
            else
            {
                validationBuilder.ValidResult = true;
            }

            return this;
        }

        public static FluentValidation<TObject> RuleFor(Expression<Func<TObject, bool>> expression)
        {
            var binExpr = expression.Body as BinaryExpression;
            Expression valueL = binExpr.Left;
            Expression valueR = binExpr.Right;

            return new FluentValidation<TObject>(internalObject);
        }

        public static FluentValidation<TObject> This(TObject content)
        {
            return new FluentValidation<TObject>(content);
        }

        public FluentValidation<TObject> IsEmpty()
        {
            switch (validationBuilder.PropertyValue)
            {
                case null:
                    {
                        validationBuilder.ValidResult = false;
                        return this;
                    }

                case string s when string.IsNullOrWhiteSpace(s):
                    {
                        validationBuilder.ValidResult = false;
                        return this;
                    }

                case DateTime s when ((DateTime)validationBuilder.PropertyValue).IsDateEmpty():
                    {
                        validationBuilder.ValidResult = false;
                        return this;
                    }

                case float s when s == 0:
                    {
                        validationBuilder.ValidResult = false;
                        return this;
                    }

                case int s when s == 0:
                    {
                        validationBuilder.ValidResult = false;
                        return this;
                    }

                case long s when s == 0:
                    {
                        validationBuilder.ValidResult = false;
                        return this;
                    }

                case decimal s when s == 0:
                    {
                        validationBuilder.ValidResult = false;
                        return this;
                    }

                case IList c when c.Count == 0:
                    {
                        validationBuilder.ValidResult = false;
                        return this;
                    }

                case ICollection c when c.Count == 0:
                    {
                        validationBuilder.ValidResult = false;
                        return this;
                    }

                case IDictionary c when c.Count == 0:
                    {
                        validationBuilder.ValidResult = false;
                        return this;
                    }

                case Array a when a.Length == 0:
                    {
                        validationBuilder.ValidResult = false;
                        return this;
                    }
            }

            validationBuilder.ValidResult = true;
            return this;
        }

        public FluentValidation<TObject> IsDateBeforToday()
        {
            if (validationBuilder.PropertyValue is DateTime)
            {
                DateTime currentDateTime = (DateTime)validationBuilder.PropertyValue;
                if (currentDateTime < DateTime.Now)
                {
                    validationBuilder.ValidResult = false;
                    return this;
                }
                else
                {
                    validationBuilder.ValidResult = true;
                    return this;
                }
            }

            return this;
        }

        public FluentValidation<TObject> IsDateAfterToday()
        {
            if (validationBuilder.PropertyValue is DateTime)
            {
                DateTime currentDateTime = (DateTime)validationBuilder.PropertyValue;
                if (currentDateTime > DateTime.Now)
                {
                    validationBuilder.ValidResult = false;
                    return this;
                }
                else
                {
                    validationBuilder.ValidResult = true;
                    return this;
                }
            }

            return this;
        }


        public FluentValidation<TObject> Length(int max)
        {
            if (validationBuilder.PropertyValue == null)
            {
                validationBuilder.FromValue = null;
                validationBuilder.ToValue = max;
                validationBuilder.ValidResult = false;
                return this;
            }

            string value = validationBuilder.PropertyValue.ToString();
            bool result = this.IsInRangeInternal<int>(value.Length, 0, max);
            if (result == false)
            {
                validationBuilder.FromValue = null;
                validationBuilder.ToValue = max;
                validationBuilder.ValidResult = false;
                return this;
            }

            validationBuilder.ValidResult = true;
            return this;
        }

        public FluentValidation<TObject> Length(int min, int max)
        {
            if (validationBuilder.PropertyValue == null)
            {
                validationBuilder.FromValue = min;
                validationBuilder.ToValue = max;
                validationBuilder.ValidResult = false;
                return this;
            }

            string value = validationBuilder.PropertyValue.ToString();
            bool result = this.IsInRangeInternal<int>(value.Length, min, max);
            if (result == false)
            {
                validationBuilder.FromValue = min;
                validationBuilder.ToValue = max;
                validationBuilder.ValidResult = false;
                return this;
            }

            validationBuilder.ValidResult = true;
            return this;
        }

        public FluentValidation<TObject> IsEMail()
        {
            if (validationBuilder.PropertyValue == null)
            {
                validationBuilder.ValidResult = true;
                return this;
            }

            Regex regEx = this.CreateRegExForEMail();

            if (regEx.IsMatch(validationBuilder.PropertyValue.ToString()) == false)
            {
                validationBuilder.ValidResult = false;
                return this;
            }

            validationBuilder.ValidResult = true;
            return this;
        }

        public ValidationBuilder Message(string message)
        {
            if (validationBuilder?.ValidResult == false)
            {
                IEnumerable<string> resultTag = message.ExtractFromString("{", "}");
                if (resultTag.Count() > 0 && resultTag.Count() == 2)
                {
                    if (validationBuilder.FromValue != null)
                    {
                        message = message.Replace($"{{{resultTag.First()}}}", $"'{validationBuilder.FromValue}'");
                    }

                    if (validationBuilder.ToValue != null)
                    {
                        message = message.Replace($"{{{resultTag.Last()}}}", $"'{validationBuilder.ToValue}'");
                    }
                }
                else if (resultTag.Count() > 0 && resultTag.Count() == 1)
                {
                    if (validationBuilder.FromValue != null)
                    {
                        message = message.Replace($"{{{resultTag.First()}}}", $"'{validationBuilder.FromValue}'");
                    }

                    if (validationBuilder.ToValue != null)
                    {
                        message = message.Replace($"{{{resultTag.First()}}}", $"'{validationBuilder.ToValue}'");
                    }
                }

                validationBuilder.ValidMessage = message;
            }
            else
            {
                if(validationBuilder != null)
                {
                    validationBuilder.ValidMessage = string.Empty;
                }
            }

            return validationBuilder;
        }

        public ValidationBuilder IsValid()
        {
            validationBuilder.ValidMessage = "Set Custom Message";
            return validationBuilder;
        }

        private Regex CreateRegExForEMail()
        {
            const string Pattern = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";
            const RegexOptions Options = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture;
            return new Regex(Pattern, Options, TimeSpan.FromSeconds(2.0));
        }

        private bool IsOutRangeInternal<TTyp>(TTyp @this, TTyp lowest, TTyp highest)
        {
            return Comparer<TTyp>.Default.Compare(lowest, @this) > 0 && Comparer<TTyp>.Default.Compare(highest, @this) < 0;
        }

        private bool IsOutRangeEqualInternal<TTyp>(TTyp @this, TTyp lowest, TTyp highest)
        {
            return Comparer<TTyp>.Default.Compare(lowest, @this) >= 0 && Comparer<TTyp>.Default.Compare(highest, @this) <= 0;
        }

        private bool IsInRangeInternal<TTyp>(TTyp @this, TTyp lowest, TTyp highest)
        {
            return Comparer<TTyp>.Default.Compare(lowest, @this) < 0 && Comparer<TTyp>.Default.Compare(highest, @this) > 0;
        }

        private bool IsInRangeEqualInternal<TTyp>(TTyp @this, TTyp lowest, TTyp highest)
        {
            return Comparer<TTyp>.Default.Compare(lowest, @this) <= 0 && Comparer<TTyp>.Default.Compare(highest, @this) >= 0;
        }
    }
}
