namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.IO;

    using global::ModernBaseLibrary.Core.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ActionTextWriter_Test : BaseTest
    {
        private const string SampleString = "Einige Bytes wandern versehentlich";

        public string TestDirPath => TestContext.TestRunDirectory;

        public string TempDirPath => Path.Combine(TestDirPath, "Temp");

        [TestInitialize]
        public void SetUp()
        {
            Directory.CreateDirectory(TempDirPath);
        }

        [TestCleanup]
        public void TearDown()
        {
            if (Directory.Exists(TempDirPath))
            {
                Directory.Delete(TempDirPath, true);
            }
        }

        [TestMethod]
        public void WriteCharArrayGivenBufferAndEndRangePerformsActionWithPartialValue()
        {
            var startOffset = SampleString.Length / 2;
            var expected = SampleString.Substring(startOffset);

            var actual = string.Empty;
            new ActionTextWriter(value => actual = value).Write(SampleString.ToCharArray(), startOffset, expected.Length);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WriteCharArrayGivenBufferAndFullRangePerformsActionWithFullValue()
        {
            var actual = string.Empty;
            new ActionTextWriter(value => actual = value).Write(SampleString.ToCharArray(), 0, SampleString.ToCharArray().Length);

            Assert.AreEqual(SampleString, actual);
        }

        [TestMethod]
        public void WriteCharArrayGivenBufferAndMidRangePerformsActionWithPartialValue()
        {
            var quarterLength = SampleString.Length / 4;
            var expected = SampleString.Substring(quarterLength, quarterLength);

            var actual = string.Empty;
            new ActionTextWriter(value => actual = value).Write(SampleString.ToCharArray(), quarterLength, quarterLength);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WriteCharArrayGivenBufferAndStartRangePerformsActionWithPartialValue()
        {
            var partialLength = SampleString.Length / 2;
            var expected = SampleString.Substring(0, partialLength);

            var actual = string.Empty;
            new ActionTextWriter(value => actual = value).Write(SampleString.ToCharArray(), 0, partialLength);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WriteCharArrayGivenLengthBeyondBoundaryThrowsArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ActionTextWriter(k => k += k).Write(SampleString.ToCharArray(), 0, 500));
        }

        [TestMethod]
        public void WriteCharArrayGivenNegativeIndexThrowsArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ActionTextWriter(k => k += k).Write(SampleString.ToCharArray(), -1, 5));
        }

        [TestMethod]
        public void WriteCharArrayGivenNegativeLengthThrowsArgumentOutOfRangeException()
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new ActionTextWriter(k => k += k).Write(SampleString.ToCharArray(), 0, -5));
        }

        [TestMethod]
        public void WriteCharArrayGivenNullArrayThrowsArgumentNullException()
        {
            Assert.ThrowsException<ArgumentNullException>(() => new ActionTextWriter(k => k += k).Write(null, 0, 0));
        }

        [TestMethod]
        public void WriteLineGivenNullStringPerformsAction()
        {
            var actual = String.Empty;
            new ActionTextWriter(value => actual = value).WriteLine((string)null);

            Assert.AreEqual(Environment.NewLine, actual);
        }

        [TestMethod]
        public void WriteLineGivenStringPerformsAction()
        {
            const string original = "A simple line";
            var actual = String.Empty;
            new ActionTextWriter(value => actual += value).WriteLine(original);

            Assert.AreEqual(original + Environment.NewLine, actual);
        }

        [TestMethod]
        public void WriteGivenNullStringPerformsAction()
        {
            var actual = String.Empty;
            new ActionTextWriter(value => actual = value).Write((string)null);

            Assert.IsNull(actual);
        }

        [TestMethod]
        public void WriteGivenStringPerformsAction()
        {
            const string expected = "ABC 123";
            var actual = String.Empty;
            new ActionTextWriter(value => actual = value).Write(expected);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void WriteGivenStringPerformsActionManyTimes()
        {
            string[] originals = { "First", "Second", "Third" };
            var expected = string.Join(Environment.NewLine, originals);
            var actual = string.Empty;
            var actionTextWriter = new ActionTextWriter(value => actual += value);
            foreach (var original in originals)
                actionTextWriter.WriteLine(original);

            Assert.AreEqual(expected + Environment.NewLine, actual);
        }
    }
}