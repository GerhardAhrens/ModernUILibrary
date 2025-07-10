//-----------------------------------------------------------------------
// <copyright file="HeapsAlgorithm.cs" company="www.lifeprojects.de">
//     Class: HeapsAlgorithm
//     Copyright © www.lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - www.lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>11.12.2024 13:23:56</date>
//
// <summary>
// Klasse für Heaps Algorithm
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.Algorithm
{
    using System.Collections.Generic;

    public class HeapsAlgorithm
    {
        public static void GeneratePermutations<T>(T[] array, int size, List<T[]> result)
        {
            if (size == 1)
            {
                result.Add((T[])array.Clone());
                return;
            }

            for (int i = 0; i < size; i++)
            {
                GeneratePermutations(array, size - 1, result);

                if (size % 2 == 1)
                {
                    Swap(ref array[0], ref array[size - 1]);
                }
                else
                {
                    Swap(ref array[i], ref array[size - 1]);
                }
            }
        }

        private static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }
    }
}
