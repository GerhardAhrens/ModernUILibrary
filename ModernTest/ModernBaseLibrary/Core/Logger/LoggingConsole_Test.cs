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
    using System.Threading;
    using System.Threading.Tasks;

    using global::ModernBaseLibrary.Core.Logger;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LoggingConsole_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingConsole_Test"/> class.
        /// </summary>
        public LoggingConsole_Test()
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
            var loggerName = "TestAddHandler";
            var logger = Logging.Instance.GetLogger(loggerName);
            var handler = new SimpleOutHandler();
            logger.AddHandler(handler);
            logger.SetLevel(LogLevel.INFO);
            Assert.IsTrue(logger.CountHandler == 1);
        }

        [TestMethod]
        public void LogLevelDebug_Test()
        {
            var logger = CreateTestLogger("TestLogLevelDebug");
            var handler = new SimpleOutHandler();
            logger.AddHandler(handler);
            logger.SetLevel(LogLevel.DEBUG);
            PushLogMsg(logger);
            Assert.AreEqual(handler.GetRecordList().Count, 5);
        }

        [TestMethod]
        public void LogLevelWarning_Test()
        {
            var logger = CreateTestLogger("TestLogLevelWarning");
            var handler = new SimpleOutHandler();
            logger.AddHandler(handler);
            logger.SetLevel(LogLevel.WARNING);
            PushLogMsg(logger);
            Assert.AreEqual(handler.GetRecordList().Count, 3);
        }

        [TestMethod]
        public void LogLevelError_Test()
        {
            var logger = CreateTestLogger("TestLogLevelError");
            var handler = new SimpleOutHandler();
            logger.AddHandler(handler);
            logger.SetLevel(LogLevel.ERROR);
            PushLogMsg(logger);
            Assert.AreEqual(handler.GetRecordList().Count, 2);
            logger.SetLevel(LogLevel.DEBUG);
            PushLogMsg(logger);
            Assert.AreEqual(handler.GetRecordList().Count, 7);
        }

        [TestMethod]
        public void LogLevelCritical_Test()
        {
            var logger = CreateTestLogger("TestLogLevelCritical");
            var handler = new SimpleOutHandler();
            logger.AddHandler(handler);
            logger.SetLevel(LogLevel.CRITICAL);
            PushLogMsg(logger);
            Assert.AreEqual(handler.GetRecordList().Count, 1);
        }

        [TestMethod]
        public void LogLevelAllAndFlush_Test()
        {
            var logger = CreateTestLogger("TestLogLevelAll");
            var handler = new SimpleOutHandler();
            logger.AddHandler(handler);
            logger.SetLevel(LogLevel.DEBUG);
            PushLogMsg(logger);
            Assert.AreEqual(handler.GetRecordList().Count, 5);
            logger.Flush();
            Assert.AreEqual(handler.GetRecordList().Count, 0);
        }

        [TestMethod]
        public void TestRecordMethodName()
        {
            var logger = CreateTestLogger("TestRecordMethodName");
            var handler = new SimpleOutHandler();
            logger.AddHandler(handler);
            logger.SetLevel(LogLevel.INFO);
            logger.Info("Test Msg");
            Assert.AreEqual(handler.GetRecordList().Count, 1);
            Assert.AreEqual(handler.GetRecordList()[0].FunctionName, "TestRecordMethodName");
        }

        [TestMethod]
        public void TestRecordException()
        {
            var logger = CreateTestLogger("TestRecordException");
            var handler = new SimpleOutHandler();
            logger.AddHandler(handler);
            logger.SetLevel(LogLevel.INFO);
            try
            {
                throw new Exception("TestExce");
            }
            catch (Exception ex)
            {
                logger.Info(ex, "Test Msg");
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }

            Assert.AreEqual(handler.GetRecordList().Count, 1);
            Assert.AreEqual(handler.GetRecordList()[0].Exception.Message, "TestExce");
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

        private ILogger CreateTestLogger(string loggerName)
        {
            var logger = Logging.Instance.GetLogger(loggerName);
            return logger;
        }

        private void PushLogMsg(ILogger logger)
        {
            logger.Debug("Debug Msg");
            logger.Info("Info Msg");
            logger.Warning("Warning Msg");
            logger.Error("Error Msg");
            logger.Critical("Critical MSg");
        }
    }

    public class SimpleOutHandler : AbstractOutHandler
    {
        private List<LogRecord> recordList;

        public SimpleOutHandler()
        {
            this.recordList = new List<LogRecord>();
        }

        public override void Push(LogRecord record)
        {
            System.Diagnostics.Trace.WriteLine(record.FunctionName);
            recordList.Add(record);
        }

        public override void Flush()
        {
            foreach (LogRecord record in this.recordList)
            {
                var time = record.EntryDateTime.ToString("HH:mm:ss.fff");
                var line = $"{time}-{record.Level.ToString().PadRight(10)}-{record.LineNumber.ToString().PadLeft(5)}-{record.Message}";
                System.Diagnostics.Trace.WriteLine(line);
            }

            this.recordList.Clear();
        }

        public List<LogRecord> GetRecordList()
        {
            return this.recordList;
        }
    }
}
