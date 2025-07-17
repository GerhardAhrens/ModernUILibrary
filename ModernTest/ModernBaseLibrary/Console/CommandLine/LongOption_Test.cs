/*
 * <copyright file="LongOption_Test.cs" company="Lifeprojects.de">
 *     Class: LongOption_Test
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>21.11.2022 20:51:17</date>
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

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernConsole.CommandLine;

    [TestClass]
    public class LongOption_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LongOption_Test"/> class.
        /// </summary>
        public LongOption_Test()
        {
        }

        [TestMethod]
        [TestDescription("--admin", true)]
        [TestDescription("-admin", false)]
        public void Long_Option_Must_Start_With_Double_Hyphens()
        {
            Dictionary<string, bool> testsAndExpectedStatus = new Dictionary<string, bool>()
            {
                { "--admin", true },
                { "-admin", false }
            };

            testsAndExpectedStatus.ToList().ForEach(test =>
            {
                bool isKey = CmdLineKeyDetection.GetLongKeyDetector().IsKey(test.Key);
                Console.WriteLine($"{test.Key} detected:{isKey} actual:{test.Value}");
                Assert.AreEqual(isKey, test.Value);
            });
        }


        [TestMethod]
        [TestDescription("--admin", true)]
        [TestDescription("--Admin", false)]
        public void Long_Option_Must_Be_Lowercase()
        {

            Dictionary<string, bool> testsAndExpectedStatus = new Dictionary<string, bool>()
            {
                { "--admin", true },
                { "--Admin", false }
            };
            testsAndExpectedStatus.ToList().ForEach(test =>
            {
                bool isKey = CmdLineKeyDetection.GetLongKeyDetector().IsKey(test.Key);
                Console.WriteLine($"{test.Key} detected:{isKey} actual:{test.Value}");
                Assert.AreEqual(isKey, test.Value);
            });
        }

        [TestMethod]
        [TestDescription("--admin", true)]
        [TestDescription("--3admin", false)]
        public void Long_Option_Name_Must_Start_With_A_Letter()
        {
            Dictionary<string, bool> testsAndExpectedStatus = new Dictionary<string, bool>()
            {
                { "--admin", true },
                { "--3admin", false }
            };
            testsAndExpectedStatus.ToList().ForEach(test =>
            {
                bool isKey = CmdLineKeyDetection.GetLongKeyDetector().IsKey(test.Key);
                Console.WriteLine($"{test.Key} detected:{isKey} actual:{test.Value}");
                Assert.AreEqual(isKey, test.Value);
            });
        }

        [TestMethod]
        [TestDescription("program --name=gerhard", true)]
        public void Long_Option_Can_Be_Joined_To_Value_By_Equals_Sign()
        {
            string[] args = CommandManager.CommandLineToArgs("program --name=gerhard");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<CmdLineEnumModel>();
            Assert.AreEqual(model.UserName, "gerhard");
        }

        [TestMethod]
        [TestDescription("program --name gerhard --admin", true)]
        public void Long_Option_Can_Have_Space_Separated_Values_1()
        {
            string[] args = CommandManager.CommandLineToArgs("program --name gerhard --admin");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<CmdLineEnumModel>();
            Assert.IsTrue(model.IsAdmin);
            Assert.AreEqual(model.UserName, "gerhard");
        }

        [TestMethod]
        [TestDescription("program --name=gerhard --admin", true)]
        public void Long_Option_Can_Have_Space_Separated_Values_2()
        {
            string[] args = CommandManager.CommandLineToArgs("program --name=gerhard --admin");
            CommandParser parser = new CommandParser(args);
            var model = parser.Parse<CmdLineEnumModel>();
            Assert.IsTrue(model.IsAdmin);
            Assert.AreEqual(model.UserName, "gerhard");
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
            [Flag("name", "n")]
            public string UserName { get; set; }

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
