//-----------------------------------------------------------------------
// <copyright file="Logger.cs" company="Lifeprojects.de">
//     Class: Logger
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.10.2022</date>
//
// <summary>
// A thread safe basic logger
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Logger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// A thread safe basic logger.
    /// </summary>
    //[DebuggerStepThrough()]
    public sealed class Logger : ILogger
    {
        private string loggerName;
        private LogLevel logLevel;
        private HashSet<IHandler> handlerList;
        private object syncObj = new object();

        /// <summary>
        /// Logger constractor.
        /// Default log level is NOSET.
        /// </summary>
        /// <param name="loggerName"></param>
        public Logger(string loggerName)
        {
            this.Init(loggerName, LogLevel.NOTSET);
        }

        public Logger(string loggerName, LogLevel logLevel)
        {
            this.Init(loggerName, logLevel);
        }

        /// <summary>
        /// Logger Name property getter.
        /// </summary>
        public string Name 
        {
            get
            {
                return this.loggerName;
            }
        }

        public int CountHandler
        {
            get
            {
                return this.handlerList.Count;
            }
        }

        /// <summary>
        /// Set log level to this logger.
        /// </summary>
        /// <param name="level"></param>
        public void SetLevel(LogLevel level)
        {
            lock (this.syncObj)
            {
                this.logLevel = level;
            }
        }

        /// <summary>
        /// Log Critical.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Critical(string message)
        {
            if (!this.CanLog(LogLevel.CRITICAL))
            {
                return;
            }

            this.PushLog(LogLevel.CRITICAL, message,null);
        }

        /// <summary>
        /// Log Critical with exception.
        /// </summary>
        /// <param name="e">Exception</param>
        /// <param name="message">Log message</param>
        public void Critical(Exception e,string message)
        {
            if (!this.CanLog(LogLevel.CRITICAL))
            {
                return;
            }

            this.PushLog(LogLevel.CRITICAL, message,e);
        }

        /// <summary>
        /// Log Error.
        /// </summary>
        /// <param name="message">Log message.</param>
        public void Error(string message)
        {
            if (!this.CanLog(LogLevel.ERROR))
            {
                return;
            }

            this.PushLog(LogLevel.ERROR, message,null);
        }


        /// <summary>
        /// Log Error with exception.
        /// </summary>
        /// <param name="e">Exception</param>
        /// <param name="message">Log message</param>
        public void Error(Exception e, string message)
        {
            if (!this.CanLog(LogLevel.ERROR))
            {
                return;
            }

            this.PushLog(LogLevel.ERROR, message,e);
        }

        /// <summary>
        /// Log Warning.
        /// </summary>
        /// <param name="message">Log message.</param
        public void Warning(string message)
        {
            if (!this.CanLog(LogLevel.WARNING))
            {
                return;
            }

            this.PushLog(LogLevel.WARNING, message,null);
        }

        /// <summary>
        /// Log Warning with exception.
        /// </summary>
        /// <param name="e">Exception</param>
        /// <param name="message">Log message</param>
        public void Warning(Exception e, string message)
        {
            if (!this.CanLog(LogLevel.WARNING))
            {
                return;
            }

            this.PushLog(LogLevel.WARNING, message,e);
        }

        /// <summary>
        /// Log Info.
        /// </summary>
        /// <param name="message">Log message.</param
        public void Info(string message, bool isAutoFlush = false)
        {
            if (!this.CanLog(LogLevel.INFO))
            {
                return;
            }

            this.PushLog(LogLevel.INFO, message,null, isAutoFlush);
        }

        /// <summary>
        /// Log Info with exception.
        /// </summary>
        /// <param name="e">Exception</param>
        /// <param name="message">Log message</param>
        public void Info(Exception e, string message, bool isAutoFlush = false)
        {
            if (!this.CanLog(LogLevel.INFO))
            {
                return;
            }

            this.PushLog(LogLevel.INFO, message,e, isAutoFlush);
        }

        /// <summary>
        /// Log Debug.
        /// </summary>
        /// <param name="message">Log message.</param
        public void Debug(string message)
        {
            if (!this.CanLog(LogLevel.DEBUG))
            {
                return;
            }

            this.PushLog(LogLevel.DEBUG, message,null);
        }

        /// <summary>
        /// Log Debug with exception.
        /// </summary>
        /// <param name="e">Exception</param>
        /// <param name="message">Log message</param>
        public void Debug(Exception e, string message)
        {
            if (!this.CanLog(LogLevel.DEBUG))
            {
                return;
            }

            this.PushLog(LogLevel.DEBUG, message,e);
        }

        public void AddHandler(IHandler handler)
        {
            lock (this.syncObj)
            {
                if (handler != null)
                {
                    this.handlerList.Add(handler);
                }
            }
        }

        /// <summary>
        /// Write log whith loglevel and message.
        /// </summary>
        /// <param name="level"></param>
        /// <param name="message"></param>
        public void WriteLog(LogLevel level, string message)
        {
            this.WriteLog(level, message, null);
        }

        public void WriteLog(LogLevel level, string message,Exception e)
        {
            this.PushLog(level, message,e);
        }

        public void Flush()
        {
            foreach (IHandler handler in this.handlerList)
            {
                handler.Flush();
            }
        }

        public Task FlushAsync()
        {
            foreach (IHandler handler in this.handlerList)
            {
                handler.Flush();
            }

            return Task.CompletedTask;
        }

        private void Init(string loggerName, LogLevel logLevel)
        {
            lock (this.syncObj)
            {
                this.loggerName = loggerName;
                this.logLevel = logLevel;
                this.handlerList = new HashSet<IHandler>();
            }
        }

        private void PushLog(LogLevel level, string message,Exception e, bool isAutoFlush = false)
        {
            if (message == null)
            {
               Logging.Instance.WriteDebugMessage("Message can not be null");
            }

            StackTrace stack = new StackTrace(true);
            StackFrame callerStackFrame = stack.GetFrame(2);
            MethodBase methodInfo = new StackTrace().GetFrame(2).GetMethod();
            string className = methodInfo.ReflectedType.Name;
            LogRecord record = new LogRecord(this.loggerName, level, stack, message, $"{className}.{methodInfo.Name}", callerStackFrame,e);

            foreach (IHandler handler in this.handlerList)
            {
                handler.Push(record);
                handler.DefaultLogFilename(record);
                if (isAutoFlush == true)
                {
                    handler.Flush();
                }
            }
        }

        private bool CanLog(LogLevel level)
        {
            if (level < this.logLevel || this.logLevel == LogLevel.NOTSET)
            {
                return false;
            }

            return true;
        }
    }
}
