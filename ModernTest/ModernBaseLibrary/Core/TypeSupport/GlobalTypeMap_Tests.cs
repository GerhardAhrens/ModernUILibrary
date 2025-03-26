//-----------------------------------------------------------------------
// <copyright file="GlobalTypeMap_Tests.cs" company="Lifeprojects.de">
//     Class: GlobalTypeMap_Tests
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>26.03.2025 11:04:11</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Core.TypeSupport
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
    using System.Xml.Linq;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class GlobalTypeMap_Tests
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalTypeMap_Tests"/> class.
        /// </summary>
        public GlobalTypeMap_Tests()
        {
        }

        [TestMethod]
        public void xyz_xyz()
        {
        }

        [DataRow("int", typeof(int))]
        [DataRow("float", typeof(float))]
        [DataRow("string", typeof(string))]
        [TestMethod]
        public void AliasTests(string input, Type expected)
        {
            var d = new GlobalTypeMap();
            Assert.AreEqual(expected, d.FindType(input));
        }

        [DataRow("Int32", typeof(Int32))]
        [DataRow("Single", typeof(Single))]
        [DataRow("String", typeof(string))]
        [TestMethod]
        public void TypeCodeTests(string input, Type expected)
        {
            var d = new GlobalTypeMap();
            Assert.AreEqual(expected, d.FindType(input));
        }

        [DataRow("System.Int32", typeof(int))]
        [DataRow("System.Single", typeof(float))]
        [DataRow("System.String", typeof(string))]
        [DataRow("System.IO.Path", typeof(Path))]
        [TestMethod]
        public void FullNameTests(string input, Type expected)
        {
            var d = new GlobalTypeMap();
            Assert.AreEqual(expected, d.FindType(input));
        }

        [DataRow("NamespaceA.ClassA", typeof(NamespaceA.ClassA))]
        [DataRow("NamespaceA.ClassA+EnumD", typeof(NamespaceA.ClassA.EnumD))]
        [DataRow("NamespaceA.ClassA+ClassB", typeof(NamespaceA.ClassA.ClassB))]
        [DataRow("NamespaceA.ClassA+ClassB+ClassC", typeof(NamespaceA.ClassA.ClassB.ClassC))]
        [TestMethod]
        public void LocalTypeTests(string input, Type expected)
        {
            var d = new GlobalTypeMap();
            Assert.AreEqual(expected, d.FindType(input));
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

namespace NamespaceA
{
    public class ClassA
    {
        public int A = 1;
        public class ClassB
        {
            public int b = 1;
            public class ClassC
            {
                public int c = 1;
            }
        }
        public enum EnumD
        {
            E,
            F
        }
    }

    public class ClassD
    {
    }

    public class ClassE
    {
    }
}
