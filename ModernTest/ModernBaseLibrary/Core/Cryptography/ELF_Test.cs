//-----------------------------------------------------------------------
// <copyright file="ELF_Test.cs" company="Lifeprojects.de">
//     Class: ELF_Test
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>01.02.2025</date>
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
    public class ELF_Test : BaseHashAlgorithmTests
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ELF_Test"/> class.
        /// </summary>
        public ELF_Test()
        {
        }

        [TestMethod]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString()
        {
            var actual = Elf32.Compute(SimpleBytesAscii);

            Assert.AreEqual((UInt32)0x025C05CE, actual);
        }

        [TestMethod]
        public void StaticDefaultSeedAndPolynomialWithShortAsciiString2()
        {
            var actual = Elf32.Compute(SimpleBytes2Ascii);

            Assert.AreEqual((UInt32)0x0A6E303E, actual);
        }

        [TestMethod]
        public void InstanceDefaultSeedAndPolynomialWith12KBinaryFile()
        {
            var hash = GetTestFileHash(Binary12KFileName, new Elf32());

            Assert.AreEqual((UInt32)0x0a8bf8f2, GetBigEndianUInt32(hash));
        }
    }
}
