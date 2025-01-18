namespace ModernTest.ModernBaseLibrary.FluentAPI
{
    using System;

    using global::ModernBaseLibrary.Extension;
    using global::ModernBaseLibrary.FluentAPI;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FluentDateTime_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        public FluentDateTime_Test()
        {
        }

        [DataRow("29.4.2021", "1.4.2021","30.4.2021", true)]
        [DataRow("1.5.2021", "1.4.2021", "30.4.2021", false)]
        [DataRow("1.3.2021", "1.4.2021", "30.4.2021", false)]
        [TestMethod]
        public void IsBetween(string input, string min, string max, bool expected)
        {
            DateTime current = input.ToDateTime();
            DateTime minDT = min.ToDateTime();
            DateTime maxDT = max.ToDateTime();

            bool result = current.That().IsBetween(minDT, maxDT);
            Assert.IsTrue(result == expected);
        }

        [DataRow("29.4.2021", "1.4.2021", "30.4.2021", RangeCheck.Inside)]
        [DataRow("1.4.2021", "1.4.2021", "1.4.2021", RangeCheck.Border)]
        [DataRow("1.5.2021", "1.4.2021", "30.4.2021", RangeCheck.Outside)]
        [DataRow("1.3.2021", "1.4.2021", "30.4.2021", RangeCheck.Outside)]
        [TestMethod]
        public void IsInRange(string input, string min, string max, RangeCheck expected)
        {
            DateTime current = input.ToDateTime();
            DateTime minDT = min.ToDateTime();
            DateTime maxDT = max.ToDateTime();

            RangeCheck result = (RangeCheck)current.That().IsInRange(minDT, maxDT);
            Assert.IsTrue(result == expected);
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