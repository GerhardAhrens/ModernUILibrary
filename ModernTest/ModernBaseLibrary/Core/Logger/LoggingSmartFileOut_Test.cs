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

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LoggingSmartFileOut_Test : BaseTest
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
        public LoggingSmartFileOut_Test()
        {
        }

        [TestMethod]
        public void LoggerName_Test()
        {
            var loggerName = "TestLogger1";
            var logger = Logging.Instance.GetLogger(loggerName);
            Assert.AreEqual(loggerName, logger.Name);
        }

        [TestMethod]
        public void AddHandler_Test()
        {
            var loggerName = "TestConsoleOutHandler";
            var logger = Logging.Instance.GetLogger(loggerName);
            var handler = new SmartFileOutHandler(pathFileName);
            logger.AddHandler(handler);
            Assert.IsTrue(logger.CountHandler == 1);
            logger.SetLevel(LogLevel.INFO);
            logger.Info($"TestMsg-Info");
            logger.Flush();

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
    }

    public class SmartFileOutHandler : AbstractOutHandler
    {
        private string logPath = string.Empty;
        private HashSet<Record> logContent = null;

        public SmartFileOutHandler(string logPath = "")
        {
            if (string.IsNullOrEmpty(logPath) == false)
            {
                this.logPath = logPath;
            }
            else
            {
                this.logPath = this.DefaultLogPath();
            }

            this.logContent = new HashSet<Record>();

            if (this.MaxFiles > 0)
            {
                this.ClearLogfiles();
            }
        }

        public override void Push(Record record)
        {
            if (logContent != null)
            {
                logContent.Add(record);
            }
        }

        public override async void Flush()
        {
            await Task.Factory.StartNew(() =>
            {
                if (logContent != null)
                {
                    foreach (Record record in logContent)
                    {
                        this.WriteFileHeader(record);
                        this.WriteToFile(record);
                    }

                    this.logContent.Clear();
                }
            });
        }

        private void WriteFileHeader(Record record)
        {
            string fullFilename = Path.Combine(this.logPath, this.DefaultLogFilename(record));

            if (File.Exists(fullFilename) == false)
            {
                using (var streamWriter = new StreamWriter(fullFilename, true, Encoding.UTF8))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("#####-Start-#####--------------------------------------------------------");
                    sb.AppendLine($"PCName / User   : {Environment.MachineName} / {Environment.UserName}");
                    sb.AppendLine($"Log Filename: {fullFilename}");
                    sb.AppendLine("-------------------------------------------------------------------------");
                    streamWriter.WriteLine(sb.ToString());
                }
            }
        }

        private void WriteToFile(Record record)
        {
            string fullFilename = Path.Combine(this.logPath, this.DefaultLogFilename(record));

            if (File.Exists(fullFilename) == true)
            {
                using (var streamWriter = new StreamWriter(fullFilename, true, Encoding.UTF8))
                {
                    var time = record.EntryDateTime.ToString("HH:mm:ss.fff");
                    if (record.Level == LogLevel.ERROR)
                    {
                        var line = $"{time}-{record.Level.ToString().PadRight(10)}-{record.LineNumber.ToString().PadLeft(5)}-{record.Message}\t{record.Exception.Message.Replace($"\r", string.Empty).Replace($"\n", string.Empty)}";
                        streamWriter.WriteLine(line);
                    }
                    else
                    {
                        var line = $"{time}-{record.Level.ToString().PadRight(10)}-{record.LineNumber.ToString().PadLeft(5)}-{record.Message}";
                        streamWriter.WriteLine(line);
                    }
                }
            }
        }
    }
}
