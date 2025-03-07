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
    //[DebuggerStepThrough()]
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

        public virtual string LogFilePattern { get; set; } = string.Empty;

        public LogRecord Record { get; private set; }

        public virtual string LogFileName { get; set; } = string.Empty;

        public virtual string ArchivePath { get; set; } = string.Empty;

        public bool IsArchiveData { get; set; } = false;

        /// <summary>
        /// Push log record to handler.
        /// </summary>
        /// <param name="record"></param>
        public abstract void Push(LogRecord record);

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
            string logPath = string.Empty;

            string rootPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            logPath = $"{rootPath}\\{this.ApplicationName()}\\Log";

            if (string.IsNullOrEmpty(logPath) == false)
            {
                try
                {
                    if (Directory.Exists(logPath) == false)
                    {
                        Directory.CreateDirectory(logPath);
                        if (this.IsArchiveData == true)
                        {
                            this.ArchivePath = Path.Combine(logPath, "Archive");
                            Directory.CreateDirectory(this.ArchivePath);
                        }
                    }
                    else
                    {
                        if (this.IsArchiveData == true)
                        {
                            this.ArchivePath = Path.Combine(logPath, "Archive");
                            if (Directory.Exists(this.ArchivePath) == false)
                            {
                                Directory.CreateDirectory(this.ArchivePath);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return logPath;
        }

        public virtual string DefaultLogFilename(LogRecord record)
        {
            if (string.IsNullOrEmpty(this.LogFilePattern) == true)
            {
                string date = DateTime.Now.ToString("yyyyMMdd");
                this.LogFileName = string.Format(string.IsNullOrEmpty(record.LoggerName) ? $"{date}.log" : $"{date}_{record.LoggerName}.log");
            }
            else
            {
                if (string.IsNullOrEmpty(this.LogFilePattern) == false || this.LogFilePattern.StartsWith("*.") == true)
                {
                    string date = DateTime.Now.ToString("yyyyMMdd");
                    this.LogFileName = string.Format(string.IsNullOrEmpty(record.LoggerName) ? $"{date}.log" : $"{date}_{record.LoggerName}.log");
                }
                else
                {
                    this.LogFileName = this.LogFilePattern;
                }
            }

            return this.LogFileName;
        }

        public virtual void ClearLogfiles(string logPath = "")
        {
            DirectoryInfo dir = null;

            if (string.IsNullOrEmpty(logPath) == true)
            {
                dir = new DirectoryInfo(this.DefaultLogPath());
            }
            else
            {
                dir = new DirectoryInfo(logPath);
            }

            IEnumerable<string> fileList = dir.EnumerateFiles(this.LogFilePattern, SearchOption.AllDirectories)
                    .OrderByDescending(p => p.Name)
                    .Select(p => p.FullName)
                    .Skip(this.MaxFiles).ToList();

            foreach (string file in fileList)
            {
                if (this.IsArchiveData == false)
                {
                    File.Delete(file);
                }
                else 
                {
                    this.MoveToArchive(Path.GetDirectoryName(file), this.ArchivePath, Path.GetFileName(file));
                }
            }
        }

        private string ApplicationName()
        {
            string result = string.Empty;

            Assembly assm = Assembly.GetEntryAssembly();
            result = assm.GetName().Name;
            return result;
        }

        private void MoveToArchive(string suorcePath, string targetPath, string fileName)
        {
        }
    }
}
