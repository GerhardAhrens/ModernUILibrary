//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Lifeprojects.de">
//     Class: Program
//     Copyright © Lifeprojects.de 2025
// </copyright>
// <Template>
// 	Version 2.0.2025.0, 28.4.2025
// </Template>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>04.05.2025 19:34:00</date>
//
// <summary>
// Konsolen Applikation mit Menü
// </summary>
//-----------------------------------------------------------------------

namespace ModernConsoleTemplate
{
    /* Imports from NET Framework */
    using System;

    /* Imports from ModernUI Framework */
    using ModernConsole.Menu;

    public class Program
    {
        private static void Main(string[] args)
        {
            SmartMenu.Menu("Console Template")
                .Add("Menüpunkt 1, Obere Ebene", () => { MenuPoint1("Menüpunkt 1, Obere Ebene"); })

              .Add(SmartMenu.Menu("Menüpunkt 2, Obere Ebene")
                .Add("Menüpunkt 1, Zweite Ebene", () => { MenuPoint2("Menüpunkt 1, Zweite Ebene"); },1)
                .Add("Menüpunkt 2, Zweite Ebene", () => { MenuPoint2("Menüpunkt 2, Zweite Ebene"); },1)
                ).Show();

        }

        private static void ApplicationExit()
        {
            Environment.Exit(0);
        }

        private static void MenuPoint1(string ebene)
        {
            MConsole.Clear();

            MConsole.Alert(ebene, "Menüpunkt", ModernConsole.Message.ConsoleMessageType.Info);

            MConsole.Wait();
        }

        private static void MenuPoint2(string ebene)
        {
            MConsole.Clear();

            MConsole.Alert(ebene,"Menüpunkt",ModernConsole.Message.ConsoleMessageType.Info);

            MConsole.Wait();
        }
    }
}
