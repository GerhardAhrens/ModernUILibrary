//-----------------------------------------------------------------------
// <copyright file="MenuExecute.cs" company="Lifeprojects.de">
//     Class: MenuExecute
//     Copyright © Lifeprojects.de GmbH 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>27.09.2018</date>
//
// <summary>
//      Die Methoden in der Klasse 'MenuExecute' durch das ConsolenMenu ausgelöst.
// </summary>
//-----------------------------------------------------------------------

namespace ModernConsoleDemo
{
    using System;
    using System.Runtime.Versioning;

    using ModernConsole.Pattern;
    using ModernConsole.Table;

    [SupportedOSPlatform("windows")]
    public class MenuExecute
    {
        [ExecuteMethodeHandler("MethodeClearScreen")]
        public void MethodeClearScreen(string sender, string param)
        {
            MConsole.ClearScreen();
            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        [ExecuteMethodeHandler("MethodeTabellenDefault")]
        public void MethodeTabellen_A(string sender, string param)
        {
            MConsole.ClearScreen();

            var table = new ConsoleTable("Eins", "Zwei", "Drei");
            table.AddRow(1, 2, 3)
                 .AddRow("Eine lange Zeile", "Ja, so ist es", "oh");

            MConsole.WriteLine("\nFORMAT: Default:\n");
            table.Show();

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }
    }
}