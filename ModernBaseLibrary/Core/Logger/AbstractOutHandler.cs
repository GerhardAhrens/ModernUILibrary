//-----------------------------------------------------------------------
// <copyright file="AbstractOutHandler.cs" company="Lifeprojects.de">
//     Class: AbstractOutHandler
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>26.10.2022</date>
//
// <summary>
// Abstract Handler implement IHandler.
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Logger
{
    using System.IO;
    using System;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Linq;
    using System.Diagnostics;

    /// <summary>
    /// Abstract Handler implement IHandler.
    /// It has one formatter to format record imformation.
    /// </summary>
    [DebuggerStepThrough()]
    public abstract class AbstractOutHandler : IHandler
    {
        /// <summary>
        /// Formatter member.
        /// </summary>
        protected IFormatter formatter;

        /// <summary>
        /// AbstractOutHandler constractor. It default ues SimpleFormatter.
        /// </summary>
        protected AbstractOutHandler()
        {
            this.formatter = new SimpleFormatter();
        }

        public virtual int MaxFiles { get; set; } = 5;

        public virtual string LogFilePattern { get; set; } = "*.log";

        public Record Record { get; private set; }

        public virtual string LogFileName { get; set; } = string.Empty;

        /// <summary>
        /// Push log record to handler.
        /// </summary>
        /// <param name="record"></param>
        public abstract void Push(Record record);

        /// <summary>
        /// The method let handler to flush.
        /// </summary>
        public virtual Task FlushAsync()
        {
            return Task.CompletedTask;
        }

        public virtual void Flush()
        {
        }

        /// <summary>
        /// Formatter srtter.
        /// </summary>
        /// <param name="formatter"></param>
        public void SetFormatter(IFormatter formatter)
        {
            this.formatter = formatter;
        }

        public virtual string DefaultLogPath()
        {
            string settingsPath = string.Empty;

            string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            settingsPath = $"{rootPath}\\{this.ApplicationName()}\\Log";

            if (string.IsNullOrEmpty(settingsPath) == false)
            {
                try
                {
                    if (Directory.Exists(settingsPath) == false)
                    {
                        Directory.CreateDirectory(settingsPath);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return settingsPath;
        }

        public virtual string DefaultLogFilename(Record record)
        {
            string date = DateTime.Now.ToString("yyyyMMdd");

            this.LogFileName = string.Format(string.IsNullOrEmpty(record.LoggerName) ? $"{date}.log" : $"{date}_{record.LoggerName}.log");

            return this.LogFileName;
        }

        public virtual void ClearLogfiles()
        {
            DirectoryInfo dir = new DirectoryInfo(this.DefaultLogPath());
            IEnumerable<string> fileList = dir.EnumerateFiles(this.LogFilePattern, SearchOption.AllDirectories)
                .OrderByDescending(p => p.Name)
                .Select(p => p.FullName)
                .Skip(this.MaxFiles).ToList();

            foreach (string file in fileList)
            {
                File.Delete(file);
            }
        }

        public string ApplicationName()
        {
            string result = string.Empty;

            Assembly assm = Assembly.GetEntryAssembly();
            result = assm.GetName().Name;
            return result;
        }
    }
}
