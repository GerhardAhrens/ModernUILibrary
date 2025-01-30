namespace ModernTest.ModernBaseLibrary.Graphics
{
    using System.Windows.Media;

    using EasyPrototypingTest;

    using global::ModernBaseLibrary.Core;
    using global::ModernBaseLibrary.Extension;
    using global::ModernBaseLibrary.Graphics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GraphicsCompare_Test : BaseTest
    {
        [TestMethod]
        public void CompareImage()
        {
            string resourceName = this.TestAssembly.GetResourceName("ApplicationLogo.png");
            byte[] result1 = AssemblyResource.GetResourceContent<byte[]>(resourceName, AssemblyLocation.ExecutingAssembly);
            byte[] result2 = AssemblyResource.GetResourceContent<byte[]>(resourceName, AssemblyLocation.ExecutingAssembly);

            Assert.That.AreEquals(result1, result2);

            ImageSource resultB1 = AssemblyResource.GetResourceContent<ImageSource>(resourceName, AssemblyLocation.ExecutingAssembly);
            ImageSource resultB2 = AssemblyResource.GetResourceContent<ImageSource>(resourceName, AssemblyLocation.ExecutingAssembly);
            Assert.That.AreEquals(resultB1, resultB2,string.Empty);
        }
    }
}