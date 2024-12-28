//-----------------------------------------------------------------------
// <copyright file="VersionStringComparer.cs" company="Lifeprojects.de">
//     Class: VersionStringComparer
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>04.08.2017</date>
//
// <summary>Class of VersionStringComparer Implemation</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Comparer
{
    using System.Collections.Generic;
    using System.Linq;

    public class VersionStringComparer : IComparer<string>
    {
        public int Compare(string x, string y)
        {
            if (string.IsNullOrEmpty(x) == true || string.IsNullOrEmpty(y) == true )
            {
                return 0;
            }

            if (x == y)
            {
                return 0;
            }

            if (x.Contains(" ") == true)
            {
                x = x.Split(' ')[0];
            }

            if (y.Contains(" ") == true)
            {
                y = y.Split(' ')[0];
            }

            string[] xparts = x.Split('.');
            string[] yparts = y.Split('.');

            int length = new[] { xparts.Length, yparts.Length }.Max();

            for (int i = 0; i < length; i++)
            {
                int xint;
                int yint;

                if (int.TryParse(xparts.ElementAtOrDefault(i), out xint) == false)
                {
                    xint = 0;
                }

                if (int.TryParse(yparts.ElementAtOrDefault(i), out yint) == false)
                {
                    yint = 0;
                }

                if (xint > yint)
                {
                    return 1;
                }

                if (yint > xint)
                {
                    return -1;
                }
            }

            return 0;
        }
    }
}