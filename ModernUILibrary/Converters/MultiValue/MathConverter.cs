//-----------------------------------------------------------------------
// <copyright file="MathConverter.cs" company="Lifeprojects.de">
//     Class: MathConverter
//     Copyright © Lifeprojects.de 2016
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>1.1.2016</date>
//
// <summary>
// http://www.ikriv.com/dev/wpf/MathConverter/index.shtml 
// MathConverter Converter Class
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Converters
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows;
    using System.Windows.Data;
    using System.Windows.Markup;

    public class MathConverter : MarkupExtension, IMultiValueConverter, IValueConverter
    {
        private readonly Dictionary<string, IExpression> _storedExpressions = new Dictionary<string, IExpression>();

        private interface IExpression
        {
            double Eval(object[] args);
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Convert(new object[] { value }, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                double result = this.Parse(parameter.ToString()).Eval(values);
                if (targetType == typeof(decimal))
                {
                    return result;
                }

                if (targetType == typeof(string))
                {
                    return result.ToString();
                }

                if (targetType == typeof(int))
                {
                    return (int)result;
                }

                if (targetType == typeof(double))
                {
                    return (double)result;
                }

                if (targetType == typeof(long))
                {
                    return (long)result;
                }

                throw new ArgumentException(string.Format("Unsupported target type {0}", targetType.FullName));
            }
            catch (Exception ex)
            {
                this.ProcessException(ex);
            }

            return DependencyProperty.UnsetValue;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        protected virtual void ProcessException(Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        private IExpression Parse(string s)
        {
            IExpression result = null;
            if (!this._storedExpressions.TryGetValue(s, out result))
            {
                result = new Parser().Parse(s);
                this._storedExpressions[s] = result;
            }

            return result;
        }

        public class Constant : IExpression
        {
            private readonly double _value;

            public Constant(string text)
            {
                if (!double.TryParse(text, out this._value))
                {
                    throw new ArgumentException(string.Format("'{0}' is not a valid number", text));
                }
            }

            public double Eval(object[] args)
            {
                return this._value;
            }
        }

        public class Variable : IExpression
        {
            private readonly int _index;

            public Variable(string text)
            {
                if (!int.TryParse(text, out this._index) || this._index < 0)
                {
                    throw new ArgumentException(string.Format("'{0}' is not a valid parameter index", text));
                }
            }

            public Variable(int n)
            {
                this._index = n;
            }

            public double Eval(object[] args)
            {
                if (this._index >= args.Length)
                {
                    throw new ArgumentException(string.Format("MathConverter: parameter index {0} is out of range. {1} parameter(s) supplied", this._index, args.Length));
                }

                return System.Convert.ToDouble(args[this._index]);
            }
        }

        private class BinaryOperation : IExpression
        {
            private readonly Func<double, double, double> _operation;
            private readonly IExpression _left;
            private readonly IExpression _right;

            public BinaryOperation(char operation, IExpression left, IExpression right)
            {
                this._left = left;
                this._right = right;
                try
                {
                    switch (operation)
                    {
                        case '+':
                            {
                                this._operation = (a, b) => (a + b);
                                break;
                            }

                        case '-':
                            {
                                this._operation = (a, b) => (a - b);
                                break;
                            }

                        case '*':
                            {
                                this._operation = (a, b) => (a * b);
                                break;
                            }

                        case '/':
                            {
                                this._operation = (a, b) => (a / b);
                                break;
                            }

                        default:
                            {
                                throw new ArgumentException("Invalid operation " + operation);
                            }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            public double Eval(object[] args)
            {
                return this._operation(this._left.Eval(args), this._right.Eval(args));
            }
        }

        private class Negate : IExpression
        {
            private readonly IExpression _param;

            public Negate(IExpression param)
            {
                this._param = param;
            }

            public double Eval(object[] args)
            {
                return -this._param.Eval(args);
            }
        }

        private class Parser
        {
            private string text;
            private int pos;

            public IExpression Parse(string text)
            {
                try
                {
                    this.pos = 0;
                    this.text = text;
                    var result = this.ParseExpression();
                    this.RequireEndOfText();
                    return result;
                }
                catch (Exception ex)
                {
                    string msg =
                        string.Format("MathConverter: error parsing expression '{0}'. {1} at position {2}", this.text, ex.Message, this.pos);

                    throw new ArgumentException(msg, ex);
                }
            }

            private IExpression ParseExpression()
            {
                try
                {
                    IExpression left = this.ParseTerm();

                    while (true)
                    {
                        if (this.pos >= this.text.Length)
                        {
                            return left;
                        }

                        var c = this.text[this.pos];

                        if (c == '+' || c == '-')
                        {
                            ++this.pos;
                            IExpression right = this.ParseTerm();
                            left = new BinaryOperation(c, left, right);
                        }
                        else
                        {
                            return left;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            private IExpression ParseTerm()
            {
                IExpression left = this.ParseFactor();

                while (true)
                {
                    if (this.pos >= this.text.Length)
                    {
                        return left;
                    }

                    var c = this.text[this.pos];

                    if (c == '*' || c == '/')
                    {
                        ++this.pos;
                        IExpression right = this.ParseFactor();
                        left = new BinaryOperation(c, left, right);
                    }
                    else
                    {
                        return left;
                    }
                }
            }

            private IExpression ParseFactor()
            {
                this.SkipWhiteSpace();
                if (this.pos >= this.text.Length)
                {
                    throw new ArgumentException("Unexpected end of text");
                }

                var c = this.text[this.pos];

                if (c == '+')
                {
                    ++this.pos;
                    return this.ParseFactor();
                }

                if (c == '-')
                {
                    ++this.pos;
                    return new Negate(this.ParseFactor());
                }

                if (c == 'x' || c == 'a')
                {
                    return this.CreateVariable(0);
                }

                if (c == 'y' || c == 'b')
                {
                    return this.CreateVariable(1);
                }

                if (c == 'z' || c == 'c')
                {
                    return this.CreateVariable(2);
                }

                if (c == 't' || c == 'd')
                {
                    return this.CreateVariable(3);
                }

                if (c == '(')
                {
                    ++this.pos;
                    var expression = this.ParseExpression();
                    this.SkipWhiteSpace();
                    this.Require(')');
                    this.SkipWhiteSpace();
                    return expression;
                }

                if (c == '{')
                {
                    ++this.pos;
                    var end = this.text.IndexOf('}', this.pos);
                    if (end < 0)
                    {
                        --this.pos;
                        throw new ArgumentException("Unmatched '{'");
                    }

                    if (end == this.pos)
                    {
                        throw new ArgumentException("Missing parameter index after '{'");
                    }

                    var result = new Variable(this.text.Substring(this.pos, end - this.pos).Trim());
                    this.pos = end + 1;
                    this.SkipWhiteSpace();
                    return result;
                }

                const string DecimalRegEx = @"(\d+\.?\d*|\d*\.?\d+)";
                var match = Regex.Match(this.text.Substring(this.pos), DecimalRegEx);
                if (match.Success)
                {
                    this.pos += match.Length;
                    this.SkipWhiteSpace();
                    return new Constant(match.Value);
                }
                else
                {
                    throw new ArgumentException(string.Format("Unexpeted character '{0}'", c));
                }
            }

            private IExpression CreateVariable(int n)
            {
                ++this.pos;
                this.SkipWhiteSpace();
                return new Variable(n);
            }

            private void SkipWhiteSpace()
            {
                while (this.pos < this.text.Length && char.IsWhiteSpace(this.text[this.pos]))
                {
                    ++this.pos;
                }
            }

            private void Require(char c)
            {
                if (this.pos >= this.text.Length || this.text[this.pos] != c)
                {
                    throw new ArgumentException("Expected '" + c + "'");
                }

                ++this.pos;
            }

            private void RequireEndOfText()
            {
                if (this.pos != this.text.Length)
                {
                    throw new ArgumentException("Unexpected character '" + this.text[this.pos] + "'");
                }
            }
        }
    }
}
