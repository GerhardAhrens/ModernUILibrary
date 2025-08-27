namespace ModernSQLiteDemo
{
    using System;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public class MenuExecute
    {
        public void FuncEmpty()
        {
            MConsole.ClearScreen();
            MConsole.Wait("Eine Taste für zurück!", ConsoleColor.Yellow);
        }
    }
}