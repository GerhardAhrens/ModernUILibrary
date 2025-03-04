/*
 * <copyright file="LoggingCompareObject.cs" company="Lifeprojects.de">
 *     Class: LoggingCompareObject
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>04.03.2025 18:10:34</date>
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
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Core.Logger;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LoggingCompareObject
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingCompareObject"/> class.
        /// </summary>
        public LoggingCompareObject()
        {
        }

        [TestMethod]
        public void GetLogger_Test()
        {
            ILogger logger1 = Logging.Instance.GetLogger("TestGetLogger");
            ILogger logger2 = Logging.Instance.GetLogger("TestGetLogger");
            Assert.AreSame(logger1, logger2);
        }

        [TestMethod]
        public void TestGetDifLogger()
        {
            ILogger logger1 = Logging.Instance.GetLogger("TestGetDifLogger");
            ILogger logger2 = Logging.Instance.GetLogger("TestGetDifLogger2");
            Assert.AreNotSame(logger1, logger2);
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
}
