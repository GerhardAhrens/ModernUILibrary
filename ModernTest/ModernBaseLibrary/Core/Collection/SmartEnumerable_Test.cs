//-----------------------------------------------------------------------
// <copyright file="SmartEnumerable_Test.cs" company="Lifeprojects.de">
//     Class: SmartEnumerable_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>10.04.2025 09:22:53</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Core.Collection
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Collection;
    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class SmartEnumerable_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SmartEnumerable_Test"/> class.
        /// </summary>
        public SmartEnumerable_Test()
        {
        }

        [TestMethod]
        public void EmptyEnumerable()
        {
            List<string> emptyList = new List<string>();

            SmartEnumerable<string> subject = new SmartEnumerable<string>(emptyList);
            using (IEnumerator<SmartEnumerable<string>.Entry> iterator = subject.GetEnumerator())
            {
                Assert.IsFalse(iterator.MoveNext());
            }
        }

        [TestMethod]
        public void SingleEntryEnumerable()
        {
            List<string> list = new List<string>();
            list.Add("x");
            TestSingleEntry(new SmartEnumerable<string>(list));
        }


        [TestMethod]
        public void SingleEntryEnumerableViaExtension()
        {
            List<string> list = new List<string>();
            list.Add("x");

            TestSingleEntry(list.AsSmartEnumerable());
        }

        [TestMethod]
        public void SingleEntryEnumerableViaCreate()
        {
            List<string> list = new List<string>();
            list.Add("x");

            TestSingleEntry(SmartEnumerable.Create(list));
        }

        private static void TestSingleEntry(SmartEnumerable<string> subject)
        {
            using (IEnumerator<SmartEnumerable<string>.Entry> iterator = subject.GetEnumerator())
            {
                Assert.IsTrue(iterator.MoveNext());
                Assert.IsTrue(iterator.Current.IsFirst);
                Assert.IsTrue(iterator.Current.IsLast);
                Assert.AreEqual("x", iterator.Current.Value);
                Assert.AreEqual(0, iterator.Current.Index);
                Assert.IsFalse(iterator.MoveNext());
            }
        }

        [TestMethod]
        public void SingleEntryUntypedEnumerable()
        {
            List<string> list = new List<string>();
            list.Add("x");
            IEnumerable subject = new SmartEnumerable<string>(list);

            int index = 0;
            foreach (SmartEnumerable<string>.Entry item in subject)
            { // only expecting 1
                Assert.AreEqual(0, index++);
                Assert.AreEqual("x", item.Value);
                Assert.IsTrue(item.IsFirst);
                Assert.IsTrue(item.IsLast);
                Assert.AreEqual(0, item.Index);
            }
            Assert.AreEqual(1, index);
        }

        [TestMethod]
        public void DoubleEntryEnumerable()
        {
            List<string> list = new List<string>();
            list.Add("x");
            list.Add("y");

            SmartEnumerable<string> subject = new SmartEnumerable<string>(list);
            using (IEnumerator<SmartEnumerable<string>.Entry> iterator = subject.GetEnumerator())
            {
                Assert.IsTrue(iterator.MoveNext());
                Assert.IsTrue(iterator.Current.IsFirst);
                Assert.IsFalse(iterator.Current.IsLast);
                Assert.AreEqual("x", iterator.Current.Value);
                Assert.AreEqual(0, iterator.Current.Index);

                Assert.IsTrue(iterator.MoveNext());
                Assert.IsFalse(iterator.Current.IsFirst);
                Assert.IsTrue(iterator.Current.IsLast);
                Assert.AreEqual("y", iterator.Current.Value);
                Assert.AreEqual(1, iterator.Current.Index);
                Assert.IsFalse(iterator.MoveNext());
            }
        }

        [TestMethod]
        public void TripleEntryEnumerable()
        {
            List<string> list = new List<string>();
            list.Add("x");
            list.Add("y");
            list.Add("z");

            SmartEnumerable<string> subject = new SmartEnumerable<string>(list);
            using (IEnumerator<SmartEnumerable<string>.Entry> iterator = subject.GetEnumerator())
            {
                Assert.IsTrue(iterator.MoveNext());
                Assert.IsTrue(iterator.Current.IsFirst);
                Assert.IsFalse(iterator.Current.IsLast);
                Assert.AreEqual("x", iterator.Current.Value);
                Assert.AreEqual(0, iterator.Current.Index);

                Assert.IsTrue(iterator.MoveNext());
                Assert.IsFalse(iterator.Current.IsFirst);
                Assert.IsFalse(iterator.Current.IsLast);
                Assert.AreEqual("y", iterator.Current.Value);
                Assert.AreEqual(1, iterator.Current.Index);

                Assert.IsTrue(iterator.MoveNext());
                Assert.IsFalse(iterator.Current.IsFirst);
                Assert.IsTrue(iterator.Current.IsLast);
                Assert.AreEqual("z", iterator.Current.Value);
                Assert.AreEqual(2, iterator.Current.Index);
                Assert.IsFalse(iterator.MoveNext());
            }
        }
        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void NullEnumerableThrowsException()
        {
            try
            {
                new SmartEnumerable<string>(null);
                Assert.Fail("Expected exception");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
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
