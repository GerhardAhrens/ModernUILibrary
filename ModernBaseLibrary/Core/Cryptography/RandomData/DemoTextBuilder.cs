//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>03.02.2023 14:00:02</date>
//
// <summary>
// Konsolen Applikation zum demonstrieren von Methoden die alle Leerzeichen in einem String entfernen
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Cryptography
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class DemoTextBuilder
    {
        public static List<string> GenerateStrings(int txtCount, int txtLength)
        {
            List<string> strings;
            var count = Convert.ToInt32(txtCount);
            var len = Convert.ToInt32(txtLength);
            strings = new List<string>(count);
            for (int i = 0; i < count; i++)
            {
                strings.Add(GenerateString(len));
            }

            return strings;
        }

        public static string GenerateString(int len)
        {
            var randomText = Path.GetRandomFileName().Replace('.', ' ');
            while ((randomText += randomText).Length < len);

            return randomText.Substring(0,len);
        }
    }
}
