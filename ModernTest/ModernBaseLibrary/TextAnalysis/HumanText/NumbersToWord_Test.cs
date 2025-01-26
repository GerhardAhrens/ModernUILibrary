namespace ModernTest.ModernBaseLibrary.Text
{
    using global::ModernBaseLibrary.Text;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NumbersToWord_Test
    {
        [TestMethod]
        [DataRow(0,"nul")]
        public void NumbersToWord_0(long input, string expected)
        {
            using(NumbersToWord nw = new NumbersToWord())
            {
                string result = nw.Convert(input);
                Assert.That.StringEquals(result, expected);
            }
        }

        [TestMethod]
        [DataRow(1, "Eins")]
        public void NumbersToWord_1(long input, string expected)
        {
            using (NumbersToWord nw = new NumbersToWord())
            {
                string result = nw.Convert(input);
                Assert.That.StringEquals(result, expected);
            }
        }

        [TestMethod]
        [DataRow(10, "Zehn")]
        public void NumbersToWord_10(long input, string expected)
        {
            using (NumbersToWord nw = new NumbersToWord())
            {
                string result = nw.Convert(input);
                Assert.That.StringEquals(result, expected);
            }
        }

        [TestMethod]
        [DataRow(11, "Elf")]
        public void NumbersToWord_11(long input, string expected)
        {
            using (NumbersToWord nw = new NumbersToWord())
            {
                string result = nw.Convert(input);
                Assert.That.StringEquals(result, expected);
            }
        }

        [TestMethod]
        [DataRow(12, "Zwölf")]
        public void NumbersToWord_12(long input, string expected)
        {
            using (NumbersToWord nw = new NumbersToWord())
            {
                string result = nw.Convert(input);
                Assert.That.StringEquals(result, expected);
            }
        }

        [TestMethod]
        [DataRow(100, "Einhundert")]
        public void NumbersToWord_100(long input, string expected)
        {
            using (NumbersToWord nw = new NumbersToWord())
            {
                string result = nw.Convert(input);
                Assert.That.StringEquals(result, expected);
            }
        }

        [TestMethod]
        [DataRow(111, "Einhundertelf")]
        public void NumbersToWord_111(long input, string expected)
        {
            using (NumbersToWord nw = new NumbersToWord())
            {
                string result = nw.Convert(input);
                Assert.That.StringEquals(result, expected);
            }
        }

        [TestMethod]
        [DataRow(2020, "Zweitausendzwanzig")]
        public void NumbersToWord_2020(long input, string expected)
        {
            using (NumbersToWord nw = new NumbersToWord())
            {
                string result = nw.Convert(input);
                Assert.That.StringEquals(result, expected);
            }
        }
    }
}
