//-----------------------------------------------------------------------
// <copyright file="TypeSupport_Tests.cs" company="Lifeprojects.de">
//     Class: TypeSupport_Tests
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>26.03.2025 11:30:57</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Core.TypeSupport
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class TypeSupport_Tests
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSupport_Tests"/> class.
        /// </summary>
        public TypeSupport_Tests()
        {
        }

        [TestMethod]
        [DataRow(typeof(NamespaceA.ClassE), "NamespaceA.ClassE, ModernTest")]
        [DataRow(null, "")]
        public void FormatAssemblyQualifiedNameTests(Type t0, string s0)
        {
            string s1 = TypeSupport.FormatAssemblyQualifiedName(t0);
            Assert.AreEqual(s0, s1);
        }

        [TestMethod]
        [DataRow(typeof(NamespaceA.ClassE), "NamespaceA.ClassE", "ModernTest")]
        [DataRow(null, "", "")]
        public void ParseAssemblyQualifiedName(Type t0, string tn0, string an0)
        {
            (string tn1, string an1) = TypeSupport.ParseAssemblyQualifiedName(t0 == null ? null : t0.AssemblyQualifiedName);
            Assert.AreEqual(tn0, tn1);
            Assert.AreEqual(an0, an1);
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
