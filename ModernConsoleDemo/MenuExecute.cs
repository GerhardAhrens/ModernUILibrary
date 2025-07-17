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

    using ModernConsole.Menu;
    using ModernConsole.Message;
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

        [ExecuteMethodeHandler("MethodeClearScreenLine")]
        public void MethodeClearScreen_B(string sender, string param)
        {
            MConsole.ClearScreen();
            MConsole.WriteLine("Löschen ab Zeile 10");
            MConsole.ClearToEndOfCurrentLine(10, ConsoleColor.Green);
            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        [ExecuteMethodeHandler("MethodeClearScreenArea")]
        public void MethodeClearScreen_C(string sender, string param)
        {
            MConsole.ClearScreen();
            MConsole.WriteLine("Löschen Bereich, ab Zeile 10, Spalte 10, Breite 20, Höhe 10");
            MConsole.ClearArea(10,10,20,10, ConsoleColor.Green);
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

        [ExecuteMethodeHandler("MethodeTabellenMarkDown")]
        public void MethodeTabellen_B(string sender, string param)
        {
            MConsole.ClearScreen();

            var table = new ConsoleTable("Eins", "Zwei", "Drei");
            table.AddRow(1, 2, 3)
                 .AddRow("Eine lange Zeile", "Ja, so ist es", "oh");

            MConsole.WriteLine("\nFORMAT: MarkDown:\n");
            table.Show(ConsoleTableFormat.MarkDown);

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        [ExecuteMethodeHandler("MethodeTabellenAlternativ")]
        public void MethodeTabellen_C(string sender, string param)
        {
            MConsole.ClearScreen();

            var table = new ConsoleTable("Eins", "Zwei", "Drei");
            table.AddRow(1, 2, 3)
                 .AddRow("Eine lange Zeile", "Ja, so ist es", "oh");

            MConsole.WriteLine("\nFORMAT: MarkDown:\n");
            table.Show(ConsoleTableFormat.Alternative);

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        [ExecuteMethodeHandler("MethodeTabellenKomplex")]
        public void MethodeTabellen_D(string sender, string param)
        {
            MConsole.ClearScreen();

            var noCount = new ConsoleTable(new ConsoleTableOptions
            {
                Columns = new[] { "TotalLine", "LineOfCode", "Total Class", "Abstract Class", "Enum", "Interface" },
                EnableCount = true
            });

            int totalLine = 100000;
            int lineOfCode = 90000;
            int totalClass = 300;
            int abstractClassCount = 4;
            int enumCount = 7;
            int interfaceCount = 5;
            noCount.AddRow(totalLine.ToString("N0"), 
                lineOfCode.ToString("N0"), 
                totalClass.ToString("N0"), 
                abstractClassCount.ToString("N0"), 
                enumCount.ToString("N0"), 
                interfaceCount.ToString("N0"))
                .Show();


            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        [ExecuteMethodeHandler("MessageAlert")]
        public void Message_A(string sender, string param)
        {
            MConsole.ClearScreen();

            MConsole.Alert("Das ist eine Meldung vom Typ Info", "Typ Info", ConsoleMessageType.Info);

            MConsole.Alert("Das ist eine Meldung vom Typ Error", "Typ Error", ConsoleMessageType.Error);

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        [ExecuteMethodeHandler("InputLine")]
        public void Eingabe_A(string sender, string param)
        {
            MConsole.ClearScreen();

            string value = MConsole.InputLine("Name:", "Gerhard");
            MConsole.Alert($"Eingabewert: {value}", "Result", ConsoleMessageType.Info);

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        [ExecuteMethodeHandler("InputPassword")]
        public void Eingabe_B(string sender, string param)
        {
            MConsole.ClearScreen();

            string value = MConsole.Password("Ihr Passwort:");
            MConsole.Alert($"Eingabewert: {value}", "Result", ConsoleMessageType.Info);

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        [ExecuteMethodeHandler("AbfrageJN")]
        public void Eingabe_C(string sender, string param)
        {
            MConsole.ClearScreen();

            bool value = MConsole.Confirm("Beenden");
            MConsole.Alert($"Auswahl: {value}", "Result", ConsoleMessageType.Info);

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        [ExecuteMethodeHandler("SayOnPos")]
        public void Eingabe_D(string sender, string param)
        {
            MConsole.ClearScreen();

            MConsole.Say(5, 10, "Text an Position 5,10");
            MConsole.Say(6, 10, "Text mit Eingabfeld: ",20);

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        [ExecuteMethodeHandler("ConsoleCheckBox")]
        public void ConsoleCheckBox_A(string sender, string param)
        {
            MConsole.ClearScreen();

            string[] plugins = ["C#", "WPF", "C++", "Java"];
            var selectedItems = MConsole.Checkbox("Wähle eine Programmiersprache aus:", plugins).Select();
            for (int i = 0; i < selectedItems.Length; i++)
            {
                var plugin = selectedItems[i];
                Console.WriteLine(plugin.Option, (ConsoleColor)i);
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        [ExecuteMethodeHandler("ProgressBarGreen")]
        public void ProgressBar_A(string sender, string param)
        {
            Console.CursorVisible = false;

            MConsole.ClearScreen();
            MConsole.WriteLine();
            MConsole.ProgressTitleColor = ConsoleColor.Yellow;
            MConsole.ProgressBackColor = ConsoleColor.DarkGreen;
            MConsole.ProgressTotal = 100;
            MConsole.ProgressTitle = "Progressbar Demo";

            bool stop = false;
            for (int i = 0; i < 100; i++)
            {
                while (Console.KeyAvailable == true)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        stop = true;
                        break;
                    }
                }

                if (stop == true)
                {
                    break;
                }

                MConsole.ProgressValue = i;
                Thread.Sleep(250);
            }

            MConsole.ProgressTotal = 0;


            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        [ExecuteMethodeHandler("ProgressBarBlue")]
        public void ProgressBar_B(string sender, string param)
        {
            Console.CursorVisible = false;

            MConsole.ClearScreen();
            MConsole.WriteLine();
            MConsole.ProgressTitleColor = ConsoleColor.Blue;
            MConsole.ProgressBackColor = ConsoleColor.Blue;
            MConsole.ProgressTotal = 100;
            MConsole.ProgressTitle = "Progressbar Demo";

            bool stop = false;
            for (int i = 0; i < 100; i++)
            {
                while (Console.KeyAvailable == true)
                {
                    if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        stop = true;
                        break;
                    }
                }

                if (stop == true)
                {
                    break;
                }

                MConsole.ProgressValue = i;
                Thread.Sleep(250);
            }

            MConsole.ProgressTotal = 0;


            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        [ExecuteMethodeHandler("SpinnerAnnimation")]
        public void ProgressBar_C(string sender, string param)
        {
            Console.CursorVisible = false;

            MConsole.ClearScreen();
            MConsole.Write($"\nBeenden mit 'x'");

            using (var spinner = new ConsoleSpinner(17, 1, delay: 500))
            {
                spinner.Start();

                while (true)
                {
                    ConsoleKey key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.X)
                    {
                        spinner.Stop();
                        break;
                    }
                }
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }
    }
}