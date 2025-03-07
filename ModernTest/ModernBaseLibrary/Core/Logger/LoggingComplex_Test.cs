/*
 * <copyright file="Logging_Test.cs" company="Lifeprojects.de">
 *     Class: Logging_Test
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>04.03.2025 18:15:43</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Beschreibung zur Klasse
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using global::ModernBaseLibrary.Core.Logger;

    using Microsoft.VisualStudio.TestPlatform.TestHost;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LoggingComplex_Test : BaseTest
    {
        private string TestDirPath => TestContext.TestRunDirectory;
        private string TempDirPath => Path.Combine(TestDirPath, "Temp");
        private string pathFileName = string.Empty;

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Core\\Logger\\DemoData\\");
            if (Directory.Exists(pathFileName) == false)
            {
                Directory.CreateDirectory(pathFileName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingConsole_Test"/> class.
        /// </summary>
        public LoggingComplex_Test()
        {
        }

        public static ILogger logger { get { return Logging.Instance.GetLogger("TestFileOutHandler"); } }

        [TestMethod]
        public void LoggerFull_Test()
        {
            LogFileOutHandler handler = new LogFileOutHandler(pathFileName);
            logger.AddHandler(handler);
            logger.SetLevel(LogLevel.INFO);

            new TestLoggerClass().TestMethode();
            TestDemoLoggerClass.TestMethode();

            int maxStep = 100;
            for (int i = 0; i < maxStep; i++)
            {
                System.Diagnostics.Trace.WriteLine($"Step: {i}");
                logger.Info($"TestMsg-Info-{i}");
                logger.Warning($"Test Meldung für Warnung-{i}");

                try
                {
                    List<string> source = null;
                    int count = source.Count;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, $"Test Meldung für Error-{i}");
                }

                try
                {
                    string text = "Gerhard";
                    string part = text.Substring(20, 10);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, $"Test Meldung für Error-{i}");
                }
            }

            Assert.AreEqual(handler.GetRecordList().Count, (maxStep*4)+2);
            logger.Flush();
            Assert.AreEqual(handler.GetRecordList().Count, 0);
            string logFile = handler.LogFileName;
            Assert.IsTrue(File.Exists(logFile));
        }

        [TestMethod]
        public async Task LoggerFull_Async_Test()
        {
            LogFileOutHandler handler = new LogFileOutHandler(pathFileName);
            logger.AddHandler(handler);
            logger.SetLevel(LogLevel.INFO);

            new TestLoggerClass().TestMethode();
            TestDemoLoggerClass.TestMethode();

            int maxStep = 100;
            for (int i = 0; i < maxStep; i++)
            {
                System.Diagnostics.Trace.WriteLine($"Step: {i}");
                logger.Info($"TestMsg-Info-{i}");
                logger.Warning($"Test Meldung für Warnung-{i}");

                try
                {
                    List<string> source = null;
                    int count = source.Count;
                }
                catch (Exception ex)
                {
                    logger.Error(ex, $"Test Meldung für Error-{i}");
                }

                try
                {
                    string text = "Gerhard";
                    string part = text.Substring(20, 10);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, $"Test Meldung für Error-{i}");
                }
            }

            Assert.AreEqual(handler.GetRecordList().Count, (maxStep * 4) + 2);
            await logger.FlushAsync();
            Assert.AreEqual(handler.GetRecordList().Count, 0);
            string logFile = handler.LogFileName;
            Assert.IsTrue(File.Exists(logFile));
        }


        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void ExceptionTest()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        public class TestLoggerClass
        {
            public void TestMethode()
            {
                LoggingComplex_Test.logger.Info("Hallo-Hallo");
            }
        }

        public class TestDemoLoggerClass
        {
            public static void TestMethode()
            {
                LoggingComplex_Test.logger.Info("Hallo-Hallo, Static Methode");
            }
        }
    }

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
