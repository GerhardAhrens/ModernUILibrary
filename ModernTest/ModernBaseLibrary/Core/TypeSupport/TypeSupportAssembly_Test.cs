//-----------------------------------------------------------------------
// <copyright file="TypeSupportAssembly_Test.cs" company="Lifeprojects.de">
//     Class: TypeSupportAssembly_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>26.03.2025 10:42:37</date>
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
    using System.Reflection;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    using ModernTest;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class TypeSupportAssembly_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSupportAssembly_Test"/> class.
        /// </summary>
        public TypeSupportAssembly_Test()
        {
        }

        [TestMethod]
        public void IDictionary_Test()
        {
            var d = new AssemblyMap();
            Assembly a01 = Assembly.GetExecutingAssembly();
            Assembly a02 = typeof(AssemblyMap).Assembly;
            string an01 = a01.GetName().Name;
            string an02 = a02.GetName().Name;
            var tm0 = new AssemblyMapEntry("", null);
            Assert.IsNull(tm0.Assembly);

            d[an01] = new AssemblyMapEntry(an01, a01);
            var e1 = d[an01];
            Assert.IsNotNull(e1);
            Assert.AreEqual(an01, e1.Name);
            Assert.AreEqual(a01.FullName, e1.Assembly.FullName);

            var e0 = new AssemblyMapEntry(an02, a02);
            d.Add(an02, e0);
            Assert.IsTrue(d.ContainsKey(an02));
            Assert.IsTrue(d.TryGetValue(an02, out AssemblyMapEntry e2));
            Assert.AreEqual(2, d.Count);
            Assert.IsTrue(d.Remove(an02));

            d.Add(new KeyValuePair<string, AssemblyMapEntry>(an02, e0));
            Assert.IsTrue(d.Contains(new KeyValuePair<string, AssemblyMapEntry>(an02, e0)));

            KeyValuePair<string, AssemblyMapEntry>[] es = new KeyValuePair<string, AssemblyMapEntry>[2];
            d.CopyTo(es, 0);
            Array.Sort(es, (x, y) => String.Compare(x.Key, y.Key));
            Assert.AreEqual(an02, es[0].Key);
            Assert.IsTrue(d.Remove(new KeyValuePair<string, AssemblyMapEntry>(an02, e0)));
            foreach (var e in d)
            {
                Assert.AreEqual(an01, e.Key);
            }

            d.Clear();
            Assert.That.IsEmpty(d);
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
