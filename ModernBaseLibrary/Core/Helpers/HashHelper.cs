//-----------------------------------------------------------------------
// <copyright file="HashHelper.cs" company="Lifeprojects.de">
//     Class: HashHelper
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>22.01.2025 10:45:53</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System.Collections.Generic;

    public static class HashHelper
    {
        public static int GetHashCode(params object[] arg)
        {
            unchecked
            {
                int result = 0;
                foreach (object o in arg)
                {
                    if (o != null)
                    {
                        result = result * 31 + o.GetHashCode();
                    }
                }

                return result;
            }
        }

        public static int GetHashCode<T1, T2>(T1 arg1, T2 arg2)
        {
            unchecked
            {
                int hash = 0;
                hash = 31 * hash + arg1.GetHashCode();
                hash = 31 * hash + arg2.GetHashCode();
                return hash;
            }
        }

        public static int GetHashCode<T1, T2, T3>(T1 arg1, T2 arg2, T3 arg3)
        {
            unchecked
            {
                int hash = 0;
                hash = 31 * hash + arg1.GetHashCode();
                hash = 31 * hash + arg2.GetHashCode();
                hash = 31 * hash + arg3.GetHashCode();
                return hash;
            }
        }

        public static int GetHashCode<T1, T2, T3, T4>(T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            unchecked
            {
                int hash = 0;
                hash = 31 * hash + arg1.GetHashCode();
                hash = 31 * hash + arg2.GetHashCode();
                hash = 31 * hash + arg3.GetHashCode();
                hash = 31 * hash + arg4.GetHashCode();
                return hash;
            }
        }

        public static int GetHashCode<T>(T[] list)
        {
            unchecked
            {
                int hash = 0;
                foreach (var item in list)
                {
                    hash = 31 * hash + item.GetHashCode();
                }

                return hash;
            }
        }

        public static int GetHashCode<T>(IEnumerable<T> list)
        {
            unchecked
            {
                int hash = 0;
                foreach (var item in list)
                {
                    hash = 31 * hash + item.GetHashCode();
                }
                return hash;
            }
        }

        /// <summary>
        /// Gets a hashcode for a collection for that the order of items 
        /// does not matter.
        /// So {1, 2, 3} and {3, 2, 1} will get same hash code.
        /// </summary>
        public static int GetHashCodeForOrderNoMatterCollection<T>(IEnumerable<T> list)
        {
            unchecked
            {
                int hash = 0;
                int count = 0;
                foreach (var item in list)
                {
                    hash += item.GetHashCode();
                    count++;
                }
                return 31 * hash + count.GetHashCode();
            }
        }

        /// <summary>
        /// Alternative way to get a hashcode is to use a fluent 
        /// interface like this:<br />
        /// return 0.CombineHashCode(field1).CombineHashCode(field2).CombineHashCode(field3);
        /// </summary>
        public static int CombineHashCode<T>(this int hashCode, T arg)
        {
            unchecked
            {
                return 31 * hashCode + arg.GetHashCode();
            }
        }
    }
}
