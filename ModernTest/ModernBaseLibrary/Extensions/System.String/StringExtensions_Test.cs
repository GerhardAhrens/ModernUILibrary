namespace ModernTest.ModernBaseLibrary
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using global::ModernBaseLibrary.Extension;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StringExtensions_Test
    {
        [TestMethod]
        public void StringInListTrue()
        {
            string[] strings = new string[] { "a", "b", "c", "z", "" };
            bool value = "b".In(strings);

            Assert.IsTrue(value);
        }

        [TestMethod]
        public void StringInListFalse()
        {
            string[] strings = new string[] { "a", "b", "c", "z", "" };
            bool value = "x".NotIn(strings);

            Assert.IsFalse(value);
        }

        [TestMethod]
        public void StringNotInListTrue()
        {
            string[] strings = new string[] { "a", "b", "c", "z", "" };
            bool value = "X".NotIn(strings);

            Assert.IsTrue(value);
        }

        [TestMethod]
        public void GetAs()
        {
            string resultString = "Gerhard".GetAs<string>();
            Assert.AreEqual(resultString,"Gerhard");
            Assert.AreEqual(resultString.GetType(), typeof(string));

            DateTime resultDateTime = "01.01.2023".GetAs<DateTime>();
            Assert.AreEqual(resultDateTime, new DateTime(2023,1,1));
            Assert.AreEqual(resultDateTime.GetType(), typeof(DateTime));

            bool resultBool = "true".GetAs<bool>();
            Assert.AreEqual(resultBool, true);
            Assert.AreEqual(resultBool.GetType(), typeof(bool));

            double resultDouble = "99.88".GetAs<double>();
            Assert.AreEqual(resultDouble, 99.88);
            Assert.AreEqual(resultDouble.GetType(), typeof(double));

            decimal resultDecimal = "99.88".GetAs<decimal>();
            Assert.AreEqual(resultDecimal, 99.88M);
            Assert.AreEqual(resultDecimal.GetType(), typeof(decimal));

            int resultInt = "100".GetAs<int>();
            Assert.AreEqual(resultInt, 100);
            Assert.AreEqual(resultInt.GetType(), typeof(int));

            Status resultEnum1 = "None".GetAs<Status>();
            Status resultEnum2 = "1".GetAs<Status>();
            Assert.AreEqual(resultEnum1.GetType(), typeof(Status));
            Assert.AreEqual(resultEnum2.GetType(), typeof(Status));
        }

        public bool IsList(object o)
        {
            return o is IList &&
               o.GetType().IsGenericType &&
               o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));
        }

        public bool IsDictionary(object o)
        {
            return o is IDictionary &&
               o.GetType().IsGenericType &&
               o.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(Dictionary<,>));
        }

        public bool IsIEnumerable(object o)
        {
            return o is IEnumerable; 
        }

        public Type GetEnumerableType(Type type)
        {
            foreach (Type intType in type.GetInterfaces())
            {
                if (intType.IsGenericType
                    && intType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    return intType.GetGenericArguments()[0];
                }
            }
            return null;
        }

        public static bool IsIEnumerableOfT(Type type)
        {
            return type.GetInterfaces().Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        [TestMethod]
        [DataRow("developer", "Gerhard")]
        public void SplitOneSeparatorFirst(string input, string expected)
        {
            string result = input.SplitEx('.').First();
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        [DataRow("developer", "Ahrens")]
        public void SplitOneSeparatorLast(string input, string expected)
        {
            string result = input.SplitEx('.').Last();
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        [DataRow("GerhardAhrens", "GerhardAhrens")]
        public void SplitOneSeparatorEmpty(string input, string expected)
        {
            string result = input.SplitEx('.')?.First();
            Assert.IsTrue(result == null);
        }

        [TestMethod]
        [DataRow("Gerhard und Ahrens", "gerhardUndAhrens")]
        public void ToCamelCase(string input, string expected)
        {
            string result = input.ToCamelCase();
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        [DataRow("Gerhard und Ahrens", "GerhardUndAhrens")]
        public void ToPascalCase(string input, string expected)
        {
            string result = input.ToPascalCase();
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        [DataRow("Gerhard und Ahrens", "Gerhard Und Ahrens")]
        public void ToProperCase(string input, string expected)
        {
            string result = input.ToProperCase();
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        [DataRow("Gerhard", "Gerard")]
        public void RemoveChar_WithH(string input, string expected)
        {
            string result = input.RemoveChar('h');
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        [DataRow(" G e r h a r d", "Gerhard")]
        public void RemoveChar_WithSpace(string input, string expected)
        {
            string result = input.RemoveChar(' ');
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        [DataRow("Gerhard Ahrens", "GA")]
        public void ToInitials(string input, string expected)
        {
            string result = input.ToInitials();
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        [DataRow("Gerhard-22- Ahrens I.", "GA")]
        public void ToInitials_WithNumber(string input, string expected)
        {
            string result = input.ToInitials();
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        [DataRow(null, "??")]
        public void ToInitials_EmptyOrNull(string input, string expected)
        {
            string result = input.ToInitials();
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        [DataRow(" G e r h a r d", "Gerhard")]
        public void RemoveWhitespace(string input, string expected)
        {
            string result = input.RemoveWhitespace();
            Assert.IsTrue(result == expected);
        }

        [TestMethod]
        [DataRow("", "Gerhard")]
        public void RemoveWhitespace_StringEmpty(string input, string expected)
        {
            string result = input.RemoveWhitespace();
            Assert.IsFalse(result == expected);
        }

        [TestMethod]
        [DataRow(null, "Gerhard")]
        public void RemoveWhitespace_Null(string input, string expected)
        {
            string result = input.RemoveWhitespace();
            Assert.IsFalse(result == expected);
        }

        [TestMethod]
        public void CapitalizeManyWords()
        {
            List<string> texts = new List<string>() { "developer", "gerhard-ahrens", "gerhard ahrens" };
            string result = texts[0].Capitalize();
            Assert.IsTrue(result == "developer");

            string result1 = texts[1].Capitalize();
            Assert.IsTrue(result1 == "Gerhard-Ahrens");

            string result2 = texts[2].Capitalize();
            Assert.IsTrue(result2 == "Gerhard Ahrens");
        }

        [TestMethod]
        public void Capitalize()
        {
            string text = "gerhard";
            string result = text.Capitalize();
            Assert.IsTrue(result == "Gerhard");
        }

        [TestMethod]
        public void CapitalizeWithNullValue()
        {
            try
            {
                string text = null;
                string result = text.Capitalize();
                Assert.IsTrue(result == "Gerhard");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is ArgumentNullException);
            }
        }

        [TestMethod]
        public void CheckPalindrome()
        {
            string text = "Radar";
            bool result = text.IsPalindrome();
            Assert.IsTrue(result == true);

            string text1 = "Gerhard";
            bool result1 = text1.IsPalindrome();
            Assert.IsTrue(result1 == false);
        }

        [TestMethod]
        public void Reverse()
        {
            string text = "Gerhard";
            string result = text.Reverse();
            Assert.IsTrue(result == "drahreG");
        }

        [TestMethod]
        public void CountCharTokenUpper()
        {
            string text = "Hallo [Gerhard] and [Beate]";
            int result = text.CountToken('L');
        }

        [TestMethod]
        public void CountCharToken()
        {
            string text = "Hallo [Gerhard] and [Beate]";
            int result = text.CountToken('l');
        }

        [TestMethod]
        public void GetUniqueCharFromString()
        {
            string text = "Hallo [Gerhard] and [Beate]";
            StringBuilder duplicateChar = text.UniqueCharFromString();
            string values = duplicateChar.ToString();
            Assert.IsTrue(string.IsNullOrEmpty(values) == false);
            Assert.IsTrue(values == "Halo [Gerhd]nBt");
        }

        [TestMethod]
        public void GetDuplicateCharacter()
        {
            string text = "Hallo [Gerhard] and [Beate]";
            StringBuilder duplicateChar = text.DuplicateCharacter();
            string values = duplicateChar.ToString();
            Assert.IsTrue(string.IsNullOrEmpty(values) == false);
            Assert.IsTrue(values == "lar ad [eae]");
        }

        [TestMethod]
        public void TruncateNoAddText()
        {
            string text = "developer@lifeprojects.de";

            string result = text.Truncate(9);

            Assert.IsTrue(result.Length == 9);
            Assert.IsTrue(result == "developer");
        }

        [TestMethod]
        public void TruncateAddText()
        {
            string text = "developer@lifeprojects.de";

            string result = text.Truncate(8, "...");

            Assert.IsTrue(result.Length == 8);
            Assert.IsTrue(result == "devel...");
        }

        [TestMethod]
        public void TruncateLeft()
        {
            string text = "developer@lifeprojects.de";

            string result = text.TruncateLeft(13);

            Assert.IsTrue(result.Length == 13);
            Assert.IsTrue(result == "developer@lifeprojects.de");
        }

        [TestMethod]
        public void TruncateLeftAddText()
        {
            string text = "developer@lifeprojects.de";

            string result = text.TruncateLeft(13, "...");

            Assert.IsTrue(result.Length == 13);
            Assert.IsTrue(result == "...rojects.de");
        }

        [DataRow("gerhardahrensptade", "gerhardahr-ensptade")]
        [TestMethod]
        public void SplitByLenght(string input, string expected)
        {
            string result = input.SplitByLenght(10,"-");
            Assert.That.StringEquals(result, expected);
        }

        [TestMethod]
        public void Left_Test()
        {
            // getting the name part of an email address
            string sample = "sample@domain.com";

            string namePart = sample.Left(6);

            Assert.AreEqual("sample", namePart);
        }

        [TestMethod]
        public void LeftOf1_Test()
        {
            // getting the name part of an email address
            string sample = "sample@domain.com";

            string namePart = sample.LeftOf('@');

            Assert.AreEqual("sample", namePart);
        }

        [TestMethod]
        public void LeftOf2_Test()
        {
            // getting a file filename without the last extension
            string sample = "sample.tar.gz";

            // skip the first match
            string fileName = sample.LeftOf('.', 1);

            Assert.AreEqual("sample.tar", fileName);
        }

        [TestMethod]
        public void LeftOfLast_Test()
        {
            // getting a filename without the last extension
            string sample = "sample.tar.gz";

            string fileName = sample.LeftOfLast('.');

            Assert.AreEqual("sample.tar", fileName);
        }

        [TestMethod]
        public void Right_Test()
        {
            // getting the domain part of an email address
            string sample = "sample@domain.com";

            string namePart = sample.Right(10);

            Assert.AreEqual("domain.com", namePart);
        }

        [TestMethod]
        public void RightOf1()
        {
            // getting the domain part of an email address
            string sample = "sample@domain.com";

            string namePart = sample.RightOf('@');

            Assert.AreEqual("domain.com", namePart);
        }

        [TestMethod]
        public void RightOf2()
        {
            // getting the full extension 
            string sample = "sample.tar.gz";

            // skip the first match
            string fileName = sample.RightOf('.', 1);

            Assert.AreEqual("tar.gz", fileName);
        }

        [TestMethod]
        public void RightOfLast()
        {
            // getting the last extension of a fileName
            string sample = "sample.tar.gz";

            string fileName = sample.RightOfLast('.');

            Assert.AreEqual("gz", fileName);
        }

        private enum Status : int
        {
            None = 0,
            Active = 1
        }
    }
}