﻿namespace ModernBaseLibrary.Core
{
    using System;

    public static partial class Argument
    {
        private const string NOTNULLOREMPTYTEMPLATE = @"[{0}] cannot be Null or empty.";

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null or empty (white-space allowed) by 
        /// throwing an exception of type <see cref="ArgumentException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <param name="param">The param to be checked</param>
        /// <param name="paramName">The name of the param to be checked, that will be included in the exception</param>
        public static void NotNullOrEmpty(string param, string paramName)
        {
            NotNullOrEmpty(param, paramName, null);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null or empty (white-space allowed) by 
        /// throwing an exception of type <see cref="ArgumentException"/> with a specific <paramref name="message"/>
        /// when the precondition has not been met
        /// </summary>
        /// <param name="param">The param to be checked</param>
        /// <param name="paramName">The name of the param to be checked, that will be included in the exception</param>
        /// <param name="message">The message that will be included in the exception</param>
        public static void NotNullOrEmpty(string param, string paramName, string message)
        {
            if (String.IsNullOrWhiteSpace(paramName))
            {
                paramName = GENERICPARAMETERNAME;
            }

            if (message == null)
            {
                message = String.Format(NOTNULLOREMPTYTEMPLATE, paramName);
            }

            var argumentException = new ArgumentException(message, paramName);
            Argument.NotNullOrEmpty(param, argumentException);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null or empty (white-space allowed) by 
        /// throwing an exception of type <typeparamref name="TException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <param name="param">The param to be checked</param>
        public static void NotNullOrEmpty<TException>(string param)
            where TException : Exception, new()
        {
            var message = String.Format(NOTNULLOREMPTYTEMPLATE, GENERICPARAMETERNAME);

            Argument.NotNullOrEmpty<TException>(param, message);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null or empty (white-space allowed) by 
        /// throwing an exception of type <typeparamref name="TException"/> with a specific <paramref name="message"/>
        /// when the precondition has not been met
        /// </summary>
        /// <param name="param">The param to be checked</param>
        /// <param name="message">The message that will be included in the exception</param>
        public static void NotNullOrEmpty<TException>(string param, string message)
            where TException : Exception, new()
        {
            if (message == null)
            {
                message = String.Format(NOTNULLOREMPTYTEMPLATE, GENERICPARAMETERNAME);
            }

            TException exception = CreateException<TException>(message);

            Argument.NotNullOrEmpty(param, exception);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null or empty (white-space allowed) by 
        /// throwing an <paramref name="exception"/> of type <typeparamref name="TException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <typeparam name="TException">The exception Type (Exception)</typeparam>
        /// <param name="param">The param to be checked</param>
        /// <param name="exception">The exception to be thrown when the precondition has not been met</param>
        public static void NotNullOrEmpty<TException>(string param, TException exception)
            where TException : Exception, new()
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Argument.For(() => String.IsNullOrEmpty(param), exception);
        }
    }
}
