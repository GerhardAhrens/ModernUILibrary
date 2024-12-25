
using Microsoft.VisualStudio.TestTools.UnitTesting;

using ModernBaseLibrary.Core;

using ModernTest.ModernBaseLibrary;

using System;
using System.ComponentModel;
using DescriptionAttribute = System.ComponentModel.DescriptionAttribute;

namespace EasyPrototypingTest
{
    /// <summary>
    /// Zusammenfassungsbeschreibung für EnumExtension_Test
    /// </summary>
    [TestClass]
    public class EnumReadableString_Test
    {
        [TestMethod]
        public void ParseWith0()
        {
            Numbers num1 = EnumReadableString.Parse<Numbers>("Nix");
            Numbers num2 = EnumReadableString.Parse<Numbers>("0");
            Assert.IsTrue(num1 == Numbers.None);
            Assert.IsTrue(num2 == Numbers.None);
        }

        [TestMethod]
        public void ParseWith100()
        {
            Numbers num1 = EnumReadableString.Parse<Numbers>("One Hundred");
            Numbers num2 = EnumReadableString.Parse<Numbers>("one hundred", true);
            Numbers num3 = EnumReadableString.Parse<Numbers>("100");
            Assert.IsTrue(num1 == Numbers.OneHundred);
            Assert.IsTrue(num2 == Numbers.OneHundred);
            Assert.IsTrue(num3 == Numbers.OneHundred);
        }

        [TestMethod]
        public void ParseWithWrongText()
        {
            try
            {
                Numbers num1 = EnumReadableString.Parse<Numbers>("Null");
            }
            catch (ArgumentException ex)
            {
                Assert.That.StringContains(ex.Message, "Unknown value Null");
            }
        }

    }

    [DefaultValue(None)]
    public enum Numbers

    {
        [ReadableString("Nix")]
        [ReadableString("0")]
        None = 0,

        [ReadableString("One Hundred")]
        [ReadableString("100")]
        OneHundred = 100
    }
}