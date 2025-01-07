//-----------------------------------------------------------------------
// <copyright file="AutoCompareOperators.cs" company="Lifeprojects.de">
//     Class: AutoCompareOperators
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>15.08.2017</date>
//
// <summary>
// Basisklasse zum erstellen eines Comparers
// </summary>
// <Website>
// http://damieng.com/blog/2005/10/11/automaticcomparisonoperatoroverloadingincsharp
// </Website>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;

    /// <summary>
    /// A base class that automatically provides all operator overloads based on your class
    /// only implementing CompareTo.
    /// </summary>
    public abstract class AutoCompareOperators : IComparable
    {
        public abstract int CompareTo(object obj);

        public static bool operator <(AutoCompareOperators obj1, AutoCompareOperators obj2)
        {
            return Compare(obj1, obj2) < 0;
        }

        public static bool operator >(AutoCompareOperators obj1, AutoCompareOperators obj2)
        {
            return Compare(obj1, obj2) > 0;
        }

        public static bool operator ==(AutoCompareOperators obj1, AutoCompareOperators obj2)
        {
            return Compare(obj1, obj2) == 0;
        }

        public static bool operator !=(AutoCompareOperators obj1, AutoCompareOperators obj2)
        {
            return Compare(obj1, obj2) != 0;
        }

        public static bool operator <=(AutoCompareOperators obj1, AutoCompareOperators obj2)
        {
            return Compare(obj1, obj2) <= 0;
        }

        public static bool operator >=(AutoCompareOperators obj1, AutoCompareOperators obj2)
        {
            return Compare(obj1, obj2) >= 0;
        }

        public static int Compare(AutoCompareOperators obj1, AutoCompareOperators obj2)
        {
            if (ReferenceEquals(obj1, obj2))
            {
                return 0;
            }

            if ((object)obj1 == null)
            {
                return -1;
            }

            if ((object)obj2 == null)
            {
                return 1;
            }

            return obj1.CompareTo(obj2);
        }

        public abstract override int GetHashCode();

        public override bool Equals(object obj)
        {
            if (!(obj is AutoCompareOperators))
            {
                return false;
            }

            return this == (AutoCompareOperators) obj;
        }
    }
}