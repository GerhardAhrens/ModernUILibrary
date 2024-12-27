namespace ModernTest.ModernBaseLibrary
{
    using System.Security;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CryptographyExtension_Test
    {
        [DataRow("Gerhard","Gerhard")]
        [TestMethod]
        public void ToSecureString(string input, string expected)
        {
            SecureString toSecure = input.ToSecureString();

            string fromSecure = toSecure.SecureStringToString();
            Assert.That.StringEquals(fromSecure, expected);
        }

        [DataRow("Gerhard", "afd1a3329ed527f0c9d890c4fbf4d448")]
        [TestMethod]
        public void ToMD5ForLower(string input, string expected)
        {
            string toHash = input.ToMD5();
            Assert.That.StringEquals(toHash, expected);
        }

        [DataRow("Gerhard", "AFD1A3329ED527F0C9D890C4FBF4D448")]
        [TestMethod]
        public void ToMD5ForUpper(string input, string expected)
        {
            string toHash = input.ToMD5(true);
            Assert.That.StringEquals(toHash, expected);
        }

        [DataRow("Gerhard", "afd1a3329ed527f0c9d890c4fbf4d448")]
        [TestMethod]
        public void VerifyMD5Hash(string input, string expected)
        {
            bool result = input.VerifyMD5Hash(expected);
            Assert.IsTrue(result);
        }
    }
}