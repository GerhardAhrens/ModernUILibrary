//-----------------------------------------------------------------------
// <copyright file="GuidGenerator_Test.cs" company="Lifeprojects.de">
//     Class: GuidGenerator_Test
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>14.01.2023</date>
//
// <summary>
// Test Klasse für Guid Generator
// </summary>
//-----------------------------------------------------------------------


namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.Globalization;
    using System.Threading;

    using EasyPrototypingNET.Core;

    using global::ModernBaseLibrary.Core;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class GuidGenerator_Test
    {
        [TestInitialize]
        public void Initialize()
        {
            CultureInfo culture = new CultureInfo("de-DE");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GuidGenerator_Test"/> class.
        /// </summary>
        public GuidGenerator_Test()
        {
        }

        [TestMethod]
        public void Type1Check()
        {
            var expected = GuidVersion.TimeBased;
            var guid = GuidGenerator.GenerateTimeBasedGuid();

            // act
            var actual = guid.GetUuidVersion();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SanityType1Check()
        {
            // arrange
            var expected = GuidVersion.Random;
            var guid = Guid.NewGuid();

            // act
            var actual = guid.GetUuidVersion();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDateTimeUnspecified()
        {
            // arrange
            var expected = new DateTime(1980, 3, 14, 12, 23, 42, 112, DateTimeKind.Unspecified);
            var guid = GuidGenerator.GenerateTimeBasedGuid(expected);

            // act
            var actual = GuidGenerator.GetDateTime(guid).ToLocalTime();

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDateTimeLocal()
        {
            // arrange
            var expected = new DateTime(1980, 3, 14, 12, 23, 42, 112, DateTimeKind.Local);
            var guid = GuidGenerator.GenerateTimeBasedGuid(expected);

            // act
            var actual = GuidGenerator.GetLocalDateTime(guid);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDateTimeUtc()
        {
            // arrange
            var expected = new DateTime(1980, 3, 14, 12, 23, 42, 112, DateTimeKind.Utc);
            var guid = GuidGenerator.GenerateTimeBasedGuid(expected);

            // act
            var actual = GuidGenerator.GetUtcDateTime(guid);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GetDateTimeOffset()
        {
            // arrange
            var expected = new DateTimeOffset(1980, 3, 14, 12, 23, 42, 112, TimeSpan.Zero);
            var guid = GuidGenerator.GenerateTimeBasedGuid(expected);

            // act
            var actual = GuidGenerator.GetDateTimeOffset(guid);

            // assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DoesNotCreateDuplicateWhenTimeHasNotPassed()
        {
            // arrange
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;

            TimestampHelper.UtcNow = () => currentTime;  //make sure all calls will return the same time
            Guid firstGuid = GuidGenerator.GenerateTimeBasedGuid();

            // act
            Guid secondGuid = GuidGenerator.GenerateTimeBasedGuid();

            // assert
            Assert.IsTrue(firstGuid != secondGuid, "first: " + firstGuid + " second: " + secondGuid);
            //Assert.NotEqual(firstGuid,secondGuid);
        }

        [TestMethod]
        public void ClockSequenceChangesWhenTimeMovesBackward()
        {
            Func<Guid, short> getClockSequence = (guid) => {
                byte[] clockSequenceBytes = new byte[2];
                Array.Copy(guid.ToByteArray(), 8, clockSequenceBytes, 0, 2);
                return BitConverter.ToInt16(clockSequenceBytes, 0);
            };

            // arrange
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;

            TimestampHelper.UtcNow = () => currentTime;
            Guid firstGuid = GuidGenerator.GenerateTimeBasedGuid();

            // act
            TimestampHelper.UtcNow = () => currentTime.AddTicks(-1);  //make sure clock went backwards
            Guid secondGuid = GuidGenerator.GenerateTimeBasedGuid();

            // assert
            //clock sequence is not equal
            Assert.AreNotEqual(getClockSequence(firstGuid), getClockSequence(secondGuid));
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
    }
}
