//-----------------------------------------------------------------------
// <copyright file="ApplicationCmdl.cs" company="Lifeprojects.de">
//     Class: ApplicationCmdl
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>04.09.2025</date>
//
// <summary>
// Die Klasse bildet ein Kommandline Inferface ab, um an die EXE von aussen Werte übergeben zu können.
// </summary>
//-----------------------------------------------------------------------

namespace ModernTemplate.Core
{
    using System.Cmdl;

    [Help($"== {App.SHORTAPPNAME} CommandLine Hilfe ==")]
    public class ApplicationCmdl
    {
        /// <summary>
        /// -u= oder -username=
        /// </summary>
        [Flag("username", "u")]
        [Help("Benutzername")]
        public string Username { get; set; }
    }
}
