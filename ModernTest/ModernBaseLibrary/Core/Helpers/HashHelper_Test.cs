namespace ModernTest.ModernBaseLibrary.Core
{
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernTest.ModernBaseLibrary;

    [TestClass]
    public class HashHelper_Test : BaseTest
    {
        [TestInitialize]
        public void SetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void GetHashCode_Params()
        {
            int hashA = HashHelper.GetHashCode("Test",10, false,null,null);
            int hashB = HashHelper.GetHashCode("Test", 10, false);
            Assert.AreEqual(hashA, hashB);
        }

        [TestMethod]
        public void GetHashCode_OfT()
        {
            int hashA = HashHelper.GetHashCode(new string[] {"Eins","Zwei", "Drei" });
            int hashB = HashHelper.GetHashCode("Eins", "Zwei", "Drei");
            Assert.AreEqual(hashA, hashB);
        }

        [TestMethod]
        public void CombineHashCode()
        {
            int hashA = 0.CombineHashCode("Eins").CombineHashCode("Zwei").CombineHashCode("Drei");
            int hashB = HashHelper.GetHashCode("Eins", "Zwei", "Drei");
            Assert.AreEqual(hashA, hashB);
        }
    }
}