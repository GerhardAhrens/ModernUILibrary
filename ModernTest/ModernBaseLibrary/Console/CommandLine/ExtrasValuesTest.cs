/*
 * <copyright file="ExtrasValuesTest.cs" company="Lifeprojects.de">
 *     Class: ExtrasValuesTest
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>21.11.2022 20:41:32</date>
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

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernConsole.CommandLine;

    [TestClass]
    public class ExtrasValuesTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtrasValuesTest"/> class.
        /// </summary>
        public ExtrasValuesTest()
        {
        }

        [TestMethod]
        [TestDescription("program -- Hello World")]
        public void Terminators_Are_Bound_To_Extras()
        {
            string[] args = CommandManager.CommandLineToArgs("program -- Hello World");
            CommandParser parser = new CommandParser(args);
            CmdLineModelNoOption model = parser.Parse<CmdLineModelNoOption>();
            CollectionAssert.Contains(model.Extras, "Hello");
            CollectionAssert.Contains(model.Extras, "World");
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
