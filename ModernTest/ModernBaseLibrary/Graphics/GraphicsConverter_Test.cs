/*
 * <copyright file="GraphicsConverter_Test.cs" company="Lifeprojects.de">
 *     Class: GraphicsConverter_Test
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
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Threading;
    using System.Windows.Media.Imaging;

    using global::ModernBaseLibrary.CoreBase.Reflection;
    using global::ModernBaseLibrary.Extension;
    using global::ModernBaseLibrary.Graphics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GraphicsConverter_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsConverter_Test"/> class.
        /// </summary>
        public GraphicsConverter_Test()
        {
        }

        [TestMethod]
        public void ConvertByteArrayToBitmapSource()
        {
            Assembly loadedAssemblies = RootAssemblyHelper.RootAssembly;
            List<string> resourceNames = loadedAssemblies.GetResourceNames();
            string imageByName = resourceNames.Find(x => x.Contains("ExtensionBin") == true);
            byte[] extensionIcon = FileExtensionImage.Get(imageByName, loadedAssemblies);

            BitmapSource bmp = GraphicsConverter.ByteArrayToBitmapSource(extensionIcon);
            Assert.IsTrue(bmp.Height > 339.4 && bmp.Width > 339.4);

            byte[] images = GraphicsConverter.BitmapSourcePngToByteArray(bmp);
            Assert.IsTrue(images.Length == 13122);
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
