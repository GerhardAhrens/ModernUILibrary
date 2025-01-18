namespace ModernTest.ModernBaseLibrary.Core
{
    using System.IO;
    using System.Text;

    using global::ModernBaseLibrary.Core.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LineTrackingStreamReader_Test : BaseTest
    {
        public string TestDirPath => TestContext.TestRunDirectory;

        public string TempDirPath => Path.Combine(TestDirPath, "Temp");

        static Stream MakeMemoryStream(string contents)
        {
            var memoryStream = new MemoryStream(contents.Length);
            var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8);
            streamWriter.Write(contents);
            streamWriter.Flush();
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

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
        public void InitializedLineAndPositionAreZero()
        {
            var stream = MakeMemoryStream("abcdefghijklmnopqrstuvwxyz");
            var tracking = new LineTrackingStreamReader(stream);

            Assert.AreEqual(0, tracking.LineNumber);
            Assert.AreEqual(0, tracking.CharacterPosition);
        }

        [TestMethod]
        public void ReadLineSetsInitialLineAndPosition()
        {
            const string expectedString = "12345";
            var stream = MakeMemoryStream(expectedString);
            var tracking = new LineTrackingStreamReader(stream);

            var actualString = tracking.ReadLine();

            Assert.AreEqual(1, tracking.LineNumber);
            Assert.AreEqual(5, tracking.CharacterPosition);
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void ReadSetsInitialLineAndPosition()
        {
            var stream = MakeMemoryStream("1");
            var tracking = new LineTrackingStreamReader(stream);
            tracking.Read();

            Assert.AreEqual(1, tracking.LineNumber);
            Assert.AreEqual(1, tracking.CharacterPosition);
        }
    }
}