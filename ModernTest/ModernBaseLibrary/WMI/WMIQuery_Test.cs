namespace ModernTest.ModernBaseLibrary.WMI
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Core;
    using global::ModernBaseLibrary.WMI;
    using global::ModernBaseLibrary.WMI.WMIClass;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class WMIQuery_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public WMIQuery_Test()
        {
        }

        [TestMethod]
        public void GetWMI_Query()
        {
            OperationResult<Dictionary<string, WMIContentResult>> result = null;
            WMIQueryTyp query = WMIQueryTyp.Win32_DesktopMonitor;

            using (WMIQuery q = new WMIQuery(query))
            {
                result = q.Get();
            }

            Dictionary<string, WMIContentResult> values = result.Result;
            Assert.IsNotNull(values);
            Assert.IsTrue(values.Count > 0);
        }

        [TestMethod]
        public void GetWMI_QueryEntity()
        {
            OperationResult<Win32_DesktopMonitor> result = null;
            WMIQueryTyp query = WMIQueryTyp.Win32_DesktopMonitor;

            using (WMIQuery q = new WMIQuery(query))
            {
                result = q.Get<Win32_DesktopMonitor>();
            }

            Win32_DesktopMonitor wmiEntity = result.Result;
            Assert.IsNotNull(wmiEntity);
            Assert.IsTrue(string.IsNullOrEmpty(wmiEntity.Caption) == false);
        }

        [TestMethod]
        public void GetWMI_Win32_Battery()
        {
            OperationResult<Win32_Battery> result = null;
            WMIQueryTyp query = WMIQueryTyp.Win32_Battery;

            using (WMIQuery q = new WMIQuery(query))
            {
                result = q.Get<Win32_Battery>();
            }

            Win32_Battery wmiEntity = result.Result;
            Assert.IsNotNull(wmiEntity);
            Assert.IsTrue(string.IsNullOrEmpty(wmiEntity.Caption) == false);
        }

        [TestMethod]
        public void GetWMI_Win32_CurrentProbe()
        {
            OperationResult<Win32_CurrentProbe> result = null;
            WMIQueryTyp query = WMIQueryTyp.Win32_CurrentProbe;

            using (WMIQuery q = new WMIQuery(query))
            {
                result = q.Get<Win32_CurrentProbe>();
            }

            Win32_CurrentProbe wmiEntity = result.Result;
            Assert.IsNotNull(wmiEntity);
            Assert.IsTrue(string.IsNullOrEmpty(wmiEntity.Caption) == false);
        }

        [TestMethod]
        public void GetWMI_Win32_VoltageProbe()
        {
            OperationResult<Win32_VoltageProbe> result = null;
            WMIQueryTyp query = WMIQueryTyp.Win32_VoltageProbe;

            using (WMIQuery q = new WMIQuery(query))
            {
                result = q.Get<Win32_VoltageProbe>();
            }

            Win32_VoltageProbe wmiEntity = result.Result;
            Assert.IsNotNull(wmiEntity);
            Assert.IsTrue(string.IsNullOrEmpty(wmiEntity.Caption) == false);
        }

        [TestMethod]
        public void GetWMI_NamespacesForWin32()
        {
            List<string> result = new List<string>();
            using (WMIQuery q = new WMIQuery())
            {
                result = q.GetClassNamesWithinWmiNamespace();
            }

        }

        [TestMethod]
        public void GetWMI_NamespacesForWMI()
        {
            List<string> result = new List<string>();
            using (WMIQuery q = new WMIQuery())
            {
                result = q.GetClassNamesWithinWmiNamespace("root\\wmi");
            }

        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void ExceptionTest()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }
    }
}
