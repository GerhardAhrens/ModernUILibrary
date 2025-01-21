namespace ModernTest.ModernBaseLibrary.CustomDataTypes
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CDT_Effort_Test
    {
        [TestInitialize]
        public void TestSetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void CreateTypeWithValue()
        {
            Effort result = 1;
            Assert.AreEqual<Effort>(result, new Effort(1));
        }

        [TestMethod]
        public void CreateTypeWithNull()
        {
            try
            {
                Effort result = null;
                Assert.AreEqual<Effort>(result,null);
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentNullException));
            }
        }

        [TestMethod]
        public void EffortIsEqual()
        {
            Effort result1 = 1;
            Effort result2 = 1;
            Assert.AreEqual<Effort>(result1, result2);
        }

        [TestMethod]
        public void EffortIsNotEqual()
        {
            Effort result1 = 1;
            Effort result2 = 10;
            Assert.AreNotEqual<Effort>(result1, result2);
        }

        [TestMethod]
        public void EffortAddValue()
        {
            Effort e1 = 1;
            Effort e2 = 10;
            Effort result = e1 + e2;
            Assert.AreEqual<Effort>(result, new Effort(11));

            Effort e3 = 1.5;
            Effort e4 = 2.6;
            Effort result2 = e3 + e4;
            Assert.AreEqual<Effort>(result2, new Effort(4.1));
        }

        [TestMethod]
        public void EffortSubValue()
        {
            Effort e1 = 10;
            Effort e2 = 1;
            Effort result = e1 - e2;
            Assert.AreEqual<Effort>(result, new Effort(9));

            Effort e3 = 9.5;
            Effort e4 = 1.6;
            Effort result2 = e3 - e4;
            Assert.AreEqual<Effort>(result2, new Effort(7.9));
        }

        [TestMethod]
        public void EffortToStringValue()
        {
            Effort e1 = 10;
            Assert.AreEqual(e1.ToString(),"10");

            Effort e2 = 2.4;
            Assert.AreEqual(e2.ToString(CultureInfo.CurrentCulture), "2,4");
        }

        [TestMethod]
        public void EffortToHours()
        {
            Effort e1 = 10;
            Assert.AreEqual(e1.ToHours(), 80);
        }
    }
}
