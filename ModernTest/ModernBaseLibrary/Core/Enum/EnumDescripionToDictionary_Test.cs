using System;
using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ModernBaseLibrary.Core;

namespace EasyPrototypingTest
{
    [TestClass]
    public class EnumDescripionToDictionary_Test
    {
        [TestMethod]
        public void EnumWithEnumHelper()
        {
            Dictionary<ColorEnum,string> dictResult = new EnumDescripionToDictionary<ColorEnum>();
            Assert.IsNotNull(dictResult);
            Assert.IsTrue(dictResult.Count == 8);
        }


        [DefaultValue(None)]
        private enum ColorEnum
        {
            [System.ComponentModel.Description("Nix")]
            None,
            red,
            green,
            [System.ComponentModel.Description("Blau")]
            blue,
            cyan,
            [System.ComponentModel.Description("Magenta")]
            magenta,
            yellow,
            black
        }
    }
}