namespace ModernTest.ModernBaseLibrary.Core
{
    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernTest.ModernBaseLibrary;

    [TestClass]
    public class CompressBytes_Test
    {
        private readonly string beforeHashText = "Hallo, hier ist Gerhard";

        [TestMethod]
        public void CompressDecompressString()
        {
            byte[] compressed = CompressBytes.CompressByteArray(CompressBytes.GetBytes(beforeHashText));
            Assert.IsTrue(compressed.Length == 43);

            string result = CompressBytes.BytesToString(CompressBytes.DecompressBytesArray(compressed));
            Assert.That.StringEquals(result, beforeHashText);
        }
    }
}