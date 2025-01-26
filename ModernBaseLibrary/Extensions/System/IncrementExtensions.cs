//-----------------------------------------------------------------------
// <copyright file="IncrementExtensions.cs" company="Lifeprojects.de">
//     Class: IncrementExtensions
//     Copyright © www.lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - www.lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>23.05.2022 09:52:48</date>
//
// <summary>
// Extension Class for 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System.Runtime.Versioning;
    using System;

    [SupportedOSPlatform("windows")]
    public static class IncrementExtensions
    {
        public static int Increment(this int @this, int addValue = 1)
        {
            return @this + addValue;
        }

        public static char Increment(this char @this, int addValue = 1)
        {
            return (Char)(Convert.ToUInt16(@this) + addValue);
        }

        public static string Increment(this string @this, int addValue = 1)
        {
            bool isLower = @this.IsLower();
            int index = GetColNumberFromName(@this);
            string result = GetColNameFromIndex(index + addValue);
            return isLower == true ? result.ToLower() : result;
        }

        private static string GetColNameFromIndex(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        private static int GetColNumberFromName(string columnName)
        {
            char[] characters = columnName.ToUpperInvariant().ToCharArray();
            int sum = 0;
            for (int i = 0; i < characters.Length; i++)
            {
                sum *= 26;
                sum += (characters[i] - 'A' + 1);
            }
            return sum;
        }
    }
}
