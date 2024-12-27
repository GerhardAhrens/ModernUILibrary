namespace ModernTest.ModernBaseLibrary
{
    using global::ModernBaseLibrary.Cryptography;
    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringCryptExtensions_Test
    {

        [TestMethod]
        public void EncryptAndDecryptString()
        {
            string text = "Gerhard Ahrens";
            string resultCrypt = text.EncryptRSA("Test");
            Assert.IsNotNull(resultCrypt);

            text = resultCrypt.DecryptRSA("Test");
            Assert.IsNotNull(text);
        }

        [TestMethod]
        public void PasswordHashingFromString()
        {
            string originalPassword = "Gerhard Ahrens";
            byte[] salt = PasswordHashing.GenerateSalt();
            string hashedPassword = PasswordHashing.HashPassword(originalPassword, salt);

            Assert.IsNotNull(hashedPassword);
        }
    }
}