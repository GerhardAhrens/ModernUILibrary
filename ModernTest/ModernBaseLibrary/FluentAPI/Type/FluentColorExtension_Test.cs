namespace ModernTest.ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Drawing;
    using System.Text;

    using global::ModernBaseLibrary.Extension;
    using global::ModernBaseLibrary.FluentAPI;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FluentColorExtension_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
        }

        public FluentColorExtension_Test()
        {
        }

        [TestMethod]
        public void ColorToInt()
        {
            /*
             * https://www.99colors.net/dot-net-colors
             */
            string hexYellow = Color.Yellow.That().ColorToHex();
            Color colorYellow = hexYellow.That().ToColorFromHexString();
            Color colorRed = "#FF0000".That().ToColorFromHexString();


            int resultInt = Color.Yellow.That().ColorToInt();
            Assert.AreEqual(resultInt, -256);

            resultInt = Color.Red.That().ColorToInt();
            Assert.AreEqual(resultInt, -65536);

            resultInt = Color.LightCoral.That().ColorToInt();
            Assert.AreEqual(resultInt, -1015680);

            resultInt = Color.White.That().ColorToInt();
            Assert.AreEqual(resultInt, -1);
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
