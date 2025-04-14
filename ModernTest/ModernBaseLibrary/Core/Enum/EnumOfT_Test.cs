namespace ModernTest.ModernBaseLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using EasyPrototypingTest;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EnumOfT_Test : BaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void EnumEquals_Test()
        {
            EnumB e2 = EnumB.Eins;
            Assert.AreEqual(e2, EnumB.Eins);

            Assert.IsFalse(EnumA.Eins.Equals(EnumB.Eins));
            Assert.IsFalse(EnumA.Zwei.EqualEnum(EnumB.Eins));
            Assert.IsTrue(EnumA.Zwei.EqualEnum(EnumB.Zwei));

            Dictionary<int, string> namedEnums = EnumA.Eins.EnumNamedValues<EnumB>();
        }

        [TestMethod]
        public void EnumOfT_Equals_Test()
        {
            Assert.AreEqual(EnumOfT<EnumB>.Count,4);
            Assert.AreEqual(EnumOfT<EnumB>.EnumTypeName,"EnumB");
            Assert.AreEqual(EnumOfT<EnumB>.GetName(EnumB.Eins),"Eins");
            Assert.AreEqual(EnumOfT<EnumB>.GetValue(EnumB.Eins),1);

            EnumB enum1 = EnumB.Eins;
            EnumB enum2 = EnumB.Eins;
            Assert.AreEqual(EnumOfT<EnumB>.Equals(enum1, enum2),true);

            EnumB enum3 = EnumB.Eins;
            EnumB enum4 = EnumB.Drei;
            Assert.AreEqual(Enum<EnumB>.Equals(enum3, enum4),false);

            Assert.AreEqual(EnumOfT<EnumB>.Equals(EnumA.Zwei, EnumB.Zwei), false);

            Assert.AreEqual(EnumOfT<EnumB>.EqualEnum(EnumA.Zwei, EnumB.Zwei), true);
        }

        [TestMethod]
        public void EnumOfT_ToDictionary_Test()
        {
            var result7 = EnumOfT<EnumB>.ToDictionary();
            Assert.AreEqual(result7.Count, 4);
        }

        [TestMethod]
        public void EnumOfT_ToDescription_Test()
        {
            string desc = EnumOfT<EnumB>.ToDescription(EnumB.Eins);
            Assert.AreEqual(desc, "Eins-1");
        }

        [TestMethod]
        public void EnumOfT_In_Test()
        {
            EnumB noneEnum = EnumB.None;
            Assert.AreEqual(EnumOfT<EnumB>.In(noneEnum, EnumB.None, EnumB.Drei),true);
        }

        [TestMethod]
        public void EnumOfT_NotIn_Test()
        {
            EnumB noneEnum = EnumB.None;
            Assert.AreEqual(EnumOfT<EnumB>.NotIn(noneEnum, EnumB.Zwei, EnumB.Drei), true);
        }
    }

    public enum EnumA : int
    {
        Eins = 1,
        Zwei = 2,
    }

    public enum EnumB : int
    {
        [System.ComponentModel.Description("Nix")]
        None = 0,
        [System.ComponentModel.Description("Eins-1")]
        Eins = EnumA.Eins,
        [System.ComponentModel.Description("Zwei-2")]
        Zwei = EnumA.Zwei,
        [System.ComponentModel.Description("Drei-3")]
        Drei = 3,
    }

    static class ExtensionsForTest
    {
        public static bool EqualEnum(this Enum @this, Enum equal)
        {
            List<Enum> thisList = Enum.GetValues(@this.GetType()).Cast<Enum>().ToList();
            string thisName = Enum.GetName(@this.GetType(), @this);

            List<Enum> equalList = Enum.GetValues(equal.GetType()).Cast<Enum>().ToList();
            string equalName = Enum.GetName(equal.GetType(), equal);

            bool result = (thisName == equalName) && thisList.Any(a => a.ToString() == thisName) == equalList.Any(a => a.ToString() == equalName);
            return result;
        }

        public static string EnumTypeName(this Enum @this)
        {
            return @this.GetType().Name;
        }

        public static Dictionary<int, string> EnumNamedValues<T>(this Enum @this)
        {
            var result = new Dictionary<int, string>();
            var values = System.Enum.GetValues(typeof(T));

            foreach (int item in values)
            {
                result.Add(item, System.Enum.GetName(typeof(T), item));
            }

            return result;
        }
    }

}