namespace ModernTest.ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Text;

    using global::ModernBaseLibrary.Extension;
    using global::ModernBaseLibrary.FluentAPI;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FluentObjectExtension_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        public FluentObjectExtension_Test()
        {
        }

        [TestMethod]
        public void IsNullObject()
        {
            object input = null;
            bool result = input.That().IsNull();
        }

        [TestMethod]
        public void IsNotNullObject()
        {
            object input = "TestString";
            bool result = input.That().IsNull();
        }

        [TestMethod]
        public void GetQuoteForTyp()
        {
            object input = null;
            string result = input.That().Quote();

            object input1 = 12;
            result = input1.That().Quote();

            object input2 = "Gerhard";
            result = input2.That().Quote();

            object input3 = true;
            result = input3.That().Quote();
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
