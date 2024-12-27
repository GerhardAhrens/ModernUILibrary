//-----------------------------------------------------------------------
// <copyright file="Result.cs" company="Lifeprojects.de">
//     Class: Result<TResult>
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.06.2022</date>
//
// <summary>
// Result Operation Pattern
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;

    public sealed class Result<TResult>
    {
        public long ElapsedMilliseconds { get; set; }

        public bool? ResultState { get; set; }

        public bool Success { get; private set; }

        public TResult Value { get; private set; }

        public string FailureMessage { get; private set; }

        public string SuccessMessage { get; private set; }

        public Exception Exception { get; private set; }

        public string ElapsedTime
        { 
            get { return this.ToMillisecondsFormat(this.ElapsedMilliseconds); } 
        }

        public static Result<TResult> SuccessResult(TResult result, string successMessage, bool? operationResultState, long elapsedMilliseconds)
        {
            return new Result<TResult>
            {
                ElapsedMilliseconds = elapsedMilliseconds,
                ResultState = operationResultState,
                Success = true,
                Value = result,
                SuccessMessage = successMessage
            };
        }

        public static Result<TResult> SuccessResult(TResult result, bool? operationResultState, long elapsedMilliseconds)
        {
            return new Result<TResult>
            {
                ElapsedMilliseconds = elapsedMilliseconds,
                ResultState = operationResultState,
                Success = true,
                Value = result,
                SuccessMessage = string.Empty
            };
        }

        public static Result<TResult> SuccessResult(TResult result, long elapsedMilliseconds)
        {
            return new Result<TResult>
            {
                ElapsedMilliseconds = elapsedMilliseconds,
                ResultState = null,
                Success = true,
                Value = result,
                SuccessMessage = string.Empty
            };
        }


        public static Result<TResult> SuccessResult(TResult result, bool? operationResultState)
        {
            return new Result<TResult>
            {
                ResultState = operationResultState,
                Success = true,
                Value = result,
                SuccessMessage = string.Empty
            };
        }

        public static Result<TResult> SuccessResult(TResult result)
        {
            return new Result<TResult>
            {
                ResultState = null,
                Success = true,
                Value = result,
                SuccessMessage = string.Empty
            };
        }

        public static Result<TResult> FailureResult(string nonSuccessMessage)
        {
            return new Result<TResult>
            {
                ResultState = null,
                Success = false,
                FailureMessage = nonSuccessMessage
            };
        }

        public static Result<TResult> FailureResult(Exception ex)
        {
            return new Result<TResult>
            {
                ResultState = null,
                Success = false,
                FailureMessage = $"{ex.Message}{Environment.NewLine}{ex.StackTrace}",
                Exception = ex
            };
        }

        public static Result<TResult> FailureResult(Exception ex, bool? resultState = null)
        {
            return new Result<TResult>
            {
                ResultState = resultState,
                Success = false,
                FailureMessage = $"{ex.Message}{Environment.NewLine}{ex.StackTrace}",
                Exception = ex
            };
        }

        private string ToMillisecondsFormat(long ticks)
        {
            TimeSpan duration = new TimeSpan(ticks);
            return $"{duration.Hours:00}:{duration.Minutes:00}:{duration.Seconds:00}";
        }
    }
}
