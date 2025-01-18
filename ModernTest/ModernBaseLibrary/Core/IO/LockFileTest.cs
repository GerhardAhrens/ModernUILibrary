namespace ModernTest.ModernBaseLibrary.Core
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using global::ModernBaseLibrary.Core.IO;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LockFileTests : BaseTest
    {
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
        public void LockFileInfo_TryAcquire_Test()
        {
            var lockFilePath = Path.Combine(TempDirPath, Guid.NewGuid().ToString());

            using (var lockFile = LockFile.TryAcquire(lockFilePath))
            {
                Assert.IsNotNull(lockFile);
            }
        }

        [TestMethod]
        public void LockFileInfo_TryAcquire_AlreadyAcquired_Test()
        {
            var lockFilePath = Path.Combine(TempDirPath, Guid.NewGuid().ToString());

            using (LockFile.TryAcquire(lockFilePath))
            {
                var lockFile = LockFile.TryAcquire(lockFilePath);
                Assert.IsNull(lockFile);
            }
        }

        [TestMethod]
        public void LockFileInfo_WaitAcquire_Test()
        {
            var lockFilePath = Path.Combine(TempDirPath, Guid.NewGuid().ToString());

            using (var originalLockFile = LockFile.TryAcquire(lockFilePath))
            {
                Task.Delay(TimeSpan.FromSeconds(0.5)).ContinueWith(_ => originalLockFile.Dispose());

                using (var newLockFile = LockFile.WaitAcquire(lockFilePath))
                {
                    Assert.IsNotNull(newLockFile);
                }
            }
        }

        [TestMethod]
        public void LockFileInfo_WaitAcquire_Cancellation_Test()
        {
            var lockFilePath = Path.Combine(TempDirPath, Guid.NewGuid().ToString());

            using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(0.5)))
            using (LockFile.TryAcquire(lockFilePath))
            {
                Assert.ThrowsException<OperationCanceledException>(() => LockFile.WaitAcquire(lockFilePath, cts.Token));
            }
        }
    }
}