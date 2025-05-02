//-----------------------------------------------------------------------
// <copyright file="CDT_Base64.cs" company="Lifeprojects.de">
//     Class: CDT_Base64
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>02.05.2025 11:12:02</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.CustomDataTypes
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
    using global::ModernBaseLibrary.Core.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class CDT_Base64
    {
        private FullPath rootPath;
        private FullPath demoDataPath;

        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            rootPath = FullPath.FromPath("..\\..\\..\\");
            demoDataPath = rootPath / "ModernBaseLibrary" / "CustomDataTypes" / "DemoData" / "Demo.png";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CDT_Base64"/> class.
        /// </summary>
        public CDT_Base64()
        {
        }

        [TestMethod]
        public void CreateBase64()
        {
            if (File.Exists(demoDataPath))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(demoDataPath);
                if (img != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        img.Save(ms, img.RawFormat);
                        Base64 base64 = ms.ToArray();
                        Assert.IsTrue(base64.Length > 0);
                        Assert.IsTrue(base64.Value is string);
                    }
                }
            }
        }

        [TestMethod]
        public void CreateBase64Compare()
        {
            if (File.Exists(demoDataPath))
            {
                System.Drawing.Image img = System.Drawing.Image.FromFile(demoDataPath);
                if (img != null)
                {
                    using (var ms = new MemoryStream())
                    {
                        img.Save(ms, img.RawFormat);
                        Base64 base64 = ms.ToArray();
                        Assert.IsTrue(base64.ToByteArray().Length > 0);
                        Assert.IsTrue(base64.ToByteArray().SequenceEqual(ms.ToArray()));
                    }
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
