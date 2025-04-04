﻿namespace ModernBaseLibrary.Core
{
    using System;
    using System.Runtime.Versioning;

    /// <summary>
    /// Facilitates runtime checks of code and allows to define preconditions and invariants within a method.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public static partial class Argument
    {
        private const string GENERICPARAMETERNAME = "parameter";
        private const string FORMESSAGETEMPLATE = "Precondition not met.";

        /// <summary>
        /// Guards the specified <paramref name="predicate"/> from being violated by 
        /// throwing an exception of type <typeparamref name="TException"/>
        /// when the precondition has not been met
        /// </summary>
        /// <param name="predicate">The precondition that has to be met</param>
        public static void For<TException>(Func<bool> predicate)
            where TException : Exception, new()
        {
            Argument.For<TException>(predicate, FORMESSAGETEMPLATE);
        }

        /// <summary>
        /// Guards the specified <paramref name="predicate"/> from being violated by 
        /// throwing an exception of type <typeparamref name="TException"/> with a specific <paramref name="message"/>
        /// when the precondition has not been met
        /// </summary>
        /// <param name="predicate">The precondition that has to be met</param>
        /// <param name="message">The message that will be included in the exception</param>
        public static void For<TException>(Func<bool> predicate, string message)
            where TException : Exception, new()
        {
            if (message == null)
            {
                message = FORMESSAGETEMPLATE;
            }

            TException exception = CreateException<TException>(message);

            Argument.For(predicate, exception);
        }

        /// <summary>
        /// Guards the specified <paramref name="predicate"/> from being violated by 
        /// throwing an <paramref name="exception"/> when the precondition has not been met
        /// </summary>
        /// <typeparam name="TException">The exception Type (Exception)</typeparam>
        /// <param name="predicate">The precondition that has to be met</param>
        /// <param name="exception">The exception to be thrown when the precondition has not been met</param>
        public static void For<TException>(Func<bool> predicate, TException exception)
            where TException : Exception, new()
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (exception == null)
            {
                throw new ArgumentNullException(nameof(exception));
            }

            bool conditionNotMet = predicate.Invoke();
            if (conditionNotMet)
            {
                throw exception;
            }
        }

        private static TException CreateException<TException>(string message = null)
            where TException : Exception, new()
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return new TException();
            }

            try
            {
                return (TException)Activator.CreateInstance(typeof(TException), message);
            }
            catch (MissingMethodException)
            {
                return new TException();
            }
        }
    }
}
