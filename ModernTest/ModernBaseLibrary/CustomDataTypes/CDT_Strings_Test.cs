namespace ModernTest.ModernBaseLibrary.CustomDataTypes
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CDT_Strings_Test
    {
        [TestInitialize]
        public void TestSetUp()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        [TestMethod]
        public void TextValue_CreateTypeA()
        {
            Strings text = new Strings("Teststring");
            Assert.IsTrue(text == "Teststring");
        }

        [TestMethod]
        public void TextValue_CreateTypeB()
        {
            Strings text = "Teststring";
            Assert.IsTrue(text == "Teststring");
        }

        [TestMethod]
        public void TextValue_Length()
        {
            Strings text = "Teststring";
            Assert.IsTrue(text.Length == 10);
        }

        [TestMethod]
        public void TextValue_IsNullOrEmpty()
        {
            Strings text = "Teststring";
            Assert.IsTrue(text.IsNullOrEmpty == false);
        }

        [TestMethod]
        public void TextValue_IsNull()
        {
            Strings text = null;
            Assert.IsTrue(text.IsNull == true);
        }

        [TestMethod]
        public void TextValue_CreateTypeFromVar()
        {
            var textVar = new Strings("TestString");
            Strings text = textVar;
            Assert.IsTrue(text == "TestString");
        }

        [TestMethod]
        public void TextValue_GetType()
        {
            Strings text = new Strings("TestString");
            Type typ = text.GetType();
            Assert.IsTrue(typ == typeof(Strings));
        }

        [TestMethod]
        public void TextValue_ConvertType()
        {
            Strings tv = new Strings("TestString");
            Type typTv = tv.GetType();
            Assert.IsTrue(typTv == typeof(Strings));

            string text = tv;
            Type typText = text.GetType();
            Assert.IsTrue(typText == typeof(string));
        }

        [TestMethod]
        public void Extension_Reverse()
        {
            Strings text = new Strings("TestString").Reverse();
            Assert.IsTrue(text == "gnirtStseT");
        }

        [TestMethod]
        [DataRow("Hase", true)]
        [DataRow("Bär", false)]
        public void ExtensionA_EqualsAny(string input, bool expected)
        {
            string[] stringList = new string[] { "Hase", "Igel" };
            bool result = new Strings(input).EqualsAny(stringList);
            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [DataRow("Hase", true)]
        [DataRow("Bär", false)]
        public void ExtensionB_EqualsAny(string input, bool expected)
        {
            string[] stringList = new string[] { "Hase", "Igel" };
            Strings text = input;
            bool result = text.EqualsAny(stringList);
            Assert.AreEqual(result, expected);
        }

        [TestMethod]
        [DataRow("1*2*3*4*5*6*7*8*9*0", "1_2_3_4_5_6_7_8_9_0")]
        public void Extension_Replace(string input,string expected)
        {
            Dictionary<string, string> replaceList = new Dictionary<string, string>()
            {
                { "*", "_" }
            };

            Strings text = input;
            string result = text.Replace(replaceList);
            Assert.IsTrue(result == expected);
        }

        #region To-Methodes
        [TestMethod]
        public void Extension_ToTokenListDefault()
        {
            string testText = "Der Hase und der Igel\nmachen ein Rennen auf dem Acker.";
            List<string> token = new Strings(testText).ToTokenList();
            Assert.IsTrue(token.Count == 2);
        }

        [TestMethod]
        public void Extension_ToTokenList()
        {
            char[] separators = new char[] { ' ', '\n', '.' };
            string testText = "Der Hase und der Igel\nmachen ein Rennen auf dem Acker.";
            List<string> token = new Strings(testText).ToTokenList(separators);
            Assert.IsTrue(token.Count == 11);
        }

        [TestMethod]
        [DataRow("Ja", true)]
        [DataRow("nein", false)]
        [DataRow("1", true)]
        [DataRow("0", false)]
        [DataRow("y", true)]
        [DataRow("n", false)]
        public void Extension_ToBool(string input, bool expected)
        {
            bool result = new Strings(input).ToBool();
            Assert.IsTrue(result ==expected);
        }

        [TestMethod]
        [DataRow("Ja", "ja")]
        public void Extension_ToLower(string input, string expected)
        {
            Strings result = new Strings(input).ToLower();
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        [DataRow("Ja", "JA")]
        public void Extension_ToUpper(string input, string expected)
        {
            Strings result = new Strings(input).ToUpper();
            Assert.IsTrue(result == expected);
        }

        #endregion To-Methodes

        #region Crypt und Hash
        [TestMethod]
        public void Extension_ToMD5()
        {
            string testText = "Der Hase und der Igel\nmachen ein Rennen auf dem Acker.";
            string hash = new Strings(testText).ToMD5();
            Assert.IsTrue(hash == "40f1e9902c692c51dcd77faf87a361e4");
        }

        [TestMethod]
        public void Extension_ToCRC32()
        {
            string testText = "Der Hase und der Igel\nmachen ein Rennen auf dem Acker.";
            string hash = new Strings(testText).ToCRC32();
            Assert.IsTrue(hash == "2639884300");
        }

        [TestMethod]
        public void Extension_ToCRC64()
        {
            string testText = "Der Hase und der Igel\nmachen ein Rennen auf dem Acker.";
            string hash = new Strings(testText).ToCRC64();
            Assert.IsTrue(hash == "4250386367443603049");
        }
        #endregion Crypt und Hash
    }
}
