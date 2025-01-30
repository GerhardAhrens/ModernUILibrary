/*
 * <copyright file="MaxLenStringFormat_Test.cs" company="Lifeprojects.de">
 *     Class: MaxLenStringFormat_Test
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
    public class MaxLenStringFormat_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MaxLenStringFormat_Test"/> class.
        /// </summary>
        public MaxLenStringFormat_Test()
        {
        }

        [TestMethod]
        public void MaxLenStringFormat_Max()
        {
            string text = string.Format(new MaxLenStringFormat(), "{0:max(7)}", "Das ist ein langer Text");
            Assert.IsTrue(text == "Das ist");
        }

        [TestMethod]
        public void MaxLenStringFormat_Left()
        {
            string text = string.Format(new MaxLenStringFormat(), "{0:sl(7)}", "Das ist ein langer Text");
            Assert.IsTrue(text == "Das ...");
        }

        [TestMethod]
        public void MaxLenStringFormat_Right()
        {
            string text = string.Format(new MaxLenStringFormat(), "{0:sr(7)}", "Das ist ein langer Text");
            Assert.IsTrue(text == "...Text");
        }

        [TestMethod]
        public void MaxLenStringFormat_Middle()
        {
            string text = string.Format(new MaxLenStringFormat(), "{0:sm(13)}", "Das ist ein langer Text");
            Assert.IsTrue(text == "Das ist ein l");
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
