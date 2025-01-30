namespace EasyPrototypingTest
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Reflection;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using ModernBaseLibrary.Extension;

    using ModernTest.ModernBaseLibrary;

    [TestClass]
    public class ResourcesExtension_Test : BaseTest
    {
        [TestMethod]
        public void ListAllResources()
        {
            List<string> resourceNames = this.TestAssembly.GetResourceNames();
            Assert.That.CountGreaterZero<string[]>(resourceNames);
        }

        [TestMethod]
        public void GetResourcesName()
        {
            string resourceName = this.TestAssembly.GetResourceName("Test.txt");
            Assert.IsTrue(resourceName == "Test.txt");
        }

        [TestMethod]
        public void GetStringContentFromResourcesNotFound()
        {
            string resourceName = this.TestAssembly.GetResourceName("XTest.txt");
            string result = this.TestAssembly.GetStringFromResource(resourceName);
            Assert.IsTrue(string.IsNullOrEmpty(result) == true);
        }

        [TestMethod]
        public void GetStringContentFromResources()
        {
            string resourceName = this.TestAssembly.GetResourceName("Test.txt");
            string result = this.TestAssembly.GetStringFromResource(resourceName);
            Assert.IsTrue(string.IsNullOrEmpty(result) == false);
        }

        [TestMethod]
        public void GetIconContentFromResources()
        {
            string resourceName = this.TestAssembly.GetResourceName("ECv2.ico");
            Icon result = this.TestAssembly.GetIconFromResource(resourceName);
            Assert.IsTrue(result.Size.Height == 48 && result.Size.Width == 48);
        }

        [TestMethod]
        public void GetImageContentFromResources()
        {
            string resourceName = this.TestAssembly.GetResourceName("ApplicationLogo.png");
            Image result = this.TestAssembly.GetImageFromResource(resourceName);
            Assert.IsTrue(result.Size.Height == 128 && result.Size.Width == 128);
        }
    }
}