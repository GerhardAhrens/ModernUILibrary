//-----------------------------------------------------------------------
// <copyright file="TaskExtensions.cs" company="Lifeprojects.de">
//     Class: TaskExtensions
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>10.05.2023 08:26:08</date>
//
// <summary>
// Class for Task<T> Extension
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Collections.Generic;
    using System.Linq;

    public static class TaskExtensions
    {
        public static Task<T> AsTask<T>(this T @this)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();
            taskCompletionSource.SetResult(@this);
            return taskCompletionSource.Task;
        }

        /// <summary>Creates a Task that has completed in the Faulted state with the specified exception.</summary>
        /// <typeparam name="TResult">Specifies the type of payload for the new Task.</typeparam>
        /// <param name="this">The target TaskFactory.</param>
        /// <param name="exception">The exception with which the Task should fault.</param>
        /// <returns>The completed Task.</returns>
        public static Task<TResult> FromException<TResult>(this TaskFactory @this, Exception exception)
        {
            var tcs = new TaskCompletionSource<TResult>(@this.CreationOptions);
            tcs.SetException(exception);
            return tcs.Task;
        }

        /// <summary>
        /// Executes a task asynchronously on all elements of a sequence in parallel and returns results.
        /// </summary>
        public static async Task<IEnumerable<TResult>> ParallelSelectAsync<T, TResult>(this IEnumerable<T> @this, Func<T, Task<TResult>> taskFunc)
        {
            return await Task.WhenAll(@this.Select(taskFunc)).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes a task asynchronously on all elements of a sequence in parallel.
        /// </summary>
        public static async Task ParallelForEachAsync<T>(this IEnumerable<T> @this, Func<T, Task> taskFunc)
        {
            await Task.WhenAll(@this.Select(taskFunc)).ConfigureAwait(false);
        }
    }
}