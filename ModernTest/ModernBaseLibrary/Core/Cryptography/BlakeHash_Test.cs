//-----------------------------------------------------------------------
// <copyright file="BlakeHash_Test.cs" company="Lifeprojects.de">
//     Class: BlakeHash_Test
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>02.08.2022 13:20:12</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernTest.ModernBaseLibrary.Cryptography
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text;
    using System.Threading;

    using global::ModernBaseLibrary.Cryptography;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BlakeHash_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlakeHash_Test"/> class.
        /// </summary>
        public BlakeHash_Test()
        {
        }

        [TestMethod]
        public void HashString256()
        {
            string strText = "Gerhard";
            Encoding enc = Encoding.Default;

            byte[] pbData = enc.GetBytes(strText);

            Blake256 blake256 = new Blake256();
            byte[] pbHash256 = blake256.ComputeHash(pbData);
            string result = MemUtil.ByteArrayToHexString(pbHash256);
            Assert.IsTrue(result == "F21B9C9B0CDD63F28EBCC87DB22A246069F09E936A1777A82F1A51169DC51DEA");
        }

        [TestMethod]
        public void HashString512()
        {
            string strText = "Gerhard";
            Encoding enc = Encoding.Default;

            byte[] pbData = enc.GetBytes(strText);

            Blake512 blake = new Blake512();
            byte[] pbHash = blake.ComputeHash(pbData);
            string result = MemUtil.ByteArrayToHexString(pbHash);
            Assert.IsTrue(result == "1B7AC87353AF6A29922C375DCE425FAF695101BA27AF0FA9459D682C24D205BE6658D38D2076D9B84F7EB5B34D1C275803D64ABB528FC800E01AC7F15A519759");
        }

        [TestMethod]
        public void HashFile256()
        {
            string strFilePath = string.Empty;
            FileStream fsIn = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            Blake256 blake = new Blake256();
            byte[] pbHash = blake.ComputeHash(fsIn);
            fsIn.Close();
            string result = MemUtil.ByteArrayToHexString(pbHash);
        }

        [TestMethod]
        public void HashFile512()
        {
            string strFilePath = string.Empty;
            FileStream fsIn = new FileStream(strFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            Blake512 blake = new Blake512();
            byte[] pbHash = blake.ComputeHash(fsIn);
            fsIn.Close();
            string result = MemUtil.ByteArrayToHexString(pbHash);
        }

        [TestMethod]
        public void BlakeSelfTest()
        {
            Blake512 blake512 = new Blake512();

            byte[] pbData = new byte[1] { 0 };
            byte[] pbExpc = new byte[64] {
                0x97, 0x96, 0x15, 0x87, 0xF6, 0xD9, 0x70, 0xFA,
                0xBA, 0x6D, 0x24, 0x78, 0x04, 0x5D, 0xE6, 0xD1,
                0xFA, 0xBD, 0x09, 0xB6, 0x1A, 0xE5, 0x09, 0x32,
                0x05, 0x4D, 0x52, 0xBC, 0x29, 0xD3, 0x1B, 0xE4,
                0xFF, 0x91, 0x02, 0xB9, 0xF6, 0x9E, 0x2B, 0xBD,
                0xB8, 0x3B, 0xE1, 0x3D, 0x4B, 0x9C, 0x06, 0x09,
                0x1E, 0x5F, 0xA0, 0xB4, 0x8B, 0xD0, 0x81, 0xB6,
                0x34, 0x05, 0x8B, 0xE0, 0xEC, 0x49, 0xBE, 0xB3
            };
            byte[] pbHash = blake512.ComputeHash(pbData);
            ThrowIfNotEqual(pbHash, pbExpc, "ST-1");

            pbData = new byte[0];
            pbExpc = new byte[64] {
                0xA8, 0xCF, 0xBB, 0xD7, 0x37, 0x26, 0x06, 0x2D,
                0xF0, 0xC6, 0x86, 0x4D, 0xDA, 0x65, 0xDE, 0xFE,
                0x58, 0xEF, 0x0C, 0xC5, 0x2A, 0x56, 0x25, 0x09,
                0x0F, 0xA1, 0x76, 0x01, 0xE1, 0xEE, 0xCD, 0x1B,
                0x62, 0x8E, 0x94, 0xF3, 0x96, 0xAE, 0x40, 0x2A,
                0x00, 0xAC, 0xC9, 0xEA, 0xB7, 0x7B, 0x4D, 0x4C,
                0x2E, 0x85, 0x2A, 0xAA, 0xA2, 0x5A, 0x63, 0x6D,
                0x80, 0xAF, 0x3F, 0xC7, 0x91, 0x3E, 0xF5, 0xB8
            };
            pbHash = blake512.ComputeHash(pbData);
            ThrowIfNotEqual(pbHash, pbExpc, "ST-2");
        }

        [DataRow("", "")]
        [TestMethod]
        public void DataRowInputTest(string input, string expected)
        {
        }

        [TestMethod]
        public void ExceptionTest()
        {
            try
            {
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex.GetType() == typeof(Exception));
            }
        }

        private static void ThrowIfNotEqual(byte[] pb1, byte[] pb2, string strMsg)
        {
            if (!MemUtil.ArraysEqual(pb1, pb2)) throw new Exception(strMsg);
        }

    }

    public static class MemUtil
    {
        /// <summary>
        /// Convert a hexadecimal string to a byte array. The input string must be
        /// even (i.e. its length is a multiple of 2).
        /// </summary>
        /// <param name="strHexString">String containing hexadecimal characters.</param>
        /// <returns>Returns a byte array. Returns <c>null</c> if the string parameter
        /// was <c>null</c> or is an uneven string (i.e. if its length isn't a
        /// multiple of 2).</returns>
        /// <exception cref="System.ArgumentNullException">Thrown if <paramref name="strHexString" />
        /// is <c>null</c>.</exception>
        public static byte[] HexStringToByteArray(string strHexString)
        {
            Debug.Assert(strHexString != null); if (strHexString == null) throw new ArgumentNullException("strHexString");

            int nStrLen = strHexString.Length;
            if ((nStrLen & 1) != 0) return null; // Only even strings supported

            byte[] pb = new byte[nStrLen / 2];
            byte bt;
            char ch;

            for (int i = 0; i < nStrLen; ++i)
            {
                ch = strHexString[i];
                if ((ch == ' ') || (ch == '\t') || (ch == '\r') || (ch == '\n')) continue;

                if ((ch >= '0') && (ch <= '9'))
                    bt = (byte)(ch - '0');
                else if ((ch >= 'a') && (ch <= 'f'))
                    bt = (byte)(ch - 'a' + 10);
                else if ((ch >= 'A') && (ch <= 'F'))
                    bt = (byte)(ch - 'A' + 10);
                else bt = 0;

                bt <<= 4;
                ++i;

                ch = strHexString[i];
                if ((ch >= '0') && (ch <= '9'))
                    bt += (byte)(ch - '0');
                else if ((ch >= 'a') && (ch <= 'f'))
                    bt += (byte)(ch - 'a' + 10);
                else if ((ch >= 'A') && (ch <= 'F'))
                    bt += (byte)(ch - 'A' + 10);

                pb[i / 2] = bt;
            }

            return pb;
        }

        /// <summary>
        /// Convert a byte array to a hexadecimal string.
        /// </summary>
        /// <param name="pbArray">Input byte array.</param>
        /// <returns>Returns the hexadecimal string representing the byte
        /// array. Returns <c>null</c>, if the input byte array was <c>null</c>. Returns
        /// an empty string, if the input byte array has length 0.</returns>
        public static string ByteArrayToHexString(byte[] pbArray)
        {
            if (pbArray == null) return null;

            int nLen = pbArray.Length;
            if (nLen == 0) return string.Empty;

            StringBuilder sb = new StringBuilder();

            byte bt, btHigh, btLow;
            for (int i = 0; i < nLen; ++i)
            {
                bt = pbArray[i];
                btHigh = bt; btHigh >>= 4;
                btLow = (byte)(bt & 0x0F);

                if (btHigh >= 10) sb.Append((char)('A' + btHigh - 10));
                else sb.Append((char)('0' + btHigh));

                if (btLow >= 10) sb.Append((char)('A' + btLow - 10));
                else sb.Append((char)('0' + btLow));
            }

            return sb.ToString();
        }

        public static bool ArraysEqual(byte[] pb1, byte[] pb2)
        {
            if ((pb1 == null) || (pb2 == null)) { Debug.Assert(false); return false; }
            if (pb1.Length != pb2.Length) return false;

            for (int i = 0; i < pb1.Length; ++i)
            {
                if (pb1[i] != pb2[i]) return false;
            }

            return true;
        }
    }
}
