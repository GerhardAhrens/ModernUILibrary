//-----------------------------------------------------------------------
// <copyright file="StringFluentExtension.cs" company="Lifeprojects.de">
//     Class: StringFluentExtension
//     Copyright © Lifeprojects.de 2021
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>28.04.2021</date>
//
// <summary>
// Die Klasse stellt Methoden für eine Typ zur Verfügung. Für diesen Typ
// wird dan die dazu passende Klasse aufgerufen.
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.FluentAPI
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.IO;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    [DebuggerNonUserCode]
    [DebuggerStepThrough]
    public static class FluentTypExtension
    {
        /// <summary>
        /// Die Methode stellt Extenstion Methodes für den Typ Object zur Verfügung
        /// </summary>
        /// <param name="this">Value vom Typ</param>
        /// <returns></returns>
        public static FluentObject That(this object @this)
        {
            return new FluentObject(@this);
        }

        /// <summary>
        /// Die Methode stellt Extenstion Methodes für den Typ String zur Verfügung
        /// </summary>
        /// <param name="this">Value vom Typ</param>
        /// <returns></returns>
        public static FluentString That(this string @this)
        {
            return new FluentString(@this);
        }

        /// <summary>
        /// Die Methode stellt Extenstion Methodes für den Typ Int zur Verfügung
        /// </summary>
        /// <param name="this">Value vom Typ</param>
        /// <returns></returns>
        public static FluentInt That(this int @this)
        {
            return new FluentInt(@this);
        }

        /// <summary>
        /// Die Methode stellt Extenstion Methodes für den Typ Bool zur Verfügung
        /// </summary>
        /// <param name="this">Value vom Typ</param>
        /// <returns></returns>
        public static FluentBool That(this bool @this)
        {
            return new FluentBool(@this);
        }

        /// <summary>
        /// Die Methode stellt Extenstion Methodes für den Typ DateTime zur Verfügung
        /// </summary>
        /// <param name="this">Value vom Typ</param>
        /// <returns></returns>
        public static FluentDateTime That(this DateTime @this)
        {
            return new FluentDateTime(@this);
        }

        /// <summary>
        /// Die Methode stellt Extenstion Methodes für den Typ Color zur Verfügung
        /// </summary>
        /// <param name="this">Value vom Typ</param>
        /// <returns></returns>
        public static FluentColor That(this Color @this)
        {
            return new FluentColor(@this);
        }

        /// <summary>
        /// Die Methode stellt Extenstion Methodes für den Typ DirectoryInfo zur Verfügung
        /// </summary>
        /// <param name="this">The this.</param>
        /// <returns></returns>
        public static FluentDirectoryInfo That(this DirectoryInfo @this)
        {
            return new FluentDirectoryInfo(@this);
        }
    }
}
