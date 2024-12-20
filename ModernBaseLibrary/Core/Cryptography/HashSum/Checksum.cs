//-----------------------------------------------------------------------
// <copyright file="Checksum.cs" company="Lifeprojects.de">
//     Class: Checksum
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
    using System.Linq;
    using System.Runtime.Versioning;


    /// <summary>
    /// Represents a calculated checksum.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public partial class Checksum : IEquatable<Checksum>
    {
        private readonly byte[] _data;

        /// <summary>
        /// Initializes checksum with given data.
        /// Use <see cref="ChecksumBuilder" /> to generate checksums.
        /// </summary>
        public Checksum(byte[] data)
        {
            _data = data.IsArgumentNotNull(nameof(data));
        }

        /// <summary>
        /// Returns checksum as a raw array of bytes.
        /// </summary>
        public byte[] ToByteArray()
        {
            // Make a copy so it won't get mutated
            return _data.ToArray();
        }

        /// <summary>
        /// Returns checksum formatted as string.
        /// </summary>
        public string ToString(ChecksumStringFormat format)
        {
            if (format == ChecksumStringFormat.Base64)
                return Convert.ToBase64String(_data);
            if (format == ChecksumStringFormat.Hex)
                return BitConverter.ToString(_data).Replace("-", "");

            throw new ArgumentOutOfRangeException(nameof(format));
        }

        /// <summary>
        /// Returns checksum formatted as base64 string.
        /// </summary>
        public override string ToString() => ToString(ChecksumStringFormat.Base64);

        /// <inheritdoc />
        public bool Equals(Checksum other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return _data.SequenceEqual(other._data);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals((Checksum) obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }
    }

    public partial class Checksum
    {
        /// <summary />
        public static bool operator ==(Checksum a, Checksum b)
        {
            if (a is null)
                return b is null;

            return a.Equals(b);
        }

        /// <summary />
        public static bool operator !=(Checksum a, Checksum b) => !(a == b);
    }
}