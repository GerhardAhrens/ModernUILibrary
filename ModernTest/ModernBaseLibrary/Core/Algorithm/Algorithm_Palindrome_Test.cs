/*
 * <copyright file="Algorithm_P.cs" company="Lifeprojects.de">
 *     Class: Algorithm_P
 *     Copyright © Lifeprojects.de 2024
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>10.12.2024 19:57:03</date>
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

namespace ModernTest.ModernBaseLibrary.Core.Algorithm
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using global::ModernBaseLibrary.Core.Algorithm;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class Algorithm_Palindrome_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Algorithm_Palindrome_Test"/> class.
        /// </summary>
        public Algorithm_Palindrome_Test()
        {
        }

        [TestMethod]
        public void PalindromeInt_OK()
        {
            int eingabe = 12321;
            int result = Palindrome<int>.Get(eingabe);

            Assert.AreEqual(eingabe, result);
        }

        [TestMethod]
        public void PalindromeInt_NotOK()
        {
            int eingabe = 123219;
            int result = Palindrome<int>.Get(eingabe);

            Assert.AreNotEqual(eingabe, result);
        }

        [TestMethod]
        public void PalindromeString_OK()
        {
            string eingabe = "anna";
            string result = Palindrome<string>.Get(eingabe);

            Assert.AreEqual(eingabe, result);
        }

        [TestMethod]
        public void PalindromeString_NotOK()
        {
            string eingabe = "charlie";
            string result = Palindrome<string>.Get(eingabe);

            Assert.AreNotEqual(eingabe, result);
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
