﻿
namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Argument
    {
        private const string NOTANYTEMPLATE = @"[{0}] cannot be empty (should contain at least one element).";

        /// <summary>
        /// Guards the specified <paramref name="param"/> from containing no elements by 
        /// throwing an exception of type <see cref="ArgumentException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <param name="param">The param to be checked</param>
        /// <param name="paramName">The name of the param to be checked, that will be included in the exception</param>
        public static void NotAny<T>(IEnumerable<T> param, string paramName)
        {
            NotAny(param, paramName, null);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from containing no elements by 
        /// throwing an exception of type <see cref="ArgumentException"/> with a specific <paramref name="message"/>
        /// when the precondition has not been met
        /// </summary>
        /// <param name="param">The param to be checked</param>
        /// <param name="paramName">The name of the param to be checked, that will be included in the exception</param>
        /// <param name="message">The message that will be included in the exception</param>
        public static void NotAny<T>(IEnumerable<T> param, string paramName, string message)
        {
            if (string.IsNullOrWhiteSpace(paramName))
            {
                paramName = GENERICPARAMETERNAME;
            }

            if (message == null)
            {
                message = String.Format(NOTANYTEMPLATE, paramName);
            }

            var argumentException = new ArgumentException(message, paramName);
            Argument.NotAny(param, argumentException);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from containing no elements by 
        /// throwing an exception of type <typeparamref name="TException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <param name="param">The param to be checked</param>
        public static void NotAny<T, TException>(IEnumerable<T> param)
            where TException : Exception, new()
        {
            var message = string.Format(NOTANYTEMPLATE, GENERICPARAMETERNAME);

            Argument.NotAny<T, TException>(param, message);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from containing no elements by 
        /// throwing an exception of type <typeparamref name="TException"/> with a specific <paramref name="message"/>
        /// when the precondition has not been met
        /// </summary>
        /// <param name="param">The param to be checked</param>
        /// <param name="message">The message that will be included in the exception</param>
        public static void NotAny<T, TException>(IEnumerable<T> param, string message)
            where TException : Exception, new()
        {
            if (message == null)
            {
                message = string.Format(NOTANYTEMPLATE, GENERICPARAMETERNAME);
            }

            TException exception = CreateException<TException>(message);

            Argument.NotAny(param, exception);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from containing no elements by 
        /// throwing an <paramref name="exception"/> of type <typeparamref name="TException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <typeparam name="TException">The exception Type (Exception)</typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="param">The param to be checked</param>
        /// <param name="exception">The exception to be thrown when the precondition has not been met</param>
        public static void NotAny<T, TException>(IEnumerable<T> param, TException exception) where TException : Exception, new()
        {
            if (param == null)
            {
                throw new ArgumentNullException(nameof(param));
            }

            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Argument.For(() => !param.Any(), exception);
        }
    }
}
