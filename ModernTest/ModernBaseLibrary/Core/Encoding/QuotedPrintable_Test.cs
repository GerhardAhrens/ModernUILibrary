namespace ModernTest.ModernBaseLibrary
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class QuotedPrintable_Test
    {
        [TestMethod]
        public void EncodeGivenAsciiReturnsAsciiString()
        {
            var expected = "!ThisIsABasicTest^";

            var actual = new QuotedPrintable().Encode(Encoding.ASCII.GetBytes(expected));

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeEncodesTabsAndSpacesByDefault()
        {
            var text = "!This Is A Basic\tTest^";
            var expected = text.Replace(" ", "=20").Replace("\t", "=09");

            var actual = new QuotedPrintable().Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeDoesNotEncodeSpacesIfSpecified()
        {
            var text = "!This Is\tA Basic Test^";
            var expected = text.Replace("\t", "=09");

            var actual = new QuotedPrintable(DoNotEncode.Space).Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeDoesNotEncodeTabsIfSpecified()
        {
            var text = "!This Is A Basic\tTest^";
            var expected = text.Replace(" ", "=20");

            var actual = new QuotedPrintable(DoNotEncode.Tab).Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeDoesNotCreateLinesOver76CharsInLength()
        {
            var text = String.Join("-", Enumerable.Repeat("A quick brown fox ", 100));

            var actual = new QuotedPrintable(DoNotEncode.Space).Encode(Encoding.ASCII.GetBytes(text));

            using (var reader = new StringReader(actual))
                Assert.IsTrue(reader.ReadLine().Length <= 76);
        }

        [TestMethod]
        public void EncodeDoesNotEncodeTabsOrSpacesIfSpecified()
        {
            var text = "!This Is A Basic\tTest^";
            var expected = text.Replace(" ", "=20").Replace("\t", "=09");

            var actual = new QuotedPrintable(DoNotEncode.Tab & DoNotEncode.Space).Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodePutsEqualsAtEndIfLastCharacterIsUnencodedSpace()
        {
            var text = "Line Ends With ";

            var actual = new QuotedPrintable(DoNotEncode.Space).Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(text + "=", actual);
        }

        [TestMethod]
        public void EncodeDoesNotPutsEqualsAtEndIfLastCharacterEncoded()
        {
            var text = "Line Ends With ";
            var expected = text.Replace(" ", "=20");

            var actual = new QuotedPrintable().Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodePutsEqualsAtEndIfLastCharacterIsUnencodedTab()
        {
            var text = "LineEndsWith\t";

            var actual = new QuotedPrintable(DoNotEncode.Tab).Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(text + "=", actual);
        }

        [TestMethod]
        public void EncodeEncodesEquals()
        {
            var text = "This=Is=A=Test";
            var expected = text.Replace("=", "=3D");

            var actual = new QuotedPrintable().Encode(Encoding.ASCII.GetBytes(text));

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EncodeGivenBinaryEncodesBinary()
        {
            var unencoded = new byte[] { 0x10, 0x44, 0x00, 0xA3, 0xFF, 0xDE, 0x4E };
            var expected = "=10D=00=A3=FF=DEN";

            var actual = new QuotedPrintable().Encode(unencoded);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DecodeGivenAsciiReturnsAsciiBytes()
        {
            var text = "This is a cool test";
            var expected = Encoding.ASCII.GetBytes(text);

            var actual = new QuotedPrintable().Decode(text);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DecodeGivenMixReturnsDecodedBytes()
        {
            var text = "^Aa =20\t=09!";
            var expected = new byte[] { 0x5E, 0x41, 0x61, 0x20, 0x20, 0x09, 0x09, 0x21 };

            var actual = new QuotedPrintable().Decode(text);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DecodeIgnoresTrailingEquals()
        {
            var text = " A =";
            var expected = new byte[] { 0x20, 0x41, 0x20 };

            var actual = new QuotedPrintable().Decode(text);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DecodeThrowsArgumentOutOfRangeIfQuotedEndsBeforeCompletion()
        {
            try
            {
                _ = new QuotedPrintable().Decode("Dam=2");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentOutOfRangeException));
            }
        }

        [TestMethod]
        public void DecodeThrowsArgumentOutOfRangeIfNotHex()
        {
            try
            {
                _ = new QuotedPrintable().Decode("Dam=4G");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(ArgumentOutOfRangeException));
            }
        }
    }
}