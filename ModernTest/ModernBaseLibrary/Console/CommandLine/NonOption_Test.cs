/*
 * <copyright file="NonOption_Test.cs" company="Lifeprojects.de">
 *     Class: NonOption_Test
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>21.11.2022 20:19:32</date>
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

namespace ModernTest.ModernBaseLibrary.Console.CommandLine
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.CommandLine;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NonOption_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonOption_Test"/> class.
        /// </summary>
        public NonOption_Test()
        {
        }

        [TestMethod]
        [TestDescription("program hello world")]
        public void NonOptionsAreBoundToOptions()
        {
            string[] args = CommandManager.CommandLineToArgs("program hello world");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<CmdLineModelNoOption>();
            CollectionAssert.Contains(model.Options, "hello");
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

        private class CmdLineModelNoOption : ICmdLineModel
        {
            public string[] Extras { get; set; }
            public string[] Options { get; set; }
        }
    }
}
