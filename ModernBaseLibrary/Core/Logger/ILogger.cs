//-----------------------------------------------------------------------
// <copyright file="LoggerBaseException.cs" company="Lifeprojects.de">
//     Class: LoggerBaseException
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.10.2022</date>
//
// <summary>Interface Definition of Logger ILogger Class</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Logger
{
    using System;
    /// <summary>
    /// Logger interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Name getter.
        /// </summary>
        string Name { get; }

        int CountHandler { get; }

        void SetLevel(LogLevel level);

        void AddHandler(IHandler handler);

        void Critical(string message);

        void Critical(Exception e, string message);

        void Error(string message);

        void Error(Exception e, string message);

        void Warning(string message);

        void Warning(Exception e, string message);

        void Info(string message, bool isAutoFlush = false);

        void Info(Exception e, string message, bool isAutoFlush = false);

        void Debug(string message);

        void Debug(Exception e, string message);

        void WriteLog(LogLevel level, string message);

        void WriteLog(LogLevel level, string message,Exception e);

        void Flush();

        Task FlushAsync();
    }
}
