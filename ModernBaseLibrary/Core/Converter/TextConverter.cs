//-----------------------------------------------------------------------
// <copyright file="TextConverter.cs" company="Lifeprojects.de">
//     Class: TextConverter
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>12.01.2023</date>
//
// <summary>
// Die Klasse ermöglicht es über eine angegebene Func<T,T> Strings bzw. Texte zu konvertieren.
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Converter
{
    using System;

    public class TextConverter
    {
        private readonly Func<string, string> convertion;

        public TextConverter(Func<string, string> convertion)
        {
            this.convertion = convertion;
        }

        public string ConvertText(string inputText)
        {
            return this.convertion(inputText);
        }
    }
}
