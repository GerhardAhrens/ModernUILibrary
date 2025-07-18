//-----------------------------------------------------------------------
// <copyright file="Line.cs" company="Lifeprojects.de">
//     Class: Line
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>18.07.2025 08:07:11</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace System
{
    public partial class MConsole
    {
        public static void Line(char lineChar = '-')
        {
            new ConsoleLine(lineChar).Show();
        }

        public static void Line(string message, char lineChar = '-')
        {
            new ConsoleLine(message,lineChar).Show();
        }

        public static void Line(string message, ConsoleColor foregroundColor, char lineChar = '-')
        {
            new ConsoleLine(message, foregroundColor,lineChar).Show();
        }

        public static void Line(int row, int col, int length,char lineChar = '-', ConsoleColor foregroundColor = ConsoleColor.White)
        {
            new ConsoleLine(row,col,length, lineChar, foregroundColor).ShowWithPos();
        }
    }

    internal class ConsoleLine()
    {
        internal ConsoleLine(char lineChar) : this()
        {
            this.LineChar = lineChar;
        }

        internal ConsoleLine(string message, char lineChar) : this()
        {
            this.Message = message;
            this.LineChar = lineChar;
        }

        internal ConsoleLine(string message, ConsoleColor foregroundColor,char lineChar) : this()
        {
            this.Message = message;
            this.LineChar = lineChar;
            this.ForegroundColor = foregroundColor;
        }

        internal ConsoleLine(int row, int col, int length, char lineChar, ConsoleColor foregroundColor) : this()
        {
            this.Row = row;
            this.Col = col;
            this.Length = length;
            this.LineChar = lineChar;
            this.ForegroundColor = foregroundColor;
        }


        private string Message { get; set; } = string.Empty;
        private char LineChar { get; set; }
        private int Row { get; set; } = 0;
        private int Col { get; set; } = 0;
        private int Length { get; set; } = 0;
        private ConsoleColor ForegroundColor { get; set; } =  ConsoleColor.White;

        public void Show()
        {
            string line = string.Empty;

            if (string.IsNullOrEmpty(this.Message) == true)
            {
                line = new string(this.LineChar, Console.BufferWidth);
            }
            else
            {
                double length = Console.BufferWidth - this.Message.Length;
                string lineL = new string(this.LineChar, Convert.ToInt32(Math.Round(length/2, 0,MidpointRounding.ToZero)));
                line = $"{lineL}{this.Message}{lineL}";
            }

            var saveForegroundColor = Console.ForegroundColor;
            Console.ForegroundColor = ForegroundColor;

            Console.CursorVisible = false;
            Console.Write(line);
            Console.Write('\n');

            Console.ForegroundColor = saveForegroundColor;
        }

        public void ShowWithPos()
        {
            var savePos = Console.GetCursorPosition();
            var saveForegroundColor = Console.ForegroundColor;
            Console.CursorVisible = false;

            string line = new string(this.LineChar, Length);
            Console.SetCursorPosition(this.Col, this.Row);
            Console.ForegroundColor = ForegroundColor;
            Console.Write(line);
            Console.Write('\n');

            Console.ForegroundColor = saveForegroundColor;
            Console.SetCursorPosition(savePos.Left, savePos.Top);
        }
    }
}
