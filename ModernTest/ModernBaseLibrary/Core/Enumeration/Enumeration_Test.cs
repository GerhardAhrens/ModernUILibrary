namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using global::ModernBaseLibrary.Core;
    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class Enumeration_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enumeration_Test"/> class.
        /// </summary>
        public Enumeration_Test()
        {
        }

        [TestMethod]
        public void Create_Enumeration()
        {
            TestEnumeration a = TestEnumeration.A;
            Assert.IsNotNull(a);
            Assert.IsTrue(a.Name == "A");
        }

        [TestMethod]
        public void Enumeration_GetNames()
        {
            TestEnumeration a = TestEnumeration.A;
            TestEnumeration b = TestEnumeration.B;

            IEnumerable<string> names = TestEnumeration.GetNames<TestEnumeration>();
            Assert.IsTrue(names.Count() == 2);
        }

        [TestMethod]
        public void Enumeration_GetValues()
        {
            TestEnumeration a = TestEnumeration.A;
            TestEnumeration b = TestEnumeration.B;

            IEnumerable<TestEnumeration> values = TestEnumeration.GetValues<TestEnumeration>();
            Assert.IsTrue(values.Count() == 2);
        }

        [TestMethod]
        public void Enumeration_Format()
        {
            TestEnumeration a = TestEnumeration.A;

            string format = a.ToString();
            string format1 = a.ToString("G");
            string format2 = a.ToString("D");
            string format3 = a.ToString("X");
        }

        [TestMethod]
        public void Enumeration_Equals()
        {
            TestEnumeration a1 = TestEnumeration.A;
            TestEnumeration a2 = TestEnumeration.A;

            Assert.AreEqual(a1, a2);
        }

        [TestMethod]
        public void Enumeration_NotEquals()
        {
            TestEnumeration a1 = TestEnumeration.A;
            TestEnumeration a2 = TestEnumeration.B;

            Assert.AreNotEqual(a1, a2);
        }

        [TestMethod]
        public void Enumeration_In1()
        {
            TestEnumeration a1 = TestEnumeration.A;
            bool isFound = a1.In(TestEnumeration.A);
            Assert.IsTrue(isFound);
        }

        [TestMethod]
        public void Enumeration_In2()
        {
            TestEnumeration a1 = TestEnumeration.A;

            bool isFound = a1.In(TestEnumeration.A, TestEnumeration.B, TestEnumeration.C);
            Assert.IsTrue(isFound);
        }

        [TestMethod]
        public void Enumeration_In3()
        {
            TestEnumeration a1 = TestEnumeration.A;
            bool isFound = a1.In(TestEnumeration.B);
            Assert.IsFalse(isFound);
        }

        [TestMethod]
        public void Enumeration_NotIn1()
        {
            TestEnumeration a1 = TestEnumeration.A;

            bool isNotFound = a1.NotIn(TestEnumeration.B);
            Assert.IsTrue(isNotFound);
        }

        [TestMethod]
        public void Enumeration_NotIn2()
        {
            TestEnumeration a1 = TestEnumeration.A;

            bool isNotFound = a1.NotIn(TestEnumeration.B, TestEnumeration.C, TestEnumeration.D);
            Assert.IsTrue(isNotFound);
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

    public class TestEnumeration : Enumeration
    {
        public static readonly TestEnumeration A = new TestEnumeration(0, nameof(A), "Test-A");
        public static readonly TestEnumeration B = new TestEnumeration(1, nameof(B), "Test-B");
        public static readonly TestEnumeration C = new TestEnumeration(2, nameof(C), "Test-C");
        public static readonly TestEnumeration D = new TestEnumeration(3, nameof(D), "Test-D");
        //...

        private static readonly IEnumerable<TestEnumeration> testData;

        protected TestEnumeration(int id, string name, string description) : base(id, name, description)
        {
        }

        static TestEnumeration()
        {
            testData = GetValues<TestEnumeration>();
        }

        public static IEnumerable<TestEnumeration> Values()
        {
            foreach (var entry in testData)
            {
                yield return entry;
            }
        }
    }
}
