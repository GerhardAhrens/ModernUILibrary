/*
 * <copyright file="GraphicsHelper_Test.cs" company="Lifeprojects.de">
 *     Class: GraphicsHelper_Test
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>29.12.2022 16:20:36</date>
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

namespace ModernTest.ModernBaseLibrary.Graphics
{
    using System;
    using System.Drawing.Imaging;
    using System.Globalization;
    using System.IO;
    using System.Threading;

    using global::ModernBaseLibrary.Graphics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GraphicsHelper_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsHelper_Test"/> class.
        /// </summary>
        public GraphicsHelper_Test()
        {
        }

        [TestMethod]
        public void CreateImageFromTextAsJPG()
        {
            string imagePath = $"{this.GetAssemblyPath.FullName}\\TestImage.jpg";
            System.Drawing.Color color = System.Drawing.Color.FromName("Red");
            GraphicsHelper.CreateImageFromText("Hallo Test", new System.Drawing.Font("Arial", 18), color, 200, imagePath, ImageFormat.Jpeg);

            Assert.IsTrue(File.Exists(imagePath));
        }

        [TestMethod]
        public void CreateImageFromTextAsGIF()
        {
            string imagePath = $"{this.GetAssemblyPath.FullName}\\TestImage.gif";
            System.Drawing.Color color = System.Drawing.Color.FromName("Red");
            GraphicsHelper.CreateImageFromText("Hallo Test", new System.Drawing.Font("Arial", 18), color, 200, imagePath, ImageFormat.Gif);

            Assert.IsTrue(File.Exists(imagePath));
        }

        [TestMethod]
        public void CreateImageFromTextAsPNG()
        {
            string imagePath = $"{this.GetAssemblyPath.FullName}\\TestImage.png";
            System.Drawing.Color color = System.Drawing.Color.FromName("Red");
            GraphicsHelper.CreateImageFromText("Hallo Test", new System.Drawing.Font("Arial",18), color, 200, imagePath);

            Assert.IsTrue(File.Exists(imagePath));
        }

        [TestMethod]
        public void CreateImageFromTextAsPNGMinimal()
        {
            string imagePath = $"{this.GetAssemblyPath.FullName}\\TestImage.png";
            GraphicsHelper.CreateImageFromText("Hallo Test", 18, 200, imagePath);

            Assert.IsTrue(File.Exists(imagePath));
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
