using System;
using System.Collections.Generic;
using System.ComponentModel;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ModernBaseLibrary.Core;
using ModernBaseLibrary.Extension;

namespace ModernTest.ModernBaseLibrary
{
    [TestClass]
    public class EnumTo_Test
    {
        [TestMethod]
        public void ToUpperString()
        {
            Numbers result = Numbers.OneHundred;
            string text = result.ToUpperString();
            Assert.AreEqual(text, "ONEHUNDRED");
        }

        [TestMethod]
        public void ToLowerString()
        {
            Numbers result = Numbers.OneHundred;
            string text = result.ToLowerString();
            Assert.AreEqual(text, "onehundred");
        }

        [TestMethod]
        public void ToValueAsString()
        {
            Numbers result = Numbers.OneHundred;
            string text = result.ToValueAsString();
            Assert.AreEqual(text, "100");
        }

        [TestMethod]
        public void ToInt()
        {
            Numbers result = Numbers.OneHundred;
            int value = result.ToInt();
            Assert.AreEqual(value, 100);
        }

        [TestMethod]
        public void Count()
        {
            Numbers result = Numbers.OneHundred;
            int value = result.Count();
            Assert.AreEqual(value, 5);
        }

        [TestMethod]
        public void InValuesA()
        {
            Numbers result = Numbers.None;
            bool value = result.In(Numbers.None);
            Assert.AreEqual(value, true);
        }

        [TestMethod]
        public void InValuesB()
        {
            Numbers result = Numbers.None;
            bool value = result.In(Numbers.None, Numbers.OneHundred);
            Assert.AreEqual(value, true);
        }

        [TestMethod]
        public void InValuesC()
        {
            Numbers result = Numbers.None;
            bool value = result.In(Numbers.None, Numbers.FourHundred);
            Assert.AreEqual(value, false);
        }

        [TestMethod]
        public void NotInValuesA()
        {
            Numbers result = Numbers.None;
            bool value = result.NotIn(Numbers.OneHundred, Numbers.TwoHundred);
            Assert.AreEqual(value, true);
        }

        [TestMethod]
        public void NotInValuesB()
        {
            Numbers result = Numbers.None;
            bool value = result.NotIn(Numbers.None, Numbers.TwoHundred);
            Assert.AreEqual(value, false);
        }

        [TestMethod]
        public void NotInValuesC()
        {
            Numbers result = Numbers.None;
            bool value = result.NotIn(Numbers.OneHundred);
            Assert.AreEqual(value, true);
        }

        [TestMethod]
        public void IsEnum()
        {
            Numbers result = Numbers.None;
            bool value = result.IsEnum();
            Assert.AreEqual(value, true);
        }

        [TestMethod]
        public void IsEnumFromObject()
        {
            object result = Numbers.None;
            bool value = result.IsEnum();
            Assert.AreEqual(value, true);
        }

        [TestMethod]
        public void IsEnumFromNull()
        {
            object result = null;
            bool value = result.IsEnum();
            Assert.AreEqual(value, false);
        }

        [TestMethod]
        public void EnumDescription()
        {
            Numbers result = Numbers.None;
            string value = result.ToDescription();
            Assert.AreEqual(value, "Nix");
        }

        [TestMethod]
        public void ToDictionary()
        {
            Numbers result = Numbers.None;
            Dictionary<int,string> value = result.ToDictionary();
            Assert.IsNotNull(value);
            Assert.AreEqual(value.Count, 5);
        }

        [TestMethod]
        public void GetAttributeOfType_Description()
        {
            Numbers result = Numbers.None;
            System.ComponentModel.DescriptionAttribute value = result.GetAttributeOfType<System.ComponentModel.DescriptionAttribute>();
            Assert.IsNotNull(value);
            Assert.AreEqual(value.Description, "Nix");
        }

        [TestMethod]
        public void GetAttributeOfType_EnumKey_Item0()
        {
            Numbers result = Numbers.None;
            EnumKeyAttribute value = result.GetAttributeOfType<EnumKeyAttribute>();
            Assert.IsNull(value);
        }

        [TestMethod]
        public void GetAttributeOfType_EnumKey_Item1()
        {
            Numbers result = Numbers.OneHundred;
            EnumKeyAttribute value = result.GetAttributeOfType<EnumKeyAttribute>();
            Assert.IsNotNull(value);
            Assert.AreEqual(value.Guid, new Guid("{E50B1FAF-A054-40DC-A617-A188177DA075}"));
            Assert.AreEqual(value.EnumKey, null);
        }

        [TestMethod]
        public void GetAttributeOfType_EnumKey_Item2()
        {
            Numbers result = Numbers.TwoHundred;
            EnumKeyAttribute value = result.GetAttributeOfType<EnumKeyAttribute>();
            Assert.IsNotNull(value);
            Assert.AreEqual(value.Guid, Guid.Empty);
            Assert.AreEqual(value.EnumKey, "A1");
        }

        [TestMethod]
        public void ToReadableString()
        {
            Numbers result = Numbers.None;
            var value = result.ToReadableString();
            Assert.IsNotNull(value);
            Assert.AreEqual(value, "None");
        }

        [TestMethod]
        public void ToReadableStringUpper()
        {
            Numbers result = Numbers.None;
            var value = result.ToReadableString(StringSensitive.Upper);
            Assert.IsNotNull(value);
            Assert.AreEqual(value, "NONE");
        }

        [TestMethod]
        public void ToEnumFromIntNone()
        {
            int enumValue = 0;
            Numbers result = enumValue.ToEnum<Numbers>();
            Assert.AreEqual(result, Numbers.None);
        }

        [TestMethod]
        public void ToEnumFromIntWrongValue()
        {
            int enumValue = 10;
            Numbers result = enumValue.ToEnum<Numbers>();
            Assert.IsTrue(result.ToInt() == 10);
        }

        [TestMethod]
        public void ToEnumFromStringNone()
        {
            string enumValue = "OneHundred";
            Numbers result = enumValue.ToEnum<Numbers>();
            Assert.AreEqual(result, Numbers.OneHundred);
        }

        [TestMethod]
        public void ToEnumFromStringWrongValue()
        {
            string enumValue = "Quatsch";
            Numbers result = enumValue.ToEnum<Numbers>();
            Assert.AreEqual(result, Numbers.None);
        }

        [DefaultValue(None)]
        private enum Numbers
        {
            [System.ComponentModel.Description("Nix")]
            None = 0,
            [EnumKey("{E50B1FAF-A054-40DC-A617-A188177DA075}")]
            [System.ComponentModel.Description("Einhundert")]
            OneHundred = 100,
            [EnumKey("A1", "Nummerischer Wert")]
            [System.ComponentModel.Description("Zweihundert")]
            TwoHundred = 200,
            [EnumKey("A2", "Nummerischer Wert")]
            [System.ComponentModel.Description("Dreihundert")]
            ThreeHundred = 300,
            [EnumKey("A3", "Nummerischer Wert")]
            [System.ComponentModel.Description("Vierhundert")]
            FourHundred = 400
        }
    }
}