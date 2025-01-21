namespace ModernTest.ModernBaseLibrary.CustomDataTypes
{
    using System;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CDT_EMail_Test
    {
        [DataRow("developer@lifeprojects.de")]
        [TestMethod]
        public void EMailAddress1_CreateType(string input)
        {
            EMail adr = new EMail(input);
            string result = adr.ToString();
            Assert.IsTrue(result == input);
            Assert.IsTrue(result == adr.MailAddress);
        }

        [TestMethod]
        public void EMailAddress2_CreateType()
        {
            EMail adr = "developer@lifeprojects.de";
            string result = adr.ToString();
        }

        [TestMethod]
        public void EMailAddressWithName_CreateType()
        {
            EMail adrName = new EMail("developer@lifeprojects.de", "Gerhard Ahrens");
        }

        [TestMethod]
        public void EMailIsEqual()
        {
            EMail email1 = new EMail("developer@lifeprojects.de", "Gerhard Ahrens");
            EMail email2 = new EMail("developer@lifeprojects.de", "Gerhard Ahrens");
            Assert.AreEqual<EMail>(email1, email2);
        }

        [TestMethod]
        public void EMailIsNotEqual_A()
        {
            EMail email1 = new EMail("developer@lifeprojects.de", "Gerhard Ahrens");
            EMail email2 = new EMail("developer@lifeprojects.com", "Gerhard Ahrens");
            Assert.AreNotEqual<EMail>(email1, email2);
        }

        [TestMethod]
        public void EMailIsNotEqual_B()
        {
            EMail email1 = new EMail("developer@lifeprojects.de", "Gerhard Ahrens");
            EMail email2 = new EMail("developer@lifeprojects.de", "Gerhard_Ahrens");
            Assert.AreNotEqual<EMail>(email1, email2);
        }
    }
}
