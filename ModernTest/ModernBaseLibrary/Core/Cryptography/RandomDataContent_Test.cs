namespace ModernTest.ModernBaseLibrary.Cryptography
{
    using System.Collections.Generic;

    using global::ModernBaseLibrary.Cryptography;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RandomDataContent_Test
    {
        [TestMethod]
        public void FullText_RandomData()
        {
            string rdnText = string.Empty;
            List<string> randomTextList = new List<string>();

            using (RandomDataContent rdn = new RandomDataContent())
            {
                for (int i = 0; i < 100; i++)
                {
                    rdnText = rdn.AlphabetAndNumeric(15);
                    randomTextList.Add(rdnText);
                }
            }

            Assert.IsTrue(randomTextList.Count > 0);
        }

        [TestMethod]
        public void GetUniqueKey()
        {
            string randomText = string.Empty;
            using (RandomDataContent rdn = new RandomDataContent())
            {
                randomText = rdn.GetUniqueKey(10);
            }

            Assert.IsTrue(randomText.Length == 10);
        }

        [TestMethod]
        public void GetUniqueKeyWithSpecialChars()
        {
            string randomText = string.Empty;
            using (RandomDataContent rdn = new RandomDataContent())
            {
                randomText = rdn.GetUniqueKey(30,true);
            }

            Assert.IsTrue(randomText.Length == 30);
        }

        [TestMethod]
        public void SafeRandom_Byte()
        {
            SafeRandom rdn = new SafeRandom();
            var b = new byte[10];
            rdn.NextBytes(b);

            var str = System.Text.Encoding.Default.GetString(b);
        }

        [TestMethod]
        public void SafeRandom_String()
        {
            SafeRandom rdn = new SafeRandom();
            string result1 = rdn.NextString(5,10);
            string result2 = rdn.NextString(5, 10);
            string result3 = rdn.NextString(5, 10);
        }
    }
}