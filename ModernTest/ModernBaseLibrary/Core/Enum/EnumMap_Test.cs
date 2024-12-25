using System.ComponentModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ModernBaseLibrary.Core;

namespace ModernTest.ModernBaseLibrary
{
    [TestClass]
    public class EnumMap_Test
    {
        [TestMethod]
        public void EnumGetName()
        {
            EnumMap<ColourEnum> map = new EnumMap<ColourEnum>();
            string colourStr = map.GetName(ColourEnum.magenta);
            Assert.IsTrue(colourStr == "magenta");
        }

        [TestMethod]
        public void EnumGetValue()
        {
            EnumMap<ColourEnum> map = new EnumMap<ColourEnum>();
            int colourNum = map.GetValue(ColourEnum.magenta);
            Assert.IsTrue(colourNum == 5);
        }

        [TestMethod]
        public void GetDescriptionA()
        {
            EnumMap<ColourEnum> map = new EnumMap<ColourEnum>();
            string colourStr = map.GetDescription(ColourEnum.cyan);
            Assert.IsTrue(colourStr == "cyan");
        }

        [TestMethod]
        public void GetDescriptionB()
        {
            EnumMap<ColourEnum> map = new EnumMap<ColourEnum>();
            string colourStr = map.GetDescription(ColourEnum.magenta);
            Assert.IsTrue(colourStr == "Magenta");
        }

        [TestMethod]
        public void EnumParse()
        {
            EnumMap<ColourEnum> map = new EnumMap<ColourEnum>();
            string colourStr = map.GetName(ColourEnum.magenta);
            ColourEnum colour = map.Parse(colourStr);
            Assert.IsTrue(colour == ColourEnum.magenta);
        }

        [TestMethod]
        public void EnumTryParse()
        {
            EnumMap<ColourEnum> map = new EnumMap<ColourEnum>();
            string colourStr = map.GetName(ColourEnum.magenta);
            bool success = map.TryParse(colourStr, out ColourEnum result);
            Assert.IsTrue(success);
        }

        [DefaultValue(None)]
        private enum ColourEnum
        {
            [System.ComponentModel.Description("Nix")]
            None, 
            red, 
            green, 
            blue, 
            cyan,
            [System.ComponentModel.Description("Magenta")]
            magenta, 
            yellow, 
            black
        }
    }
}