namespace ModernTest.ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Drawing;
    using System.Text;

    using global::ModernBaseLibrary.Extension;
    using global::ModernBaseLibrary.FluentAPI;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FluentInt_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        public FluentInt_Test()
        {
        }

        [TestMethod]
        new public void ToString()
        {
            int input = 100;
            string result = input.That().ToString();
            Assert.IsTrue(input.ToString() == result);

            result = input.That().ToString("C");
            Assert.IsTrue(input.ToString("C") == result);
        }

        [DataRow(100, 90,101, true)]
        [DataRow(100, 100, 100, false)]
        [DataRow(100, 91, 99, false)]
        [TestMethod]
        public void IsBetween(int input, int min, int max, bool expected)
        {
            bool result = input.That().IsBetween(min,max);
            Assert.IsTrue(result == expected);
        }


        [DataRow(100, 110, 90, RangeCheck.Inside)]
        [DataRow(100, 100, 100, RangeCheck.Border)]
        [DataRow(100, 80, 99, RangeCheck.Outside)]
        [DataRow(100, 101, 110, RangeCheck.Outside)]
        [TestMethod]
        public void IsInRange(int input, int min, int max, RangeCheck expected)
        {
            RangeCheck result = input.IsInRange(min, max);
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        public void IntToColor()
        {
            int input = -265;
            Color resultColor = input.That().ToColor();
        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void ExceptionTest()
        {
        }

        [TestMethod]
        public void TryExceptionTest()
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