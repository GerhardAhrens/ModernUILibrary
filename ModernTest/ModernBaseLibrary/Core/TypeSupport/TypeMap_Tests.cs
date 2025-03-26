//-----------------------------------------------------------------------
// <copyright file="TypeMap_Tests.cs" company="Lifeprojects.de">
//     Class: TypeMap_Tests
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>26.03.2025 11:25:23</date>
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
    public class TypeMap_Tests
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMap_Tests"/> class.
        /// </summary>
        public TypeMap_Tests()
        {
        }

        [TestMethod]
        public void CtorTest()
        {
            var d = new TypeMap();
            Assert.IsEmpty(d);
            Assert.IsFalse(d.IsReadOnly);
            Assert.IsEmpty(d.Keys);
            Assert.IsEmpty(d.Values);
        }

        [TestMethod]
        public void IDictionaryTest()
        {
            var d = new TypeMap();
            string tn01 = "int";
            string tn02 = "float";
            var tm0 = new TypeMapEntry("", null);
            Assert.IsNull(tm0.Type);

            d[tn01] = new TypeMapEntry(tn01, typeof(int));
            var e1 = d[tn01];
            Assert.IsNotNull(e1);
            Assert.AreEqual(tn01, e1.Name);
            Assert.AreEqual(typeof(int), e1.Type);

            var e0 = new TypeMapEntry(tn02, typeof(float));
            d.Add(tn02, e0);
            Assert.IsTrue(d.ContainsKey(tn02));
            Assert.IsTrue(d.TryGetValue(tn02, out TypeMapEntry e2));
            Assert.AreEqual(2, d.Count);
            Assert.IsTrue(d.Remove(tn02));

            d.Add(new KeyValuePair<string, TypeMapEntry>(tn02, e0));
            Assert.IsTrue(d.Contains(new KeyValuePair<string, TypeMapEntry>(tn02, e0)));
            KeyValuePair<string, TypeMapEntry>[] es = new KeyValuePair<string, TypeMapEntry>[2];
            d.CopyTo(es, 0);
            Array.Sort(es, (x, y) => String.Compare(x.Key, y.Key));
            Assert.AreEqual(tn02, es[0].Key);
            Assert.IsTrue(d.Remove(new KeyValuePair<string, TypeMapEntry>(tn02, e0)));
            foreach (var e in d)
            {
                Assert.AreEqual(tn01, e.Key);
            }

            d.Clear();
            Assert.IsEmpty(d);
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
