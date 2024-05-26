//-----------------------------------------------------------------------
// <copyright file="OrdinalStringComparer.cs" company="Lifeprojects.de">
//     Class: OrdinalStringComparer
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>1.1.2016</date>
//
// <summary>Class of OrdinalStringComparer Implemation</summary>
// <example>
//  List<string> lastFileSort = files.OrderBy(x => x, new OrdinalStringComparer()).ToList();
// </example>
//-----------------------------------------------------------------------

namespace System
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Description of OrdinalStringComparer.
    /// </summary>
    [DebuggerStepThrough]
    [Serializable]
    public class OrdinalStringComparer : IComparer<string>
    {
        private readonly bool ignoreCase = true;

        public OrdinalStringComparer() : this(true)
        {
        }

        public OrdinalStringComparer(bool ignoreCase)
        {
            this.ignoreCase = ignoreCase;
        }

        public int Compare(string x, string y)
        {
            // check for null values first: a null reference is considered to be less than any reference that is not null
            if (x == null && y == null)
            {
                return 0;
            }

            if (x == null)
            {
                return -1;
            }

            if (y == null)
            {
                return 1;
            }

            StringComparison comparisonMode = this.ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;

            string[] splitX = Regex.Split(x.Replace(" ", string.Empty), "([0-9]+)");
            string[] splitY = Regex.Split(y.Replace(" ", string.Empty), "([0-9]+)");

            int comparer = 0;

            for (int i = 0; comparer == 0 && i < splitX.Length; i++)
            {
                if (splitY.Length <= i)
                {
                    comparer = 1; // x > y
                }

                int numericX = -1;
                int numericY = -1;
                if (int.TryParse(splitX[i], out numericX))
                {
                    if (int.TryParse(splitY[i], out numericY))
                    {
                        comparer = numericX - numericY;
                    }
                    else
                    {
                        comparer = 1; // x > y
                    }
                }
                else
                {
                    comparer = string.Compare(splitX[i], splitY[i], comparisonMode);
                }
            }

            return comparer;
        }
    }
}