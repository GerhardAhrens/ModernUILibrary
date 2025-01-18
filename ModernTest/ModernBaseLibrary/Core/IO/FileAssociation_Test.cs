/*
 * <copyright file="FileAssociation_Test.cs" company="Lifeprojects.de">
 *     Class: FileAssociation_Test
 *     Copyright © Lifeprojects.de 2023
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>gerhard.ahrens@lifeprojects.de</email>
 * <date>23.04.2023 09:13:56</date>
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

namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Threading;

    using global::ModernBaseLibrary.Core.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FileAssociation_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAssociation_Test"/> class.
        /// </summary>
        public FileAssociation_Test()
        {
        }

        [TestMethod]
        public void ExistFileExtension()
        {
            string fileExtensionTyp = "test";
            string fullName = this.GetAssemblyFullname;
            string exeName = this.GetAssemblyName;

            FileAssociation fa = new FileAssociation(fileExtensionTyp, fullName, exeName);
            Assert.IsNotNull(fa);
            Assert.IsFalse(fa.ExtensionExist());
        }

        [TestMethod]
        public void RegisterFileExtension()
        {
            string fileExtensionTyp = "test";
            string fullName = this.GetAssemblyFullname;
            string exeName = this.GetAssemblyName;

            FileAssociation fa = new FileAssociation(fileExtensionTyp, fullName, exeName);
            if (fa != null)
            {
                if (fa.ExtensionExist() == false)
                {
                    fa.CreateAssociation();
                }
            }
        }

        [TestMethod]
        public void UnRegisterFileExtension()
        {
            string fileExtensionTyp = "test";
            DirectoryInfo exePath = this.GetAssemblyPath;
            string exeName = this.GetAssemblyName;

            FileAssociation fa = new FileAssociation(fileExtensionTyp, exePath.FullName, exeName);
            if (fa != null)
            {
                if (fa.ExtensionExist() == true)
                {
                    fa.ExtensionDelete();
                }
            }
        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void FileNotFoundTest()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(FileNotFoundException));
            }
        }
    }
}
