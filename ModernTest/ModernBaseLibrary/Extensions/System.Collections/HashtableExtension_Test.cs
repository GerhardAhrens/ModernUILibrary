//-----------------------------------------------------------------------
// <copyright file="HashtableExtension_Test.cs" company="Lifeprojects.de">
//     Class: HashtableExtension_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>14.05.2025 10:07:36</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Collection
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Threading;
    using EasyPrototypingTest;

    using global::ModernBaseLibrary.Extensions.System.Collections;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class HashtableExtension_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HashtableExtension_Test"/> class.
        /// </summary>
        public HashtableExtension_Test()
        {
        }

        [TestMethod]
        public void TryGet_Found()
        {
            Hashtable hashTable = new Hashtable();

            hashTable.Add("IsConnected", true);
            hashTable.Add("MyClass", new TestClass());

            Assert.IsTrue(hashTable.Count == 2);

            bool IsConnected = hashTable.TryGet<bool>("IsConnected", false);
            Assert.AreEqual(IsConnected,true);

            TestClass MyClass = hashTable.TryGet<TestClass>("MyClass", null);
            Assert.IsNotNull(MyClass);
            Assert.IsTrue(MyClass is  TestClass);
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

        private class TestClass
        {

        }
    }
}
