/*
 * <copyright file="IDictionaryExtension_Test.cs" company="Lifeprojects.de">
 *     Class: IDictionaryExtension_Test
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>25.09.2022 09:15:32</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * UnitTest für IDictionaryExtension
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

namespace ModernTest.ModernBaseLibrary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class IDictionaryExtension_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IDictionaryExtension_Test"/> class.
        /// </summary>
        public IDictionaryExtension_Test()
        {
        }

        [TestMethod]
        public void IsNullOrEmpty_SetNull()
        {
            IDictionary dict = null;
            bool result = dict.IsNullOrEmpty();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNullOrEmpty_SetInstanz()
        {
            IDictionary dict = new Dictionary<int,int>();
            bool result = dict.IsNullOrEmpty();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsNullOrEmpty_SetGreater0()
        {
            IDictionary dict = new Dictionary<int, int>();
            dict.Add(0, 0);
            bool result = dict.IsNullOrEmpty();
            Assert.IsFalse(result);
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
