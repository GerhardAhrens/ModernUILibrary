﻿namespace ModernBaseLibrary.Core
{
    using System;

    public static partial class Argument
    {
        private const string NOTNULLORWHITESPACETEMPLATE = @"[{0}] cannot be Null, empty or white-space.";

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null, empty or white-space by 
        /// throwing an exception of type <see cref="ArgumentException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <param name="param">The param to be checked</param>
        /// <param name="paramName">The name of the param to be checked, that will be included in the exception</param>
        public static void NotNullOrWhitespace(string param, string paramName)
        {
            NotNullOrWhitespace(param, paramName, null);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null, empty or white-space by 
        /// throwing an exception of type <see cref="ArgumentException"/> with a specific <paramref name="message"/>
        /// when the precondition has not been met
        /// </summary>
        /// <param name="param">The param to be checked</param>
        /// <param name="paramName">The name of the param to be checked, that will be included in the exception</param>
        /// <param name="message">The message that will be included in the exception</param>
        public static void NotNullOrWhitespace(string param, string paramName, string message)
        {
            if (String.IsNullOrWhiteSpace(paramName))
            {
                paramName = GENERICPARAMETERNAME;
            }

            if (message == null)
            {
                message = String.Format(NOTNULLORWHITESPACETEMPLATE, paramName);
            }

            var argumentException = new ArgumentException(message, paramName);
            Argument.NotNullOrWhitespace(param, argumentException);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null, empty or white-space by 
        /// throwing an exception of type <typeparamref name="TException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <param name="param">The param to be checked</param>
        public static void NotNullOrWhitespace<TException>(string param)
            where TException : Exception, new()
        {
            var message = String.Format(NOTNULLORWHITESPACETEMPLATE, GENERICPARAMETERNAME);

            Argument.NotNullOrWhitespace<TException>(param, message);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null, empty or white-space by 
        /// throwing an exception of type <typeparamref name="TException"/> with a specific <paramref name="message"/>
        /// when the precondition has not been met
        /// </summary>
        /// <param name="param">The param to be checked</param>
        /// <param name="message">The message that will be included in the exception</param>
        public static void NotNullOrWhitespace<TException>(string param, string message)
            where TException : Exception, new()
        {
            if (message == null)
            {
                message = String.Format(NOTNULLORWHITESPACETEMPLATE, GENERICPARAMETERNAME);
            }

            TException exception = CreateException<TException>(message);

            Argument.NotNullOrWhitespace(param, exception);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null, empty or white-space by 
        /// throwing an <paramref name="exception"/> when the precondition has not been met
        /// </summary>
        /// <typeparam name="TException">The exception Type (Exception)</typeparam>
        /// <param name="param">The param to be checked</param>
        /// <param name="exception">The exception to be thrown when the precondition has not been met</param>
        public static void NotNullOrWhitespace<TException>(string param, TException exception)
            where TException : Exception, new()
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Argument.For(() => String.IsNullOrWhiteSpace(param), exception);
        }
    }
}
