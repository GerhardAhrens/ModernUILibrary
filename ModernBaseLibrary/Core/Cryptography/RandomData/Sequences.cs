//-----------------------------------------------------------------------
// <copyright file="Sequences.cs" company="Lifeprojects.de">
//     Class: Sequences
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.03.2023</date>
//
// <summary>
// Klasse zum Erstellen von Alphanummerischen Zeichenfolgen
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    public static class Sequences
    {
        private static Random random = null;

        static Sequences()
        {
            random = new Random();
        }

        /// <summary>
        /// Die Methode gibt eine Folge von geraden Zahlen zurück
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<int> Even(int start, int count)
        {
            if (start % 2 == 1)
            {
                start++;
            }

            int counter = 0;
            while (counter < count)
            {
                yield return start;
                counter++;
                start += 2;
            }
        }

        /// <summary>
        /// Die Methode gibt eine Folge von ungeraden Zahlen zurück
        /// </summary>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<int> Odd(int start, int count)
        {
            if (start % 2 == 0)
            {
                start++;
            }

            int counter = 0;
            while (counter < count)
            {
                yield return start;
                counter++;
                start += 2;
            }
        }

        /// <summary>
        /// Die Methode gibt eine Folgen von Zahlen des Typ (TResult) zurück.
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="start"></param>
        /// <param name="count"></param>
        /// <param name="step"></param>
        /// <returns></returns>
        public static IEnumerable<TResult> Numbers<TResult>(TResult start, int count, TResult step)
        {
            Type typ = typeof(TResult);
            if (typ.IsNumericType() == false)
            {
                yield return (TResult)Convert.ChangeType(0, typeof(TResult), CultureInfo.InvariantCulture);
            }

            int counter = 0;
            while (counter < count)
            {
                yield return (TResult)Convert.ChangeType(start, typeof(TResult), CultureInfo.InvariantCulture);
                counter++;
                TResult addVal = (TResult)Convert.ChangeType(step, typeof(TResult), CultureInfo.InvariantCulture);
                start = Add<TResult>(start, addVal);
            }
        }

        private static T Add<T>(T in1, T in2)
        {
            var d1 = Convert.ToDouble(in1);
            var d2 = Convert.ToDouble(in2);
            return (T)(dynamic)(d1 + d2);
        }

        private static T Sub<T>(T in1, T in2)
        {
            var d1 = Convert.ToDouble(in1);
            var d2 = Convert.ToDouble(in2);
            return (T)(dynamic)(d1 - d2);
        }
    }
}
