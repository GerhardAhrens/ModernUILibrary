/*
 * <copyright file="DateQuarterFormatTo_Test.cs" company="Lifeprojects.de">
 *     Class: DateQuarterFormatTo_Test
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>08.12.2022 20:52:55</date>
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

namespace ModernTest.ModernBaseLibrary.FormatTo
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DateQuarterFormatTo_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileSizeFormatTo_Test"/> class.
        /// </summary>
        public DateQuarterFormatTo_Test()
        {
        }

        [TestMethod]
        public void DateQuarterFormatTo_Short()
        {
            string quaterText = string.Format(new DateQuarterFormatTo(), "{0:q}", new DateTime(2022,12,1));
            Assert.IsTrue(quaterText == "4. Q");
        }

        [TestMethod]
        public void DateQuarterFormatTo_Long()
        {
            string quaterText = string.Format(new DateQuarterFormatTo(), "{0:Q}", new DateTime(2022, 12, 1));
            Assert.IsTrue(quaterText == "4. Quartal");
        }

        [TestMethod]
        public void DateQuarterFormatTo_LongYear()
        {
            string quaterText = string.Format(new DateQuarterFormatTo(), "{0:qy}", new DateTime(2022, 12, 1));
            Assert.IsTrue(quaterText == "4. Q 2022");
        }

        [TestMethod]
        public void DateQuarterFormatTo_RomanShort()
        {
            string quaterText = string.Format(new DateQuarterFormatTo(), "{0:r}", new DateTime(2022, 12, 1));
            Assert.IsTrue(quaterText == "IV. Q");
        }

        [TestMethod]
        public void DateQuarterFormatTo_RomanLong()
        {
            string quaterText = string.Format(new DateQuarterFormatTo(), "{0:R}", new DateTime(2022, 12, 1));
            Assert.IsTrue(quaterText == "IV. Quartal");
        }

        [TestMethod]
        public void DateQuarterFormatTo_RomanLongYear()
        {
            string quaterText = string.Format(new DateQuarterFormatTo(), "{0:RY}", new DateTime(2022, 12, 1));
            Assert.IsTrue(quaterText == "IV. Quartal 2022");
        }

        [TestMethod]
        public void DateQuarterFormatTo_UnknowFormat()
        {
            string quaterText = string.Format(new DateQuarterFormatTo(), "{0:xx}", new DateTime(2022, 12, 1));
            Assert.IsTrue(quaterText == "xx is unknown Format");
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
