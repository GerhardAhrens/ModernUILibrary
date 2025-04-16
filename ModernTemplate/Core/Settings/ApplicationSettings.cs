//-----------------------------------------------------------------------
// <copyright file="ApplicationSettings.cs" company="Lifeprojects.de">
//     Class: ApplicationSettings
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>gerhard.ahrens@lifeprojects.de</email>
// <date>16.04.2025</date>
//
// <summary>
// Die Klasse liest und schreibt Benutzerbezogene Einstellungen
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

        public string LastUser { get; set; }

        public DateTime LastAccess { get; set; }

        public bool ExitApplicationQuestion { get; set; } = true;

        public bool SaveLastWindowsPosition { get; set; } = false;

        public bool IsLogging { get; set; } = false;

        public RunEnvironments RunEnvironment { get; set; } = 0;
    }
}
