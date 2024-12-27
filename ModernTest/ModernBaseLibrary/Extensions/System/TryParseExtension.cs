namespace ModernTest.ModernBaseLibrary
{
    using System;
    using System.ComponentModel;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Zusammenfassungsbeschreibung für Client_Test
    /// </summary>
    [TestClass]
    public class TryParseExtension : BaseTest
    {
        public TryParseExtension()
        {
            string language = this.GetCurrentCulture.Name;
            Assert.IsTrue(language == "de-DE");
        }

        [TestMethod]
        public void TryParseInteger()
        {
            string input = "100";
            int outValue;
            bool valueResult = input.TryParse<int>(out outValue);
            Assert.IsTrue(valueResult);
            Assert.IsTrue(outValue.ToString() == input);
        }

        [TestMethod]
        public void TryParseNullableInteger()
        {
            string input = "100";
            int? outValue;
            bool valueResult = input.TryParse<int?>(out outValue);
            Assert.IsTrue(valueResult);
            Assert.IsTrue(outValue.ToString() == input);
        }

        [TestMethod]
        public void TryParseDecimal()
        {
            string input = "100,11";
            decimal outValue;
            bool valueResult = input.TryParse<decimal>(out outValue);
            Assert.IsTrue(valueResult);
            Assert.IsTrue(outValue.ToString("#.00") == input);
        }

        [TestMethod]
        public void TryParseNullableDecimal()
        {
            string input = "100,11";
            decimal? outValue;
            bool valueResult = input.TryParse<decimal?>(out outValue);
            Assert.IsTrue(valueResult);
            Assert.IsTrue(outValue.Value.ToString("#.00") == input);
        }

        [TestMethod]
        public void TryParseDateTime()
        {
            string input = "03.05.2021";
            DateTime outValue;
            bool valueResult = input.TryParse<DateTime>(out outValue);
            Assert.IsTrue(valueResult);
            Assert.IsTrue(outValue.ToShortDateString() == input);
        }

        [TestMethod]
        public void TryParseNullableDateTime()
        {
            string input = "03.05.2021";
            DateTime? outValue;
            bool valueResult = input.TryParse<DateTime?>(out outValue);
            Assert.IsTrue(valueResult);
            Assert.IsTrue(outValue.Value.ToShortDateString() == input);
        }

        [TestMethod]
        public void TryParseEnum()
        {
            string input = "Nix";
            Coolness outValue;
            bool valueResult = input.TryParse<Coolness>(out outValue);
            Assert.IsTrue(valueResult);
            Assert.IsTrue(outValue == Coolness.None);
        }

        [DefaultValue(None)]
        public enum Coolness

        {
            [System.ComponentModel.Description("Nix")]
            None = 0,

            [System.ComponentModel.Description("Not so cool")]
            NotSoCool = 5,

            Cool,

            [System.ComponentModel.Description("Very cool")]
            VeryCool = NotSoCool + 7,

            [System.ComponentModel.Description("Super cool")]
            SuperCool
        }
    }
}