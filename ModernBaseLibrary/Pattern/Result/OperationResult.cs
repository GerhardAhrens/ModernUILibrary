//-----------------------------------------------------------------------
// <copyright file="OperationResult.cs" company="Lifeprojects.de">
//     Class: OperationResult<TResult>
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

    public sealed class OperationResult<TResult>
    {

        public DateTime OperationResultTime { get; set; }

        public bool? OperationResultState { get; set; }

        public bool Success { get; private set; }

        public TResult Result { get; private set; }

        public string NonSuccessMessage { get; private set; }

        public string SuccessMessage { get; private set; }

        public Exception Exception { get; private set; }

        public static OperationResult<TResult> SuccessResult(TResult result, string successMessage, bool? operationResultState, DateTime operationResultTime)
        {
            return new OperationResult<TResult>
            {
                OperationResultTime = operationResultTime,
                OperationResultState = operationResultState,
                Success = true,
                Result = result,
                SuccessMessage = successMessage
            };
        }

        public static OperationResult<TResult> SuccessResult(TResult result, bool? operationResultState, DateTime operationResultTime)
        {
            return new OperationResult<TResult>
            {
                OperationResultTime = operationResultTime,
                OperationResultState = operationResultState,
                Success = true,
                Result = result,
                SuccessMessage = string.Empty
            };
        }

        public static OperationResult<TResult> SuccessResult(TResult result, DateTime operationResultTime)
        {
            return new OperationResult<TResult>
            {
                OperationResultTime = operationResultTime,
                OperationResultState = null,
                Success = true,
                Result = result,
                SuccessMessage = string.Empty
            };
        }


        public static OperationResult<TResult> SuccessResult(TResult result, bool? operationResultState)
        {
            return new OperationResult<TResult>
            {
                OperationResultState = operationResultState,
                Success = true,
                Result = result,
                SuccessMessage = string.Empty
            };
        }

        public static OperationResult<TResult> SuccessResult(TResult result)
        {
            return new OperationResult<TResult>
            {
                OperationResultState = null,
                Success = true,
                Result = result,
                SuccessMessage = string.Empty
            };
        }

        public static OperationResult<TResult> Failure(string nonSuccessMessage)
        {
            return new OperationResult<TResult>
            {
                OperationResultState = null,
                Success = false,
                NonSuccessMessage = nonSuccessMessage
            };
        }

        public static OperationResult<TResult> Failure(Exception ex)
        {
            return new OperationResult<TResult>
            {
                OperationResultState = null,
                Success = false,
                NonSuccessMessage = $"{ex.Message}{Environment.NewLine}{ex.StackTrace}",
                Exception = ex
            };
        }

        public static OperationResult<TResult> Failure(Exception ex, bool? operationResultState = null)
        {
            return new OperationResult<TResult>
            {
                OperationResultState = operationResultState,
                Success = false,
                NonSuccessMessage = $"{ex.Message}{Environment.NewLine}{ex.StackTrace}",
                Exception = ex
            };
        }
    }
}
