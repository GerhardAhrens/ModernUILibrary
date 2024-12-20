//-----------------------------------------------------------------------
// <copyright file="RandomExtensions.cs" company="Lifeprojects.de">
//     Class: RandomExtensions
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.01.2021</date>
//
// <summary>Extensions Class for System.Random</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    /// <summary>
    /// Extensions for System.Random
    /// </summary>
    public static class RandomExtensions
    {
        /// <summary>
        /// Returns a random floating-point number that is greater than or equal to minValue, and less than maxValue.
        /// </summary>
        public static double NextDouble(this Random random, double minValue, double maxValue)
            => random.NextDouble() * (maxValue - minValue) + minValue;

        /// <summary>
        /// Returns a random floating-point number that is less than maxValue.
        /// </summary>
        public static double NextDouble(this Random random, double maxValue)
            => NextDouble(random, double.MinValue, maxValue);
    }
}