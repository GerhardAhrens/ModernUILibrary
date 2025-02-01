/*
 * <copyright file="VCalendar_Test.cs" company="Lifeprojects.de">
 *     Class: VCalendar_Test
 *     Copyright © Lifeprojects.de 2025
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>01.02.2025 20:09:06</date>
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

namespace ModernTest.ModernBaseLibrary.Data.VCalendar
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using global::ModernBaseLibrary.Extension;
    using global::ModernBaseLibrary.VCalendar;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class VCalendar_Test : BaseTest
    {
        private string TestDirPath => TestContext.TestRunDirectory;
        private string TempDirPath => Path.Combine(TestDirPath, "Temp");

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VCalendar_Test"/> class.
        /// </summary>
        public VCalendar_Test()
        {
        }

        [TestMethod]
        public void VCalendarReader_Test()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Data\\VCalendar\\DemoData\\Test.ics");
            if (File.Exists(pathFileName) == true)
            {
                VCalendarReader vReader = new VCalendarReader(pathFileName);
                VCalendarParser parser = new VCalendarParser(vReader);
                Assert.IsNotNull(parser);

                List<VCalendarEvent> content = parser.Contents;
                Assert.IsNotNull(content);
                Assert.IsTrue(content.Count > 0);

                VCalendarEvent calEvent = content.FirstOrDefault();

                DateTime startDate = VCalendarParser.ToDateTime(calEvent.DTStart);
                DateTime endDate = VCalendarParser.ToDateTime(calEvent.DTEnd);

                Assert.AreEqual(startDate, new DateTime(2025,2,1,19,0,0));
                Assert.AreEqual(endDate, new DateTime(2025, 2, 2, 20, 0, 0));
            }
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
