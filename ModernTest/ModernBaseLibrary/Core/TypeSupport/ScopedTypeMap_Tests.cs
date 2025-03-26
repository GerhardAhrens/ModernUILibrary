//-----------------------------------------------------------------------
// <copyright file="ScopedTypeMap_Tests.cs" company="Lifeprojects.de">
//     Class: ScopedTypeMap_Tests
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>26.03.2025 11:20:17</date>
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
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class ScopedTypeMap_Tests
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopedTypeMap_Tests"/> class.
        /// </summary>
        public ScopedTypeMap_Tests()
        {
        }

        [TestMethod]
        public void xyz_xyz()
        {
        }

        [DataRow("NamespaceA.ClassD", "", typeof(NamespaceA.ClassD))]
        [DataRow("ClassD", "NamespaceA", typeof(NamespaceA.ClassD))]
        [TestMethod]
        public void DataRowInputTest(string tn0, string nn0, Type t0)
        {
            GlobalTypeMap g = new GlobalTypeMap();
            ScopedTypeMap u = new ScopedTypeMap(g);
            u.UsingNamespace(nn0);
            Type t1 = u.FindType(tn0);
            Assert.AreEqual(t0, t1);
        }

        [TestMethod]
        [DataRow("NamespaceA.ClassD", "TypeSupportTests", typeof(NamespaceA.ClassD))]
        [DataRow("ClassD", "TypeSupportTests", typeof(NamespaceA.ClassD))]
        public void UsingAssemblyTest(string tn0, string an0, Type t0)
        {
            GlobalTypeMap g = new GlobalTypeMap();
            ScopedTypeMap u = new ScopedTypeMap(g);
            u.UsingAssembly(an0);
            Type t1 = u.FindType(tn0);
            Assert.AreEqual(t0, t1);
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
