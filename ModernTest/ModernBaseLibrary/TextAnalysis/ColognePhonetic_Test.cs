namespace ModernTest.ModernBaseLibrary.Text
{
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ColognePhonetic_Test
    {
        [TestInitialize]
        public void TestSetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [DataRow("Ahrens", "0768")]
        [DataRow("AHRENS", "0768")]
        [DataRow("Arens", "0768")]
        [DataRow("Arenz", "0768")]
        [DataRow("Arendz", "0768")]
        [DataRow("Ahrends", "0768")]
        [DataRow("Arends", "0768")]
        [DataRow("Ares", "078")]
        [TestMethod]
        public void ColognePhonetic(string input, string expected)
        {
            ColognePhonetic result = new ColognePhonetic(input);
            Assert.IsTrue(expected == result.ToString());
        }
    }
}