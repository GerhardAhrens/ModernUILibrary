//-----------------------------------------------------------------------
// <copyright file="RandomDataContent.cs" company="Lifeprojects.de">
//     Class: RandomDataContent
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>26.07.2019</date>
//
// <summary>
// The Class for create random Data (for strings, numbers, bool etc.)
// </summary>
//-----------------------------------------------------------------------

namespace ModernUIDemo.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.Versioning;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Controls;

    using ModernBaseLibrary.Core;

    using ModernUIDemo.Core;

    public class RandomDataContent : DisposableCoreBase
    {
        private Random random = null;

        public RandomDataContent()
        {
            random = new Random();
        }

        public string AlphabetAndNumeric(int length, CharacterCasing casing = CharacterCasing.Normal)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            if (length > chars.Length)
            {
                return string.Empty;
            }

            string result = string.Empty;

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

        public string Letters(int length, CharacterCasing casing = CharacterCasing.Normal)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

            if (length > chars.Length)
            {
                return string.Empty;
            }

            string result = string.Empty;

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

        public TResult Numbers<TResult>(int length)
        {
            const string chars = "0123456789";

            if (length > chars.Length)
            {
                return (TResult)Convert.ChangeType(0, typeof(TResult), CultureInfo.InvariantCulture);
            }

            string nums = new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());

            return (TResult)Convert.ChangeType(nums, typeof(TResult), CultureInfo.InvariantCulture);
        }

        public double NumbersDouble(double min, double max, int countDigits = 2)
        {
            var value = random.NextDouble() * (max - min) + min;

            if (value == 0)
            {
                value = random.NextDouble() * (max - min) + min;
            }

            return Math.Round(value, countDigits, MidpointRounding.AwayFromZero);
        }

        public decimal NumbersDecimal(decimal min, decimal max, int countDigits = 2)
        {
            decimal value = (decimal)random.NextDouble() * (max - min) + min;

            if (value == 0)
            {
                value = (decimal)random.NextDouble() * (max - min) + min;
            }

            return Math.Round(value, countDigits, MidpointRounding.AwayFromZero);
        }

        public int NumbersInt(int min, int max)
        {
            return random.Next(min, max);
        }

        public string Words(List<string> words)
        {
            int index = random.Next(words.Count);
            return words[index];
        }

        public DateTime Dates(DateTime from, DateTime to)
        {
            TimeSpan range = to - from;

            var randTimeSpan = new TimeSpan((long)(random.NextDouble() * range.Ticks));

            return from + randTimeSpan;
        }

        public DateTime Dates(DateTime from)
        {
            TimeSpan range = DateTime.Now - from;

            var randTimeSpan = new TimeSpan((long)(random.NextDouble() * range.Ticks));

            return from + randTimeSpan;
        }

        public bool Boolean()
        {
            return random.Next(0, 2) == 0;
        }

        public string GetUniqueKey(int size, bool specialChars = false)
        {
            char[] chars = null;

            if (specialChars == true)
            {
                chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!§$%&/()=?*+~#-_".ToCharArray();
            }
            else
            {
                chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            }

            byte[] data = new byte[4 * size];
            using (var crypto = RandomNumberGenerator.Create())
            {
                crypto.GetBytes(data);
            }
            StringBuilder result = new StringBuilder(size);
            for (int i = 0; i < size; i++)
            {
                var rnd = BitConverter.ToUInt32(data, i * 4);
                var idx = rnd % chars.Length;

                result.Append(chars[idx]);
            }

            return result.ToString();
        }

        [SupportedOSPlatform("windows")]
        public byte[] GetByteArray(int size)
        {
            Guard.NotGreaterThan<int>(0,size, "The Value must Greater 0");

            Random rnd = new Random();
            byte[] b = new byte[size];
            rnd.NextBytes(b);
            return b;
        }

        protected override void DisposeManagedResources()
        {
            random = null;
        }

        protected override void DisposeUnmanagedResources()
        {
        }
    }
}