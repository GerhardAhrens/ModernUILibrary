namespace System
{
    using System.Runtime.Versioning;

    using ModernConsole.Message;

    [SupportedOSPlatform("windows")]
    public static partial class MConsole
    {
        public static void Alert(string message, string title, ConsoleMessageType messageType = ConsoleMessageType.Error)
        {
            ConsoleAlert.Create(message, title, messageType);
        }

        public static void Info(string message, string title, ConsoleMessageType messageType = ConsoleMessageType.Info)
        {
            ConsoleAlert.Create(message, title, messageType);
        }
    }
}

namespace ModernConsole.Message
{
    using System.Runtime.Versioning;

    using ModernConsole.Extension;

    [SupportedOSPlatform("windows")]
    internal static class ConsoleAlert
    {
        public static void Create(string message, string title, ConsoleMessageType type = ConsoleMessageType.Info)
        {
            var stringLength = message.Length + 4; 
            var maxLength = Math.Min(stringLength, System.Console.WindowWidth);
            var wrappedMessage = WrapText(message, maxLength - 4);


            title ??= string.Empty;
            WriteLine(BuildHeader(title, type, maxLength), (ConsoleColor)type);
            foreach (var line in wrappedMessage)
            {
                Write("│", (ConsoleColor)type);
                Write($" {line.PadRight(maxLength - 4)} ");
                Write("│", (ConsoleColor)type);
            }

            var pos = Console.GetCursorPosition();
            Console.SetCursorPosition(0, pos.Top+1);
            WriteLine(BuildFooter(type, maxLength), (ConsoleColor)type);
        }

        private static string BuildHeader(string title, ConsoleMessageType type, int maxLength)
        {

            int count = (int)Math.Ceiling((decimal)((maxLength - 2 - title.Length) / 2));
            var isBalanced = count * 2 + title.Length >= (maxLength - 2);

            var str = $"┌{'─'.Repeat(count)}{title}{'─'.Repeat(!isBalanced ? count + 1 : count)}┐";

            return str;
        }

        private static string BuildFooter(ConsoleMessageType type, int maxLength)
        {
            return $"└{'─'.Repeat(maxLength - 2)}┘";
        }

        private static List<string> WrapText(string text, int maxLength)
        {
            var words = text.Split(' ');
            var lines = new List<string>();
            var currentLine = string.Empty;

            foreach (var word in words)
            {
                if ((currentLine + word).Length > maxLength)
                {
                    lines.Add(currentLine);
                    currentLine = word;
                }
                else
                {
                    currentLine += (currentLine.Length > 0 ? " " : "") + word;
                }
            }

            if (currentLine.Length > 0)
            {
                lines.Add(currentLine);
            }

            return lines;
        }

        private static void WriteLine(string message, ConsoleColor? color = null)
        {
            DoWriteLine(message, color);
        }

        private static void DoWriteLine(string message, ConsoleColor? color = null)
        {
            if (color.HasValue == true)
            {
                System.Console.ForegroundColor = color.Value;
            }

            System.Console.WriteLine(message);
            System.Console.ResetColor();
        }

        private static void Write(string message, ConsoleColor? color = null)
        {
            DoWrite(message, color);
        }

        private static void DoWrite(string message, ConsoleColor? color = null)
        {
            if (color.HasValue == true)
            {
                System.Console.ForegroundColor = color.Value;
            }

            System.Console.Write(message);

            if (color.HasValue == true)
            {
                System.Console.ResetColor();
            }
        }
    }

    public enum ConsoleMessageType : int
    {
        None = 0,
        Info = ConsoleColor.Blue,
        Success = ConsoleColor.Green,
        Warning = ConsoleColor.DarkYellow,
        Error = ConsoleColor.Red,
    }
}
