//-----------------------------------------------------------------------
// <copyright file="FileExtensionImage_Test.cs" company="Lifeprojects.de">
//     Class: FileExtensionImage_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>13.03.2025 08:34:44</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Graphics
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Windows;
    using System.Windows.Media.Imaging;

    using global::ModernBaseLibrary.Core;
    using global::ModernBaseLibrary.CoreBase.Reflection;
    using global::ModernBaseLibrary.Extension;
    using global::ModernBaseLibrary.Graphics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FileExtensionImage_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileExtensionImage_Test"/> class.
        /// </summary>
        public FileExtensionImage_Test()
        {
        }

        [TestMethod]
        public void GetFileExtensionIconFromResources()
        {
            Assembly loadedAssemblies = RootAssemblyHelper.RootAssembly;

            List<string> resourceNames = loadedAssemblies.GetResourceNames();
            byte[] extensionIcon = FileExtensionImage.Get(resourceNames[7], loadedAssemblies);
            BitmapSource bmp = GraphicsConverter.ByteArrayToBitmapSource(extensionIcon);
            Assert.IsTrue(bmp.Height > 339.4 && bmp.Width > 339.4);
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
