//-----------------------------------------------------------------------
// <copyright file="ApplicationSettings.cs" company="company">
//     Class: ApplicationSettings
//     Copyright © company 2025
// </copyright>
//
// <author>Gerhard Ahrens - company</author>
// <email>gerhard.ahrens@company.de</email>
// <date>dd.MM.yyyy</date>
//
// <summary>
// Die Klasse liest und schreibt Benutzerbezogene Einstellungen. Die Datei für die Einstellungen wird in der Regel
// im Verzeichnis ProgramData\Anwendungsname als JSON Datei gespeichert.
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System;

    using ModernBaseLibrary.CoreBase;

    using ModernUI.MVVM.Enums;

    public class ApplicationSettings : SmartSettingsBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationSettings"/> class.
        /// </summary>
        public ApplicationSettings() : base(null,App.SHORTNAME)
        {
        }

        public string DatenbankConnection { get; set; }

        public string LastUser { get; set; }

        public DateTime LastAccess { get; set; }

        public bool ExitApplicationQuestion { get; set; }

        public bool SaveLastWindowsPosition { get; set; }

        public int SetLoggingLevel { get; set; }
    }
}
