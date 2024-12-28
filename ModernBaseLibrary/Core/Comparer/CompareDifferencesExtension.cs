//-----------------------------------------------------------------------
// <copyright file="CompareDifferencesExtension.cs" company="Lifeprojects.de">
//     Class: CompareDifferencesExtension
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>26.10.2021</date>
//
// <summary>
// Extension Class for List<CompareResult> Types
// </summary>
//-----------------------------------------------------------------------

namespace System
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using ModernBaseLibrary.Comparer;

    public static class CompareDifferencesExtension
    {
        public static bool IsNullOrEmpty<T>(this List<CompareResult> @this)
        {
            return @this == null || @this.Any() == false;
        }

        public static string ToText(this List<CompareResult> @this)
        {
            if (@this == null || @this.Any() == false)
            {
                return string.Empty;
            }
            else
            {
                StringBuilder resultSB = new StringBuilder();
                @this.ForEach(x => resultSB.AppendLine($"{x.FullName}"));

                return resultSB.ToString();
            }
        }

        public static string ToShortText(this List<CompareResult> @this)
        {
            if (@this == null || @this.Any() == false)
            {
                return string.Empty;
            }
            else
            {
                StringBuilder resultSB = new StringBuilder();
                resultSB.AppendLine($"{"Fieldname".PadRight(22)} : OldValue [<=>] NewValue");
                foreach (CompareResult item in @this)
                {
                    resultSB.AppendLine(item.ShortText);
                }

                return resultSB.ToString();
            }
        }

        public static List<string> ToList(this List<CompareResult> @this)
        {
            if (@this == null || @this.Any() == false)
            {
                return null;
            }
            else
            {
                List<string> resultSB = new List<string>();
                @this.ForEach(x => resultSB.Add($"{x.FullName}"));

                return resultSB;
            }
        }
    }
}
