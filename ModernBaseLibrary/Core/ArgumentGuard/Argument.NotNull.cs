﻿namespace ModernBaseLibrary.Core
{
    using System;

    public static partial class Argument
    {
        private const string NOTNULLMESSAGETEMPLATE = @"[{0}] cannot be Null.";

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null by 
        /// throwing an exception of type <see cref="ArgumentNullException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <typeparam name="TParam">The param Type (any reference type)</typeparam>
        /// <param name="param">The param to be checked</param>
        /// <param name="paramName">The name of the param to be checked, that will be included in the exception</param>        
        public static void NotNull<TParam>(TParam param, string paramName)
            where TParam : class
        {
            NotNull(param, paramName, null);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null by 
        /// throwing an exception of type <see cref="ArgumentNullException"/> with a specific <paramref name="message"/>
        /// when the precondition has not been met
        /// </summary>
        /// <typeparam name="TParam">The param Type (any reference type)</typeparam>
        /// <param name="param">The param to be checked</param>
        /// <param name="paramName">The name of the param to be checked, that will be included in the exception</param>
        /// <param name="message">The message that will be included in the exception</param>
        public static void NotNull<TParam>(TParam param, string paramName, string message)
            where TParam : class
        {
            if (string.IsNullOrWhiteSpace(paramName))
            {
                paramName = GENERICPARAMETERNAME;
            }

            if (message == null)
            {
                message = string.Format(NOTNULLMESSAGETEMPLATE, paramName);
            }

            var argumentNullException = new ArgumentNullException(paramName, message);
            Argument.NotNull(param, argumentNullException);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null by 
        /// throwing an exception of type <typeparamref name="TException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <typeparam name="TParam">The param Type (any reference type)</typeparam>
        /// <typeparam name="TException">The exception Type (Exception) to be thrown if the precondition has not been met</typeparam>
        /// <param name="param">The param to be checked</param>
        public static void NotNull<TParam, TException>(TParam param)
            where TParam : class
            where TException : Exception, new()
        {
            var message = string.Format(NOTNULLMESSAGETEMPLATE, GENERICPARAMETERNAME);

            Argument.NotNull<TParam, TException>(param, message);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null by 
        /// throwing an exception of type <typeparamref name="TException"/> with a specific <paramref name="message"/>
        /// when the precondition has not been met
        /// </summary>
        /// <typeparam name="TParam">The param Type (any reference type)</typeparam>
        /// <typeparam name="TException">The exception Type (Exception) to be thrown if the precondition has not been met</typeparam>
        /// <param name="param">The param to be checked</param>
        /// <param name="message">The message that will be included in the exception</param>
        public static void NotNull<TParam, TException>(TParam param, string message)
            where TParam : class
            where TException : Exception, new()
        {
            if (message == null)
            {
                message = string.Format(NOTNULLMESSAGETEMPLATE, GENERICPARAMETERNAME);
            }

            TException exception = CreateException<TException>(message);

            Argument.NotNull(param, exception);
        }

        /// <summary>
        /// Guards the specified <paramref name="param"/> from being null by 
        /// throwing an <paramref name="exception"/> of type <typeparamref name="TException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <typeparam name="TParam">The param Type (any reference type)</typeparam>
        /// <typeparam name="TException">The exception Type (Exception)</typeparam>
        /// <param name="param">The param to be checked</param>
        /// <param name="exception">The exception to be thrown when the precondition has not been met</param>
        public static void NotNull<TParam, TException>(TParam param, TException exception)
            where TParam : class
            where TException : Exception, new()
        {
            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            Argument.For(() => param == null, exception);
        }
    }
}
