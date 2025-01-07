/*
 * <copyright file="TestDataGenerator.cs" company="Lifeprojects.de">
 *     Class: TestDataGenerator
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>02.01.2025 20:25:45</date>
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

namespace ModernTest.ModernBaseLibrary.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.ExceptionServices;
    using System.Threading;
    using DemoDataGeneratorLib.Base;

    using global::ModernBaseLibrary.Cryptography;
    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class TestDataGenerator_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TestDataGenerator"/> class.
        /// </summary>
        public TestDataGenerator_Test()
        {
        }

        [TestMethod]
        public void GenerateTestData_String()
        {
            IEnumerable<UserTestGenerator> users = BuildDemoData<UserTestGenerator>.CreateForList<UserTestGenerator>(ConfigObject,100);
            Assert.AreEqual(users.Count(), 100);
        }

        private UserTestGenerator ConfigObject(UserTestGenerator user)
        {
            user.UserName = BuildDemoData.LastName();

            return user;
        }

        [TestMethod]
        public void GenerateTestData_SetTimeStamp()
        {
            IEnumerable<UserTestGenerator> users = BuildDemoData<UserTestGenerator>.CreateForList<UserTestGenerator>(ConfigObjectSetTimeStamp, 100);
            Console.SetOut(new DebugTextWriter());
            users.ForEach<UserTestGenerator>(user =>
            {
                Console.WriteLine($"{user.CreateOn};{user.CreateBy}|{user.ModifiedOn};{user.ModifiedBy}");
            });

            Assert.AreEqual(users.Count(), 100);
        }

        private UserTestGenerator ConfigObjectSetTimeStamp(UserTestGenerator user)
        {
            var timeStamp = BuildDemoData.SetTimeStamp();
            user.CreateOn = timeStamp.CreateOn;
            user.CreateBy = timeStamp.CreateBy;
            user.ModifiedOn = timeStamp.ModifiedOn;
            user.ModifiedBy = timeStamp.ModifiedBy;

            return user;
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

    [DebuggerDisplay("UserName={this.UserName}")]
    public class UserTestGenerator
    {
        public string UserName { get; set; }
        public DateTime CreateOn { get; set; }
        public string CreateBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
