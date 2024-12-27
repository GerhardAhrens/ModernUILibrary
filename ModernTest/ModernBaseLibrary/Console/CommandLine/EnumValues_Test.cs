/*
 * <copyright file="EnumValues_Test.cs" company="Lifeprojects.de">
 *     Class: EnumValues_Test
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>21.11.2022 20:28:42</date>
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using global::ModernBaseLibrary.CommandLine;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EnumValues_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnumValues_Test"/> class.
        /// </summary>
        public EnumValues_Test()
        {
        }

        [TestMethod]
        [TestDescription("program --role=SuperAdmin --role=Admin")]
        public void Values_Long_Option_Equals_Separated_Values_Can_Have_Multiple_Values()
        {
            string[] args = CommandManager.CommandLineToArgs("program --role=SuperAdmin --role=Admin");
            CommandParser parser = new CommandParser(args);
            CmdLineEnumModel model = parser.Parse<CmdLineEnumModel>();
            Assert.IsTrue(model.IsAdmin);
            Assert.IsTrue(model.IsSuperAdmin);
        }

        [TestMethod]
        [TestDescription("program -r=SuperAdmin -r=Admin")]
        public void Values_Short_Option_Equals_Separated_Values_Can_Have_Multiple_Values()
        {
            string[] args = CommandManager.CommandLineToArgs("program -r=SuperAdmin -r=Admin");
            CommandParser parser = new CommandParser(args);
            CmdLineEnumModel model = parser.Parse<CmdLineEnumModel>();
            Assert.IsTrue(model.IsAdmin);
            Assert.IsTrue(model.IsSuperAdmin);
        }

        [TestMethod]
        [TestDescription("program --role SuperAdmin --role Admin")]
        public void Values_Long_Option_Space_Separated_Values_Can_Have_Multiple_Values()
        {
            string[] args = CommandManager.CommandLineToArgs("program --role SuperAdmin --role Admin");
            CommandParser parser = new CommandParser(args);
            CmdLineEnumModel model = parser.Parse<CmdLineEnumModel>();
            Assert.IsTrue(model.IsAdmin);
            Assert.IsTrue(model.IsSuperAdmin);
        }

        [TestMethod]
        [TestDescription("program -r SuperAdmin -r Admin")]
        public void Values_Short_Option_Space_Separated_Values_Can_Have_Multiple_Values()
        {
            string[] args = CommandManager.CommandLineToArgs("program -r SuperAdmin -r Admin");
            CommandParser parser = new CommandParser(args);
            CmdLineEnumModel model = parser.Parse<CmdLineEnumModel>();
            Assert.IsTrue(model.IsAdmin);
            Assert.IsTrue(model.IsSuperAdmin);
        }

        [TestMethod]
        [TestDescription("program --role SuperAdmin Admin")]
        public void Values_Long_Option_Space_Separated_Values_Can_Have_Multiple_Values_2()
        {
            string[] args = CommandManager.CommandLineToArgs("program --role SuperAdmin Admin");
            CommandParser parser = new CommandParser(args);
            CmdLineEnumModel model = parser.Parse<CmdLineEnumModel>();
            Assert.IsTrue(model.IsAdmin);
            Assert.IsTrue(model.IsSuperAdmin);
        }

        [TestMethod, Description("program -sa")]
        public void Short_Option_With_NO_Values_Can_Be_Aggregated()
        {
            string[] args = CommandManager.CommandLineToArgs("program -sa");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<CmdLineEnumModel>();
            Assert.IsTrue(model.IsAdmin);
            Assert.IsTrue(model.IsSuperAdmin);
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

        [Help("== This is a Test Model for Flag-Attribute ==")]
        private class CmdLineEnumModel
        {
            [Flag("role", "r")]
            public List<Role> Roles { get; set; } = Enum.GetValues(typeof(Role)).Cast<Role>().Select(v => v).ToList();

            [Flag("admin", "a")]
            public bool IsAdmin
            {
                get
                {
                    return Roles.Any(role => role == Role.Admin);
                }
                set
                {
                    if (!Roles.Any(role => role == Role.Admin)) Roles.Add(Role.Admin);
                }
            }

            [Flag("superadmin", "s")]
            public bool IsSuperAdmin
            {
                get
                {
                    return Roles.Any(role => role == Role.SuperAdmin);
                }
                set
                {
                    if (!Roles.Any(role => role == Role.SuperAdmin)) Roles.Add(Role.SuperAdmin);
                }
            }

            public enum Role
            {
                User,
                Admin,
                SuperAdmin
            }
        }
    }
}
