//-----------------------------------------------------------------------
// <copyright file="SafeRandom.cs" company="Lifeprojects.de">
//     Class: SafeRandom
//     Copyright © Lifeprojects.de
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.01.2021</date>
//
// <summary>
// The Class for create random Data (for strings, numbers, bool etc.)
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Cryptography
{
    using System;
    using System.Linq;
    using System.Windows.Controls;

    using ModernBaseLibrary.Extension;

    /// <inheritdoc />
    public class SafeRandom : ISafeRandom
    {
        private readonly object lockObject;
        private readonly Random random;

        /// <inheritdoc />
        public SafeRandom(int seed)
        {
            lockObject = new object();
            random = new Random(seed);
        }

        /// <inheritdoc />
        public SafeRandom()
        {
            lockObject = new object();
            random = new Random();
        }

        /// <inheritdoc />
        public int Next()
        {
            lock (lockObject)
            {
                return random.Next();
            }
        }

        /// <inheritdoc />
        public int Next(int maxValue)
        {
            lock (lockObject)
            {
                return random.Next(maxValue);
            }
        }

        /// <inheritdoc />
        public int Next(int minValue, int maxValue)
        {
            lock (lockObject)
            {
                return random.Next(minValue, maxValue);
            }
        }

        /// <inheritdoc />
        public double NextDouble()
        {
            lock (lockObject)
            {
                return random.NextDouble();
            }
        }

        /// <inheritdoc />
        public double NextDouble(double minValue, double maxValue)
        {
            lock (lockObject)
            {
                return random.NextDouble(minValue, maxValue);
            }
        }

        /// <inheritdoc />
        public double NextDouble(double maxValue)
        {
            lock (lockObject)
            {
                return random.NextDouble(maxValue);
            }
        }

        /// <inheritdoc />
        public void NextBytes(byte[] buffer)
        {
            lock (lockObject)
            {
                random.NextBytes(buffer);
            }
        }

        public string NextString(int minValue, int maxValue, CharacterCasing casing = CharacterCasing.Normal)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            if (maxValue > chars.Length)
            {
                return string.Empty;
            }

            string result = string.Empty;
            int length = random.Next(minValue, maxValue);

            if (casing == CharacterCasing.Lower)
            {
                result = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray()).ToLower();
            }
            else if (casing == CharacterCasing.Upper)
            {
                result = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray()).ToUpper();
            }
            else
            {
                result = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            }

            return result;
        }
    }
}