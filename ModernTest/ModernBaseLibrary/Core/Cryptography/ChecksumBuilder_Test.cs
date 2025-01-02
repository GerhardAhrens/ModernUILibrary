namespace ModernTest.ModernBaseLibrary.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Threading;

    using global::ModernBaseLibrary.Cryptography;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ChecksumBuilder_Test
    {
        [TestMethod]
        public void Calculate_SameMutatorsSameOrder_Test()
        {
            var checksums = new[]
            {
                new ChecksumBuilder()
                    .Mutate("test string")
                    .Mutate(1337)
                    .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                    .Calculate()
                    .ToString(),

                new ChecksumBuilder()
                    .Mutate("test string")
                    .Mutate(1337)
                    .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                    .Calculate()
                    .ToString()
            };

            Assert.IsTrue(checksums.First() == checksums.Last());
        }

        [TestMethod]
        public void Calculate_SameMutatorsDifferentOrder_Test()
        {
            var checksums = new[]
            {
                new ChecksumBuilder()
                    .Mutate("test string")
                    .Mutate(1337)
                    .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                    .Calculate()
                    .ToString(),

                new ChecksumBuilder()
                    .Mutate(Convert.FromBase64String("WW91IGFyZSBjdXJpb3Vz"))
                    .Mutate(1337)
                    .Mutate("test string")
                    .Calculate()
                    .ToString()
            };

            Assert.IsFalse(checksums.First() == checksums.Last());
        }

        [TestMethod]
        public void Calculate_DifferentMutators_Test()
        {
            var date = DateTimeOffset.Now;

            var checksums = new[]
            {
                new ChecksumBuilder()
                    .Mutate(1)
                    .Calculate()
                    .ToString(),

                new ChecksumBuilder()
                    .Mutate(2)
                    .Calculate()
                    .ToString(),

                new ChecksumBuilder()
                    .Mutate(date)
                    .Calculate()
                    .ToString(),

                new ChecksumBuilder()
                    .Mutate(date.AddMilliseconds(1))
                    .Calculate()
                    .ToString(),

                new ChecksumBuilder()
                    .Mutate(Guid.NewGuid())
                    .Calculate()
                    .ToString(),

                new ChecksumBuilder()
                    .Mutate(Guid.NewGuid())
                    .Calculate()
                    .ToString()
            };


            CollectionAssert.AllItemsAreUnique(checksums);
        }

        [TestMethod]
        public void Calculate_CultureInvariant_Test()
        {
            var cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures | CultureTypes.SpecificCultures);
            var checksums = new List<string>();

            foreach (var culture in cultures)
            {
                Thread.CurrentThread.CurrentCulture = culture;
                checksums.Add(new ChecksumBuilder()
                    .Mutate(12345678912123)
                    .Mutate(new DateTime(1991, 04, 16, 15, 10, 02))
                    .Mutate(-232323)
                    .Mutate(-5252323.123)
                    .Calculate()
                    .ToString());
            }

            CollectionAssert.AllItemsAreNotNull(checksums);
        }

        [TestMethod]
        public void Mutate_Stream_Test()
        {
            var thisAssemblyFilePath = Assembly.GetExecutingAssembly().Location;

            using (var fileStream = File.OpenRead(thisAssemblyFilePath))
            {
                var checksum = new ChecksumBuilder()
                    .Mutate(fileStream)
                    .Calculate()
                    .ToString();

                Assert.IsFalse(string.IsNullOrEmpty(checksum));
            }
        }

        [TestMethod]
        public void Equality_Test()
        {
            var data = new byte[] { 1, 2, 3, 4, 5 };
            var checksums = new[]
            {
                new Checksum(data),
                new Checksum(data)
            };
            var checksumsToByteArrays = checksums.Select(c => c.ToByteArray()).ToArray();
            var checksumsToStrings = checksums.Select(c => c.ToString()).ToArray();
        }

        [TestMethod]
        public void ToString_Test()
        {
            ChecksumStringFormat format = ChecksumStringFormat.Hex;
            var data = new byte[] { 1, 2, 3, 4, 5 };
            var checksum = new Checksum(data);
            var str = checksum.ToString(format);

            Assert.IsFalse(string.IsNullOrEmpty(str));
        }

        [TestMethod]
        public void HashMD5FromFile_Test()
        {
            var thisAssemblyFilePath = Assembly.GetExecutingAssembly().Location;
            using (HashFile hf = new HashFile())
            {
                var aa = hf.GetMD5(thisAssemblyFilePath);
            }
        }

        [TestMethod]
        public void HashFromFile_Test()
        {
            var thisAssemblyFilePath = Assembly.GetExecutingAssembly().Location;
            using (HashFile hf = new HashFile(';'))
            {
                var resultMD5 = hf.GetChecksumMD5(thisAssemblyFilePath);
                var resultSHA256 = hf.GetChecksumSHA256(thisAssemblyFilePath);
            }
        }
    }
}