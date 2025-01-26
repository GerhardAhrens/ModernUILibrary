namespace ModernTest.ModernBaseLibrary.Network
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Network;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PhysicalMAC_Test
    {
        [TestInitialize]
        public void TestSetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void PhysicalMAC_Singel()
        {
            string mac = PhysicalMAC.Get();
        }

        [TestMethod]
        public void PhysicalMAC_List()
        {
            List<string> macList = PhysicalMAC.GetList();
        }

        [TestMethod]
        public void NetworkInterfaceInfo()
        {
            Dictionary<string,string> macList = PhysicalMAC.NetworkInterfaceInfo();
        }
    }
}