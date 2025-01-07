//-----------------------------------------------------------------------
// <copyright file="EventArgs.cs" company="Lifeprojects.de">
//     Class: EventArgs
//     Copyright © Gerhard Ahrens, 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>02.04.2020</date>
//
// <summary>Die Klasse stellt ein generisches EventArgs<T> für EventHandler
//          zur Verfügung.
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;

    /// <summary>
    /// Brings a quick and easy possibility to pass data with the <see cref="System.EventHandler" /> delegate.
    /// </summary>
    /// <typeparam name="T">The type of the containing data.</typeparam>
    /// <example>
    /// <code lang="csharp">
    /// <![CDATA[
    /// public event EventHandler<EventArgs<string>> Something;
    /// 
    /// private void OnSomething(string parameter)
    /// {
    ///     var handler = Something;
    ///     if (handler != null)
    ///         handler(this, new EventArgs<string>(parameter));
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class EventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets the data passed with the <see cref="System.EventHandler" /> delegate.
        /// </summary>
        public T Value { get; private set; }

        /// <summary>
        /// Initializes a new instance of the DW.SharpTools.EventArgs{T} class.
        /// </summary>
        /// <param name="value">The type of the containing data.</param>
        public EventArgs(T value)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Brings a quick and easy possibility to pass data with the <see cref="System.EventHandler" /> delegate.
    /// </summary>
    /// <typeparam name="T1">The first type of the containing data.</typeparam>
    /// <typeparam name="T2">The second type of the containing data.</typeparam>
    /// <example>
    /// <code lang="csharp">
    /// <![CDATA[
    /// public class Demo
    /// {
    ///     public event EventHandler<EventArgs<string, string>> FileMoved;
    /// 
    ///     private void MoveFile()
    ///     {
    ///         var sourceFile = @"C:\Source.txt";
    ///         var destinationFile = @"C:\Destination.txt";
    /// 
    ///         File.Move(sourceFile, destinationFile);
    ///         OnFileMoved(sourceFile, destinationFile);
    ///     }
    /// 
    ///     private void OnFileMoved(string source, string destination)
    ///     {
    ///         var handler = FileMoved;
    ///         if (handler != null)
    ///             handler(this, new EventArgs<string, string>(source, destination));
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class EventArgs<T1, T2> : EventArgs<T1>
    {
        /// <summary>
        /// Gets the second data passed with the <see cref="System.EventHandler" /> delegate.
        /// </summary>
        public T2 Value2 { get; private set; }

        /// <summary>
        /// Initializes a new instance of the DW.SharpTools.EventArgs{T1, T2} class.
        /// </summary>
        /// <param name="value1">The first type of the containing data.</param>
        /// <param name="value2">The second type of the containing data.</param>
        public EventArgs(T1 value1, T2 value2)
            : base(value1)
        {
            Value2 = value2;
        }
    }
    /// <summary>
    /// Brings a quick and easy possibility to pass data with the <see cref="System.EventHandler" /> delegate.
    /// </summary>
    /// <typeparam name="T1">The first type of the containing data.</typeparam>
    /// <typeparam name="T2">The second type of the containing data.</typeparam>
    /// <typeparam name="T3">The third type of the containing data.</typeparam>
    /// <example>
    /// <code lang="csharp">
    /// <![CDATA[
    /// public class Demo
    /// {
    ///     public event EventHandler<EventArgs<string, string, string>> DirectoriesCreated;
    /// 
    ///     private void CreateDirectories()
    ///     {
    ///         var firstDirectory = @"C:\Source";
    ///         var secondDirectory = @"C:\Destination";
    ///         var thirdDirectory = @"C:\Backup";
    /// 
    ///         Directory.CreateDirectory(firstDirectory);
    ///         Directory.CreateDirectory(secondDirectory);
    ///         Directory.CreateDirectory(thirdDirectory);
    ///         OnDirectoriesCreated(firstDirectory, secondDirectory, thirdDirectory);
    ///     }
    /// 
    ///     private void OnDirectoriesCreated(string first, string second, string third)
    ///     {
    ///         var handler = DirectoriesCreated;
    ///         if (handler != null)
    ///             handler(this, new EventArgs<string, string, string>(first, second, third));
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// </example>
    public class EventArgs<T1, T2, T3> : EventArgs<T1, T2>
    {
        /// <summary>
        /// Gets the third data passed with the <see cref="System.EventHandler" /> delegate.
        /// </summary>
        public T3 Value3 { get; private set; }

        /// <summary>
        /// Initializes a new instance of the DW.SharpTools.EventArgs{T1, T2, T3} class.
        /// </summary>
        /// <param name="value1">The first type of the containing data.</param>
        /// <param name="value2">The second type of the containing data.</param>
        /// <param name="value3">The third type of the containing data.</param>
        public EventArgs(T1 value1, T2 value2, T3 value3)
            : base(value1, value2)
        {
            Value3 = value3;
        }
    }

    public class EventArgs<T1, T2, T3, T4> : EventArgs<T1, T2, T3>
    {
        /// <summary>
        /// Gets the third data passed with the <see cref="System.EventHandler" /> delegate.
        /// </summary>
        public T4 Value4 { get; private set; }

        /// <summary>
        /// Initializes a new instance of the DW.SharpTools.EventArgs{T1, T2, T3} class.
        /// </summary>
        /// <param name="value1">The first type of the containing data.</param>
        /// <param name="value2">The second type of the containing data.</param>
        /// <param name="value3">The third type of the containing data.</param>
        public EventArgs(T1 value1, T2 value2, T3 value3, T4 value4)
            : base(value1, value2, value3)
        {
            Value4 = value4;
        }
    }
}
