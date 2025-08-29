//-----------------------------------------------------------------------
// <copyright file="LogFileOutHandler.cs" company="company">
//     Class: LogFileOutHandler
//     Copyright © company 2025
// </copyright>
//
// <author>Gerhard Ahrens - company</author>
// <email>gerhard.ahrens@company.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Klasse zur implementierung eines Logging Handler zur Dateiausgabe
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ModernBaseLibrary.Core.Logger;

    public class LogFileOutHandler : AbstractOutHandler
    {
        private string logPath = string.Empty;
        private HashSet<LogRecord> logContent = null;

        public LogFileOutHandler(string logPath = "")
        {
            if (string.IsNullOrEmpty(logPath) == false)
            {
                this.logPath = logPath;
            }
            else
            {
                this.logPath = this.DefaultLogPath();
            }

            if (Directory.Exists(this.logPath) == false)
            {
                Directory.CreateDirectory(this.logPath);
                string archivePath = Path.Combine(logPath, "Archive");
                Directory.CreateDirectory(archivePath);
            }
            else
            {
                string archivePath = Path.Combine(logPath, "Archive");
                if (Directory.Exists(archivePath) == false)
                {
                    Directory.CreateDirectory(archivePath);
                }
            }

            this.logContent = new HashSet<LogRecord>();

            if (this.MaxFiles > 0)
            {
                this.ClearLogfiles(this.logPath);
            }
        }

        public override string LogFilePattern { get; set; } = "*.log";

        public override void Push(LogRecord record)
        {
            if (logContent != null)
            {
                logContent.Add(record);
            }
        }

        public override void Flush()
        {
            if (this.logContent != null)
            {
                foreach (LogRecord record in this.logContent)
                {
                    this.WriteFileHeader(record);
                    this.WriteToFile(record);
                }

                this.logContent.Clear();
            }
        }

        public override async Task FlushAsync()
        {
            await Task.Factory.StartNew(() =>
            {
                if (this.logContent != null)
                {
                    foreach (LogRecord record in this.logContent)
                    {
                        this.WriteFileHeader(record);
                        this.WriteToFile(record);
                    }

                    this.logContent.Clear();
                }
            });
        }

        public HashSet<LogRecord> GetRecordList()
        {
            return this.logContent;
        }

        private void WriteFileHeader(LogRecord record)
        {
            string fullFilename = Path.Combine(this.logPath, this.DefaultLogFilename(record));
            this.LogFileName = fullFilename;
            if (File.Exists(fullFilename) == false)
            {
                using (var streamWriter = new StreamWriter(fullFilename, true, Encoding.UTF8))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("#####-Start-#####--------------------------------------------------------");
                    sb.AppendLine($"PCName / User   : {Environment.MachineName} - {Environment.UserDomainName}/{Environment.UserName}");
                    sb.AppendLine($"Log Filename: {fullFilename}");
                    sb.AppendLine("-------------------------------------------------------------------------");
                    streamWriter.WriteLine(sb.ToString());
                }
            }
        }

        private void WriteToFile(LogRecord record)
        {
            string fullFilename = Path.Combine(this.logPath, this.DefaultLogFilename(record));
            this.LogFileName = fullFilename;

            if (File.Exists(fullFilename) == true)
            {
                using (var streamWriter = new StreamWriter(fullFilename, true, Encoding.UTF8))
                {
                    var time = record.EntryDateTime.ToString("HH:mm:ss.fff");
                    if (record.Level == LogLevel.ERROR)
                    {
                        if (record.Exception != null)
                        {
                            var line = $"{time}-{record.Level.ToString().PadRight(10)}-{record.LineNumber.ToString().PadLeft(5)}-{record.Message}\t{record.Exception.Message.Replace($"\r", string.Empty).Replace($"\n", string.Empty)}";
                            streamWriter.WriteLine(line);
                        }
                        else
                        {
                            var line = $"{time}-{record.Level.ToString().PadRight(10)}-{record.LineNumber.ToString().PadLeft(5)}|{record.FunctionName}|{record.Message}";
                            streamWriter.WriteLine(line);
                        }
                    }
                    else
                    {
                        var line = $"{time}-{record.Level.ToString().PadRight(10)}-{record.LineNumber.ToString().PadLeft(5)}|{record.FunctionName}|{record.Message}";
                        streamWriter.WriteLine(line);
                    }
                }
            }
        }
    }
}
