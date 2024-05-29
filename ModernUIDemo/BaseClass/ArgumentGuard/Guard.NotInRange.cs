
namespace ModernUIDemo.Core
{
    using System;

    public static partial class Guard
    {
        private const string NOTINRANGETEMPLATE = @"[{0}] cannot be in Range between {1} and {2}.";

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being greater than the specified <paramref name="minValue"/> by 
        /// throwing an exception of type <see cref="ArgumentOutOfRangeException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <typeparam name="TParam">The param Type (any value type)</typeparam>
        /// <param name="param">The param to be checked</param>
        /// <param name="minValue">The minValue against which the param will be checked</param>
        /// <param name="paramName">The name of the param to be checked, that will be included in the exception</param>
        public static void NotInRange<TParam>(TParam param, TParam minValue, TParam maxValue, string paramName) 
            where TParam : IComparable<TParam>
        {
            Guard.NotInRange(param, minValue, maxValue, paramName, null);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being greater than the specified <paramref name="minValue"/> by 
        /// throwing an exception of type <see cref="ArgumentOutOfRangeException"/> with a specific <paramref name="message"/>
        /// when the precondition has not been met
        /// </summary>
        /// <typeparam name="TParam">The param Type (any value type)</typeparam>
        /// <param name="param">The param to be checked</param>
        /// <param name="minValue">The minValue against which the param will be checked</param>
        /// <param name="paramName">The name of the param to be checked, that will be included in the exception</param>
        /// <param name="message">The message that will be included in the exception</param>
        public static void NotInRange<TParam>(TParam param, TParam minValue, TParam maxValue, string paramName, string message) 
            where TParam : IComparable<TParam>
        {
            if (string.IsNullOrWhiteSpace(paramName))
            {
                paramName = GENERICPARAMETERNAME;
            }

            if (message == null)
            {
                message = string.Format(NOTINRANGETEMPLATE, paramName, minValue, maxValue);
            }

            var argumentOutOfRangeException = new ArgumentOutOfRangeException(paramName, message);
            Guard.NotInRange(param, minValue, maxValue, argumentOutOfRangeException);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being greater than the specified <paramref name="minValue"/> by 
        /// throwing an exception of type <typeparamref name="TException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <typeparam name="TParam">The param Type (any value type)</typeparam>
        /// <typeparam name="TException">The exception Type (Exception)</typeparam>
        /// <param name="param">The param to be checked</param>
        /// <param name="minValue">The minValue against which the param will be checked</param>
        public static void NotInRange<TParam, TException>(TParam param, TParam minValue, TParam maxValue)
            where TParam : IComparable<TParam>
            where TException : Exception, new()
        {
            var message = string.Format(NOTINRANGETEMPLATE, GENERICPARAMETERNAME, minValue, maxValue);

            Guard.NotInRange<TParam, TException>(param, minValue, maxValue, message);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being greater than the specified <paramref name="minValue"/> by 
        /// throwing an exception of type <typeparamref name="TException"/> with a specific <paramref name="message"/>
        /// when the precondition has not been met
        /// </summary>
        /// <typeparam name="TParam">The param Type (any value type)</typeparam>
        /// <typeparam name="TException">The exception Type (Exception)</typeparam>
        /// <param name="param">The param to be checked</param>
        /// <param name="minValue">The minValue against which the param will be checked</param>
        /// <param name="message">The message that will be included in the exception</param>
        public static void NotInRange<TParam, TException>(TParam param, TParam minValue, TParam maxValue, string message)
            where TParam : IComparable<TParam>
            where TException : Exception, new()
        {
            if (message == null)
            {
                message = string.Format(NOTINRANGETEMPLATE, GENERICPARAMETERNAME, minValue, maxValue);
            }

            TException exception = CreateException<TException>(message);

            Guard.NotInRange(param, minValue, maxValue, exception);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being greater than the specified <paramref name="minValue"/> by 
        /// throwing an exception of type <typeparamref name="TException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <typeparam name="TParam">The param Type (any value type)</typeparam>
        /// <typeparam name="TException">The exception Type (Exception)</typeparam>
        /// <param name="param">The param to be checked</param>
        /// <param name="minValue">The minValue against which the param will be checked</param>
        /// <param name="exception">The exception to be thrown when the precondition has not been met</param>
        public static void NotInRange<TParam, TException>(TParam param, TParam minValue, TParam maxValue, TException exception)
            where TParam : IComparable<TParam>
            where TException : Exception, new()
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Guard.For(() => !(param.CompareTo(minValue) >= 0 && param.CompareTo(maxValue) <= 0), exception);
        }
    }
}
