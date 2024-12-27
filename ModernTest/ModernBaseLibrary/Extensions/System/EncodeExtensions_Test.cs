namespace ModernTest.ModernBaseLibrary
{
    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Zusammenfassungsbeschreibung für Client_Test
    /// </summary>
    [TestClass]
    public class EncodeExtensions_Test : BaseTest
    {
        public EncodeExtensions_Test()
        {
            string language = this.GetCurrentCulture.Name;
            Assert.IsTrue(language == "de-DE");
        }

        [TestMethod]
        public void EncodeDecaodeBase64_String()
        {
            /* https://www.c-sharpcorner.com/article/base64-extensions/ */

            string originalString = "Hello, World!";
            string encodedString = originalString.EncodeBase64();
            string decodedString = encodedString.DecodeBase64();
            Assert.IsTrue(originalString == decodedString);
        }

        [TestMethod]
        public void EncodeDecaodeBase64_Int()
        {
            int originalInt = 42;
            string encodedInt = originalInt.EncodeBase64();
            int decodedInt = encodedInt.DecodeBase64<int>();
            Assert.IsTrue(originalInt == decodedInt);
        }

        [TestMethod]
        public void EncodeDecaodeBase64_Array()
        {
            byte[] originalBytes = new byte[] { 0x10, 0x20, 0x30, 0x40 };
            string encodedBytes = originalBytes.EncodeBase64ByteArray();
            byte[] decodedBytes = encodedBytes.DecodeBase64ByteArray();
            Assert.IsTrue(originalBytes.IsEqual(decodedBytes));
        }
    }
}