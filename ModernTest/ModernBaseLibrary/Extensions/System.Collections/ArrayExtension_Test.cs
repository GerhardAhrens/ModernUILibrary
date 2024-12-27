namespace ModernTest.ModernBaseLibrary
{
    using System.Collections.Generic;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ArrayExtension_Test
    {
        #region IsNullOrEmpty
        [TestMethod]
        public void Array_IsNullOrEmpty_True()
        {
            int[] simpleArray = new int[] { };
            Assert.IsTrue(simpleArray.IsNullOrEmpty());
        }

        [TestMethod]
        public void Array_IsNullOrEmpty_False()
        {
            int[] simpleArray = new int[] { 1, 2, 3, 4 };
            Assert.IsFalse(simpleArray.IsNullOrEmpty());
        }

        [TestMethod]
        public void Array_IsNotNullOrEmpty()
        {
            int[] simpleArray = new int[] { 1, 2, 3, 4 };
            Assert.IsFalse(simpleArray.IsNullOrEmpty());
        }
        #endregion IsNullOrEmpty

        #region BlockCopy

        [TestMethod]
        public void BlockCopyTest()
        {
            string[] source = new string[15];
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = "string " + i.ToString();
            }

            string[] block = source.BlockCopy(0, 10);

            Assert.AreEqual(10, block.Length);

            for (int i = 0; i < block.Length; i++)
            {
                Assert.AreEqual(string.Format("string {0}", i), block[i]);
            }
        }

        [TestMethod]
        public void BlockCopyWithPadding()
        {
            string[] source = new string[15];
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = "string " + i.ToString();
            }

            string[] block = source.BlockCopy(10, 10, true);

            Assert.AreEqual(10, block.Length);

            for (int i = 0; i < block.Length; i++)
            {
                if (i < 5)
                    Assert.AreEqual(string.Format("string {0}", i + 10), block[i]);
                else
                    Assert.AreEqual(null, block[i]);
            }
        }

        [TestMethod]
        public void BlockCopyWithoutPadding()
        {
            string[] source = new string[15];
            for (int i = 0; i < source.Length; i++)
            {
                source[i] = "string " + i.ToString();
            }

            string[] block = source.BlockCopy(10, 10, false);

            Assert.AreEqual(5, block.Length);

            for (int i = 0; i < block.Length; i++)
            {
                if (i < 5)
                {
                    Assert.AreEqual(string.Format("string {0}", i + 10), block[i]);
                }
                else
                {
                    Assert.AreEqual(null, block[i]);
                }
            }
        }
        #endregion BlockCopy

        [TestMethod]
        public void Array_ToEnumerableOfT()
        {
            int[] simpleArray = new int[] { 1, 2, 3, 4 };
            IEnumerable<int> result = simpleArray.ToEnumerable<int>();
            Assert.IsFalse(result.IsNullOrEmpty());
        }

        [TestMethod]
        public void Array_ConvertToOfT()
        {
            int[] simpleArray = new int[] { 1, 2, 3, 4 };
            string[] result = simpleArray.ConvertTo<string>();
            Assert.IsFalse(result.IsNullOrEmpty());
        }

        [TestMethod]
        public void ClearAll()
        {
            int[] simpleArray = new int[] { 1, 2, 3, 4 };
            int[] result = simpleArray.ClearAll<int>();
            Assert.IsTrue(result.Length == 4);
            CollectionAssert.Contains(result, 0);
        }

        [TestMethod]
        public void ClearAt()
        {
            int[] simpleArray = new int[] { 1, 2, 3, 4 };
            int[] result = simpleArray.ClearAt<int>(2);
            Assert.IsTrue(result.Length == 4);
            Assert.IsTrue(result[1] == 0);
        }
    }
}
