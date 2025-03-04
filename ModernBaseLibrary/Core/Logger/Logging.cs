//-----------------------------------------------------------------------
// <copyright file="Logging.cs" company="Lifeprojects.de">
//     Class: Logging
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.10.2022</date>
//
// <summary>
// A Logging class. You can get logger by this class.
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Logger
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// A Logging class. You can get logger by this class.
    /// </summary>
    //[DebuggerStepThrough()]
    public sealed class Logging
    {
        public bool DebugMode { get; set; }

        private static volatile Logging instance;
        private static object syncRoot = new object();
        private Dictionary<string, ILogger> loggerDictionary;

        private Logging()
        {
            this.loggerDictionary = new Dictionary<string, ILogger>();
        }

        /// <summary>
        /// Singleton get instance property. 
        /// </summary>
        public static Logging Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new Logging();
                        }
                    }
                }
                return instance;
            }
        }

        /// <summary>
        /// Get loogger by logger name.
        /// </summary>
        /// <param name="loggerName">Which logger name you want to get.</param>
        /// <returns></returns>
        public ILogger GetLogger(string loggerName)
        {
            lock (syncRoot)
            {
                if (this.loggerDictionary.ContainsKey(loggerName) == false)
                {
                    this.loggerDictionary.Add(loggerName, new Logger(loggerName));
                }

                return this.loggerDictionary[loggerName];
            }
        }

        /// <summary>
        /// Add logger manually. But you can not add logger if thie logger name already exists. 
        /// </summary>
        /// <param name="logger">Your logger class.</param>
        /// <Exception crf="LoggerNameDuplicateException">If logger name already exists.</Exception>>
        public void AddLogger(ILogger logger)
        {
            lock (syncRoot)
            {
                if (this.loggerDictionary.ContainsKey(logger.Name))
                {
                   this.WriteDebugMessage($"Logger Name '{logger.Name}' Duplicate.");
                   return;
                }

                this.loggerDictionary.Add(logger.Name, logger);
            }
        }


        public void WriteDebugMessage(string message)
        {
            if (this.DebugMode)
            {
                System.Diagnostics.Debug.Assert(!string.IsNullOrEmpty(message));
                System.Diagnostics.Trace.WriteLine(this.FormateDebugMessage(message));
            }
        }

        private string FormateDebugMessage(string message)
        {
            string formatedMessage = $"[Logger] {DateTime.Now.ToString()} -- {message}";
            return formatedMessage;
        }

    }
}