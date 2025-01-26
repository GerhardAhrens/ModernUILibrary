//-----------------------------------------------------------------------
// <copyright file="NumbersToWord.cs" company="Lifeprojects.de">
//     Class: NumbersToWord
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.08.2020</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Text
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Versioning;
    using System.Text;

    using ModernBaseLibrary.Core;
    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    public class NumbersToWord : DisposableCoreBase
    {
        /// <summary>
        /// Konvertiert eine Zahl in Wörter
        /// </summary>
        /// <param name="number">Die Zahl die umgewandelt werden soll</param>
        /// <returns>Die Zahlen als Wörter</returns>
        public string Convert(long number, bool numberAsUpper = false)
        {
            if (number == 0)
            {
                return numberAsUpper == true ? "Nul" : "nul";
            }

            List<NumberToWordBase> numberlist = new List<NumberToWordBase>();
            NumberToWordBase num = new Hundert();
            numberlist.Add(num);
            num.Calculate(number, numberlist);

            numberlist.Reverse();
            StringBuilder sb = new StringBuilder();
            foreach (NumberToWordBase numberBase in numberlist)
            {
                sb.Append(numberBase);
            }

            if (numberAsUpper == true)
            {
                return sb.ToString().ToUpper().Trim();
            }
            else
            {
                return sb.ToString().Trim().ToPascalCase();
            }
        }
    }
}
