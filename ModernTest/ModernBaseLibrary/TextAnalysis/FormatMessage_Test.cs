namespace ModernTest.ModernBaseLibrary.Text
{
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;
    using global::ModernBaseLibrary.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FormatMessage_Test
    {
        [TestInitialize]
        public void TestSetUp()
        {
        }

        [TestMethod]
        public void Format_Get_Plural()
        {
            string testPlural = "Es wurden 10 Datensätze gelöscht";
            string tplStr1 = "Es [wurde/wurden] [ein/{0}] [Datensatz/Datensätze] gelöscht";
            string result = FormatMessage.Get(tplStr1,10);

            Assert.AreEqual(testPlural, result);
        }

        [TestMethod]
        public void Format_Get_Singular()
        {
            string testSingular = "Es wurde ein Datensatz gelöscht";
            string tplStr1 = "Es [wurde/wurden] [ein/{0}] [Datensatz/Datensätze] gelöscht";
            string result = FormatMessage.Get(tplStr1, 1);

            Assert.AreEqual(testSingular, result);
        }
    }
}