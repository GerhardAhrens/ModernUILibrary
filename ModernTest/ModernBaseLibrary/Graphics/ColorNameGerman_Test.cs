namespace ModernTest.ModernBaseLibrary.Graphics
{
    using global::ModernBaseLibrary.Graphics;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ColorNameGerman_Test : BaseTest
    {
        [TestMethod]
        public void ColorToName_MediaColor()
        {
            System.Windows.Media.Color co = System.Windows.Media.Colors.Red;
            string colorName = ColorNameGerman.GetGermanName(co);

            Assert.IsNotNull(colorName);
            Assert.AreEqual(colorName, "Rot");
        }

        [TestMethod]
        public void ColorToName_MediaBrush()
        {
            System.Windows.Media.Brush co = System.Windows.Media.Brushes.Red;
            string colorName = ColorNameGerman.GetGermanName(co);

            Assert.IsNotNull(colorName);
            Assert.AreEqual(colorName, "Rot");
        }
    }
}