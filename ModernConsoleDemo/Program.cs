namespace ModernConsoleDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Versioning;

    using ModernConsole.CommandLine;
    using ModernConsole.Menu;
    using ModernConsole.Pattern;

    [SupportedOSPlatform("windows")]
    internal class Program
    {
        public static List<ConsoleMenuOption> menuOptions;

        private static void Main(string[] args)
        {
            SmartMenu.Menu("Consolen Features")
               .Add("ClearScreen", () => { ConsoleMenuHandler("MethodeClearScreen"); })

               .Add(SmartMenu.Menu("CommandLine")
                .Add("CommandLine - Help", () => { CmdHelp01(args); }, 1)
                .Add("CommandLine - Flag 1", () => { Flags01(args); }, 1)
                .Add("CommandLine - Flag 2", () => { Flags02(args); }, 1)
                .Add("CommandLine - Flag 3", () => { Flags03(args); }, 1)
                .Add("CommandLine - Flag 4", () => { Flags04(args); }, 1))

              .Add(SmartMenu.Menu("Tabellen")
                .Add("Tabelle, Layout Default", () => { ConsoleMenuHandler("MethodeTabellenDefault"); }, 1))

              .Add(SmartMenu.Menu("Meldungen")
                .Add("Message Alert", () => { ConsoleMenuHandler("MessageAlert"); }, 1))

              .Add(SmartMenu.Menu("Eingabe")
                .Add("Eingabe allgemein", () => { ConsoleMenuHandler("InputLine"); }, 1)
                .Add("Eingabe Passwort ", () => { ConsoleMenuHandler("InputPassword"); }, 1)
                .Add("Abfrage mit J/N ", () => { ConsoleMenuHandler("AbfrageJN"); }, 1)
            ).Show();
        }

        private static void ConsoleMenuHandler(string commandParam)
        {
            Type className = MethodBase.GetCurrentMethod().DeclaringType;
            var methodes = new MenuExecute();
            MethodInfo myMethod = ExecuteHandlerFactory.ExecuteMethod<MenuExecute>(commandParam);
            myMethod.Invoke(methodes, new object[] { className.Name, commandParam });
        }

        private static void CmdHelp01(string[] args)
        {
            string[] cmdArgs = CommandManager.CommandLineToArgs("program --help");
            CommandParser parser = new CommandParser(cmdArgs);
            FileCopyModel copyInfo = parser.Parse<FileCopyModel>();
            string[] helpText = parser.GetHelpInfo<FileCopyModel>().Split('\n');

            MConsole.ClearScreen();
            foreach (string line in helpText)
            {
                MConsole.WriteLine(line);
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        private static void Flags01(string[] args)
        {
            string[] cmdArgs = CommandManager.CommandLineToArgs("program -q=text.txt -z=neu.txt");
            CommandParser parser = new CommandParser(args.Length == 0 ? cmdArgs : args);
            FileCopyModel copyInfo = parser.Parse<FileCopyModel>();

            MConsole.ClearScreen();

            if (copyInfo.source != null)
            {
                MConsole.WriteLine(copyInfo.source);
            }

            if (copyInfo.destination != null)
            {
                MConsole.WriteLine(copyInfo.destination);
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        private static void Flags02(string[] args)
        {
            string[] cmdArgs = CommandManager.CommandLineToArgs("program -u=gerhard");
            CommandParser parser = new CommandParser(args.Length == 0 ? cmdArgs : args);
            FileCopyModel copyInfo = parser.Parse<FileCopyModel>();

            MConsole.ClearScreen();
            if (copyInfo.Username != null)
            {
                MConsole.WriteLine(copyInfo.Username);
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        private static void Flags03(string[] args)
        {
            string[] cmdArgs = CommandManager.CommandLineToArgs("program -a");
            CommandParser parser = new CommandParser(args.Length == 0 ? cmdArgs : args);
            FileCopyModel copyInfo = parser.Parse<FileCopyModel>();

            MConsole.ClearScreen();
            if (copyInfo.Actions != null)
            {
                MConsole.WriteLine(string.Join("; ", copyInfo.Actions));
            }

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }

        private static void Flags04(string[] args)
        {
            string[] cmdArgs = CommandManager.CommandLineToArgs("program -d");
            CommandParser parser = new CommandParser(args.Length == 0 ? cmdArgs : args);
            FileCopyModel copyInfo = parser.Parse<FileCopyModel>();

            MConsole.ClearScreen();
            MConsole.WriteLine(copyInfo.IsDeleted);

            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }
    }
}