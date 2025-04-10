//-----------------------------------------------------------------------
// <copyright file="DictionaryByType_Test.cs" company="Lifeprojects.de">
//     Class: DictionaryByType_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>10.04.2025 09:15:10</date>
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
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using global::ModernBaseLibrary.Collection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class DictionaryByType_Test
    {
        private DictionaryByType subject;


        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryByType_Test"/> class.
        /// </summary>
        public DictionaryByType_Test()
        {
            subject = new DictionaryByType();
        }

        [TestMethod]
        public void AddThenGet()
        {
            object o = new object();
            subject.Add("hi");
            subject.Add(10);
            subject.Add(o);

            Assert.AreEqual("hi", subject.Get<string>());
            Assert.AreEqual(10, subject.Get<int>());
            Assert.AreSame(o, subject.Get<object>());
        }

        [TestMethod]
        public void PutThenGet()
        {
            object o = new object();
            subject.Put("hi");
            subject.Put(10);
            subject.Put(o);

            Assert.AreEqual("hi", subject.Get<string>());
            Assert.AreEqual(10, subject.Get<int>());
            Assert.AreSame(o, subject.Get<object>());
        }

        [TestMethod]
        public void RepeatedAddForSameTypeThrowsException()
        {
            subject.Add("Hi");
            try
            {
                subject.Add("There");
                Assert.Fail("Expected exception");
            }
            catch (ArgumentException)
            {
                // Expected
            }
        }

        [TestMethod]
        public void RepeatedPutForSameTypeOverwritesValue()
        {
            subject.Put("Hi");
            Assert.AreEqual("Hi", subject.Get<string>());
            subject.Put("There");
            Assert.AreEqual("There", subject.Get<string>());
        }

        [TestMethod]
        public void GetFailsForMissingType()
        {
            try
            {
                subject.Get<string>();
                Assert.Fail("Expected exception");
            }
            catch (KeyNotFoundException)
            {
                // Expected
            }
        }

        [TestMethod]
        public void TryGetSucceedsForMissingType()
        {
            string x;
            Assert.IsFalse(subject.TryGet(out x));
            Assert.IsNull(x);
        }

        [TestMethod]
        public void TryGetFillsInValueForPresentType()
        {
            subject.Put("Hi");
            string x;
            Assert.IsTrue(subject.TryGet(out x));
            Assert.AreEqual("Hi", x);
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
