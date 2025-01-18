namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RangeParse_Test
    {
        [TestMethod]
        public void RangeParse_Explode()
        {
            List<int> output = RangeParse.Explode(5, 9);

            Assert.AreEqual(5, output.Count);
            Assert.AreEqual(5, output[0]);
            Assert.AreEqual(6, output[1]);
            Assert.AreEqual(7, output[2]);
            Assert.AreEqual(8, output[3]);
            Assert.AreEqual(9, output[4]);
        }

        [TestMethod]
        public void ExplodeSingle()
        {
            List<int> output = RangeParse.Explode(1, 1);

            Assert.AreEqual(1, output.Count);
            Assert.AreEqual(1, output[0]);
        }

        [TestMethod]
        public void InterpretSimple()
        {
            var output = RangeParse.Interpret("50");
            Assert.AreEqual(1, output.Count);
            Assert.AreEqual(50, output[0]);
        }

        [TestMethod]
        public void InterpretComplex()
        {
            var output = RangeParse.Interpret("1,5-9,14,16,17,20-24");

            Assert.AreEqual(14, output.Count);
            Assert.AreEqual(1, output[0]);
            Assert.AreEqual(5, output[1]);
            Assert.AreEqual(6, output[2]);
            Assert.AreEqual(7, output[3]);
            Assert.AreEqual(8, output[4]);
            Assert.AreEqual(9, output[5]);
            Assert.AreEqual(14, output[6]);
            Assert.AreEqual(16, output[7]);
            Assert.AreEqual(17, output[8]);
            Assert.AreEqual(20, output[9]);
            Assert.AreEqual(21, output[10]);
            Assert.AreEqual(22, output[11]);
            Assert.AreEqual(23, output[12]);
            Assert.AreEqual(24, output[13]);
        }

        [ExpectedException(typeof(FormatException))]
        [TestMethod]
        public void InterpretBad()
        {
            RangeParse.Interpret("das passt nicht");
        }
    }
}
