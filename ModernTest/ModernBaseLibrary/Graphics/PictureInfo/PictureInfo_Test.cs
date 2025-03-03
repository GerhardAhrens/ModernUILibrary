//-----------------------------------------------------------------------
// <copyright file="PictureInfo_Test.cs" company="Lifeprojects.de">
//     Class: PictureInfo_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>03.03.2025 17:42:33</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Graphics
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.IO;
    using ModernBaseLibrary.Graphics;
    using global::ModernBaseLibrary.Graphics;

    [TestClass]
    public class PictureInfo_Test : BaseTest
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
        /// Initializes a new instance of the <see cref="PictureInfo_Test"/> class.
        /// </summary>
        public PictureInfo_Test()
        {
        }

        [TestMethod]
        public void PngInfo_Test()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Graphics\\PictureInfo\\DemoData\\Demo.png");

            if (File.Exists(pathFileName) == true)
            {
                using (FileStream sr = new FileStream(pathFileName, FileMode.Open, FileAccess.Read))
                {
                    ImageInfoBase picInfo = ImageInfo.GetInfo(ImageInfo.ImageInfoType.Png, (Stream)sr);
                }
            }
        }

        [TestMethod]
        public void GifInfo_Test()
        {
            DirectoryInfo di = new DirectoryInfo(TempDirPath);
            string pathFileName = Path.GetFullPath($"{di.Parent.Parent.Parent}\\ModernTest\\ModernBaseLibrary\\Graphics\\PictureInfo\\DemoData\\Demo.gif");

            if (File.Exists(pathFileName) == true)
            {
                using (FileStream sr = new FileStream(pathFileName, FileMode.Open, FileAccess.Read))
                {
                    ImageInfoBase picInfo = ImageInfo.GetInfo(ImageInfo.ImageInfoType.Gif, (Stream)sr);
                }
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
