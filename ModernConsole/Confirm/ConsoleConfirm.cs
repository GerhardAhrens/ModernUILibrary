namespace System
{
    using System.Runtime.Versioning;

    public partial class MConsole
    {
        public static bool Confirm(string message)
        {
            return new ConsoleConfirm(message).GetAnswer();
        }

        public static bool Confirm(string message, bool defaultValue)
        {
            return new ConsoleConfirm(message, defaultValue).GetAnswer();
        }
    }

    [SupportedOSPlatform("windows")]
    internal class ConsoleConfirm(string message)
    {
        private bool? defaultValue;

        internal ConsoleConfirm(string Message, bool defaultValue) : this(Message)
        {

            this.defaultValue = defaultValue;
        }

        internal bool GetAnswer()
        {
            var confirmActions = "j/n";
            if (this.defaultValue.HasValue == true)
            {
                confirmActions = this.defaultValue.Value ? "J/n" : "j/N";
            }

            Console.Write($"{message} [{confirmActions}]: ");
            var result = Console.ReadKey();
            Console.WriteLine();
            if (result.Key == ConsoleKey.Enter)
            {
                if (this.defaultValue.HasValue == true)
                {
                    return this.defaultValue.Value;
                }
                else
                {
                    MConsole.WriteErrorLine("Es kann nur j oder n gewählt werden.");
                    return GetAnswer();
                }
            }

            return result.Key == ConsoleKey.J;
        }
    }
}
