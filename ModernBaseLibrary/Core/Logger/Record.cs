//-----------------------------------------------------------------------
// <copyright file="Record.cs" company="Lifeprojects.de">
//     Class: Record
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.10.2022</date>
//
// <summary>Definition of Logger Record Class</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Logger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Logging Record.
    /// </summary>
    [DebuggerDisplay("LoggerName={this.LoggerName};DateTime={this.EntryDateTime};Level={this.Level};Message={this.Message}")]
    [DebuggerStepThrough()]
    public sealed class Record
    {
        public Record(string loggerName, LogLevel logLevel, StackTrace stacktrace, string msg, string func, StackFrame callerStackFrame, Exception e)
        {
            this.LoggerName = loggerName;
            this.EntryDateTime = DateTime.Now;
            this.Level = logLevel;
            this.Stack = stacktrace;
            this.Message = msg;
            this.FunctionName = func;
            this.CallerStackFrame = callerStackFrame;
            this.Exception = e;
        }

        /// <summary>
        /// Which logger record.
        /// </summary>
        public string LoggerName { get; private set; }

        public DateTime EntryDateTime { get; private set; }

        /// <summary>
        /// Record's log level.
        /// </summary>
        public LogLevel Level { get; private set; }

        /// <summary>
        /// Record's stack trace.
        /// </summary>
        public StackTrace Stack { get; private set; }

        /// <summary>
        /// Log message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Logging file name.
        /// </summary>
        public string FileName
        {
            get
            {
                return this.CallerStackFrame.GetFileName();
            }
        }

        /// <summary>
        /// Which method logged.
        /// </summary>
        public string FunctionName { get; set; }

        /// <summary>
        /// Line number call log.
        /// </summary>
        public int LineNumber
        {
            get
            {
                return this.CallerStackFrame.GetFileLineNumber();
            }
        }

        /// <summary>
        /// The stackframe which call logger.
        /// </summary>
        public StackFrame CallerStackFrame { get; private set; }

        /// <summary>
        /// Log exception.
        /// </summary>
        public Exception Exception { get; private set; }

    }
}
