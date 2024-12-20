//-----------------------------------------------------------------------
// <copyright file="ChecksumBuilder.cs" company="Lifeprojects.de">
//     Class: ChecksumBuilder
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>15.01.2021</date>
//
// <summary>
//  Mit dieser Klasse können beliebige Check-Summen erstellt werden
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Cryptography
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Runtime.Versioning;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Fluent interface for mutating and calculating checksums.
    /// </summary>

    [SupportedOSPlatform("windows")]
    public class ChecksumBuilder : IChecksumBuilder
    {
        private readonly StringBuilder _buffer;

        /// <summary>
        /// Initializes an instance of <see cref="ChecksumBuilder"/>.
        /// </summary>
        public ChecksumBuilder()
        {
            _buffer = new StringBuilder();
        }

        #region Mutators

        private ChecksumBuilder AppendToBuffer(string value)
        {
            // Append value to buffer with separator
            _buffer.Append(value);
            _buffer.Append(';');

            return this;
        }

        private ChecksumBuilder AppendToBuffer(IFormattable value, string format = null)
        {
            var str = value.ToString(format, CultureInfo.InvariantCulture);
            return AppendToBuffer(str);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(string value)
        {
            value.IsArgumentNotNull(nameof(value));

            return AppendToBuffer(value);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(char value)
        {
            var str = value.ToString();
            return Mutate(str);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(IFormattable value)
        {
            value.IsArgumentNotNull(nameof(value));

            return AppendToBuffer(value);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(bool value)
        {
            var str = value ? "TRUE" : "FALSE";
            return Mutate(str);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(TimeSpan value)
        {
            var ticks = value.Ticks;
            return Mutate(ticks);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(DateTime value)
        {
            var ticks = value.ToUniversalTime().Ticks;
            return Mutate(ticks);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(DateTimeOffset value)
        {
            var ticks = value.ToUniversalTime().Ticks;
            return Mutate(ticks);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(byte[] data)
        {
            data.IsArgumentNotNull(nameof(data));

            var str = Convert.ToBase64String(data);
            return Mutate(str);
        }

        /// <inheritdoc />
        public IChecksumBuilder Mutate(Stream stream)
        {
            stream.IsArgumentNotNull(nameof(stream));

            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return Mutate(memoryStream.ToArray());
            }
        }

        #endregion

        /// <inheritdoc />
        public Checksum Calculate(HashAlgorithm algorithm, bool disposeAlgorithm = true)
        {
            algorithm.IsArgumentNotNull(nameof(algorithm));

            try
            {
                // Flush buffer and convert to bytes
                var bufferData = Encoding.Unicode.GetBytes(_buffer.ToString());
                _buffer.Clear();

                // Calculate checksum
                var checksumData = algorithm.ComputeHash(bufferData);
                return new Checksum(checksumData);
            }
            finally
            {
                if (disposeAlgorithm)
                    algorithm.Dispose();
            }
        }

        /// <inheritdoc />
        public Checksum Calculate() => Calculate(SHA256.Create());

        /// <inheritdoc />
        public override string ToString() => _buffer.ToString();
    }
}