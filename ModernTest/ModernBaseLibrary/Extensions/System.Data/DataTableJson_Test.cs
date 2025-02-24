/*
 * <copyright file="ExpandoObjectTests.cs" company="Lifeprojects.de">
 *     Class: ExpandoObjectTests
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>23.02.2025</date>
 * <Project>ModernTest.ModernBaseLibrary</Project>
 *
 * <summary>
 * Test Klasse für den Typ ExpandoObject
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
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Threading;

    using DemoDataGeneratorLib.Base;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Summary description for DataTable_Test
    /// </summary>
    [TestClass]
    public class DataTableJson_Test : BaseTest
    {
        public string TestDirPath => TestContext.TestRunDirectory;
        public string TempDirPath => Path.Combine(TestDirPath, "Temp");

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void DataTableToJson()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Extensions\\System.Data\\DemoData\\DataTabelToJson_A.json");

            DataTable usersDt = BuildDemoData<UserDemoDaten>.CreateForDataTable<UserDemoDaten>(ConfigObject, 1);
            if (usersDt.Rows.Count > 0)
            {
                string jsonText = usersDt.ToJson();
                File.WriteAllText(pathFileName, jsonText);
            }
        }

        [TestMethod]
        public void JsonToDataTable()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Extensions\\System.Data\\DemoData\\DataTabelToJson_A.json");
            string jsonText = File.ReadAllText(pathFileName);
            var aa = jsonText.JsonToDataTable<UserDemoDaten>(nameof(UserDemoDaten));
        }

        private UserDemoDaten ConfigObject(UserDemoDaten demoDaten, int counter)
        {
            var timeStamp = BuildDemoData.SetTimeStamp();
            demoDaten.UserName = BuildDemoData.Username();
            demoDaten.Betrag = BuildDemoData.CurrencyValue(1_000, 10_000);
            demoDaten.IsDeveloper = BuildDemoData.Boolean();
            demoDaten.City = BuildDemoData.City();
            demoDaten.CreateOn = timeStamp.CreateOn;
            demoDaten.CreateBy = timeStamp.CreateBy;
            demoDaten.ModifiedOn = timeStamp.ModifiedOn;
            demoDaten.ModifiedBy = timeStamp.ModifiedBy;

            return demoDaten;
        }


        [DebuggerDisplay("UserName={this.UserName}")]
        private class UserDemoDaten
        {
            public string UserName { get; set; }
            public bool IsDeveloper { get; set; }
            public decimal Betrag { get; set; }
            public string City { get; set; }
            public DateTime CreateOn { get; set; }
            public string CreateBy { get; set; }
            public DateTime ModifiedOn { get; set; }
            public string ModifiedBy { get; set; }
        }
    }
}
