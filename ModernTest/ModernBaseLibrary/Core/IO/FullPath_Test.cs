//-----------------------------------------------------------------------
// <copyright file="FullPath_Test.cs" company="Lifeprojects.de">
//     Class: FullPath_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>02.05.2025 10:37:21</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Core.IO
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using global::ModernBaseLibrary.Core.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class FullPath_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FullPath_Test"/> class.
        /// </summary>
        public FullPath_Test()
        {
        }

        [TestMethod]
        public void IsEmpty()
        {
            Assert.IsTrue(default(FullPath).IsEmpty);
            Assert.IsTrue(FullPath.Empty.IsEmpty);
            Assert.IsFalse(FullPath.FromPath("test").IsEmpty);
        }

        [TestMethod]
        public void Properties()
        {
            var path = FullPath.FromPath("test") / "a" / "b.txt";
            Assert.IsFalse(path.IsEmpty);
            Assert.AreEqual("b.txt", path.Name);
            Assert.AreEqual(".txt", path.Extension);
            Assert.AreEqual("b", path.NameWithoutExtension);
            Assert.AreEqual(FullPath.FromPath("test") / "a", path.Parent);
        }
        [TestMethod]
        public void GetRootPathFromPath()
        {
            FullPath rootPath = FullPath.FromPath("..\\..\\..\\..");
            Assert.AreEqual(rootPath, "C:\\_Projekte\\_Git_Private\\ModernUI");
        }

        [TestMethod]
        public void GetRootPathCombine()
        {
            FullPath rootPath = FullPath.FromPath("..\\..\\..\\..");
            Assert.AreEqual(rootPath, "C:\\_Projekte\\_Git_Private\\ModernUI");

            FullPath readmePath = FullPath.Combine(rootPath, "ModernTemplate\\_Doc\\README.md");
            Assert.AreEqual(readmePath, "C:\\_Projekte\\_Git_Private\\ModernUI\\ModernTemplate\\_Doc\\README.md");

            Assert.IsTrue(File.Exists(readmePath) == true);
        }

        [TestMethod]
        public void GetRootPathCombineSeparator()
        {
            FullPath rootPath = FullPath.FromPath("..\\..\\..\\..");
            Assert.AreEqual(rootPath, "C:\\_Projekte\\_Git_Private\\ModernUI");

            FullPath readmePath = rootPath / "ModernTemplate\\_Doc\\" / "README.md";
            Assert.AreEqual(readmePath, "C:\\_Projekte\\_Git_Private\\ModernUI\\ModernTemplate\\_Doc\\README.md");

            Assert.IsTrue(File.Exists(readmePath) == true);
        }

        [TestMethod]
        public void FullPathComaprePath()
        {
            FullPath rootPath = FullPath.FromPath("..\\..\\..\\..");
            FullPath readmePath = rootPath / "ModernTemplate\\_Doc\\" / "README.md";

            Assert.IsTrue(readmePath == "C:\\_Projekte\\_Git_Private\\ModernUI\\ModernTemplate\\_Doc\\README.md");
        }

        [TestMethod]
        public void FullPathGetParent()
        {
            FullPath rootPath = FullPath.FromPath("..\\..\\..\\..");
            FullPath readmePath = rootPath / "ModernTemplate\\_Doc\\" / "README.md";

            Assert.IsTrue(readmePath.Parent == "C:\\_Projekte\\_Git_Private\\ModernUI\\ModernTemplate\\_Doc");
        }

        [TestMethod]
        public void JsonSerialize_RoundTripEmpty()
        {
            var value = FullPath.Empty;
            Assert.AreEqual(value, JsonSerializer.Deserialize<FullPath>(JsonSerializer.Serialize(value)));
        }

        [TestMethod]
        public void JsonSerialize_RoundTripNonEmpty()
        {
            var value = FullPath.FromPath(@"c:\test");
            Assert.AreEqual(value, JsonSerializer.Deserialize<FullPath>(JsonSerializer.Serialize(value)));
        }

        [TestMethod]
        public void JsonSerialize_Empty()
        {
            Assert.AreEqual("\"\"", JsonSerializer.Serialize(FullPath.Empty));
        }

        [TestMethod]
        public void JsonSerialize_NonEmpty()
        {
            var path = Environment.CurrentDirectory;
            Assert.AreEqual(JsonSerializer.Serialize(path), JsonSerializer.Serialize(FullPath.FromPath(path)));
            Assert.AreEqual(path, JsonSerializer.Deserialize<FullPath>(JsonSerializer.Serialize(FullPath.FromPath(path))).Value);
        }

        [TestMethod]
        public void JsonDeserialize_Null()
        {
            Assert.AreEqual(FullPath.Empty, JsonSerializer.Deserialize<FullPath>(@"null"));
        }

        [TestMethod]
        public void JsonDeserialize_Empty()
        {
            Assert.AreEqual(FullPath.Empty, JsonSerializer.Deserialize<FullPath>(@""""""));
        }

        [TestMethod]
        public void JsonDeserialize_NonEmpty()
        {
            Assert.AreEqual(FullPath.FromPath(@"c:\test"), JsonSerializer.Deserialize<FullPath>(@"""c:\\test"""));
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
