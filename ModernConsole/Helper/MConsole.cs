/*
 * <copyright file="MConsole.cs" company="Lifeprojects.de">
 *     Class: MConsole
 *     Copyright © Lifeprojects.de 2022
 * </copyright>
 *
 * <author>Gerhard Ahrens - Lifeprojects.de</author>
 * <email>developer@lifeprojects.de</email>
 * <date>27.09.2022</date>
 * <Project>EasyPrototypingNET</Project>
 *
 * <summary>
 * Erweiterungen zur Unterstützung von Consolen Applikationen
 * </summary>
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by the Free Software Foundation, 
 * either version 3 of the License, or (at your option) any later version.
 * This program is distributed in the hope that it will be useful,but WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.You should have received a copy of the GNU General Public License along with this program. 
 * If not, see <http://www.gnu.org/licenses/>.
*/

namespace System
{
    using System.Diagnostics;
    using System.Globalization;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Text;
    using System.Threading;

    using ModernConsole.Extension;

    [SupportedOSPlatform("windows")]
    public static partial class MConsole
    {
        static MConsole()
        {
            Console.OutputEncoding = Encoding.Unicode;
        }

        #region Encoding
        public static void FixEncoding()
        {
            FixEncoding(Encoding.Unicode);
        }

        /// <summary>
        /// Fixes the encoding of the console window for unsupported UI cultures. This method
        /// should be called once at application startup.
        /// </summary>
        public static void FixEncoding(Encoding encoding)
        {

            Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentUICulture.GetConsoleFallbackUICulture();
            if (Console.OutputEncoding.CodePage != 65001 &&
                Console.OutputEncoding.CodePage != Thread.CurrentThread.CurrentUICulture.TextInfo.OEMCodePage &&
                Console.OutputEncoding.CodePage != Thread.CurrentThread.CurrentUICulture.TextInfo.ANSICodePage)
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("de-DE");
            }
        }

        #endregion Encoding

        #region Environment

        #region Native interop

        /// <summary>
        /// Defines values returned by the GetFileType function.
        /// </summary>
        private enum FileType : uint
        {
            /// <summary>The specified file is a character file, typically an LPT device or a console.</summary>
            FileTypeChar = 0x0002,

            /// <summary>The specified file is a disk file.</summary>
            FileTypeDisk = 0x0001,

            /// <summary>The specified file is a socket, a named pipe, or an anonymous pipe.</summary>
            FileTypePipe = 0x0003,

            /// <summary>Unused.</summary>
            FileTypeRemote = 0x8000,

            /// <summary>Either the type of the specified file is unknown, or the function failed.</summary>
            FileTypeUnknown = 0x0000,
        }

        /// <summary>
        /// Defines standard device handles for the GetStdHandle function.
        /// </summary>
        private enum StdHandle : int
        {
            /// <summary>The standard input device. Initially, this is the console input buffer, CONIN$.</summary>
            Input = -10,

            /// <summary>The standard output device. Initially, this is the active console screen buffer, CONOUT$.</summary>
            Output = -11,

            /// <summary>The standard error device. Initially, this is the active console screen buffer, CONOUT$.</summary>
            Error = -12,
        }

        /// <summary>
        /// Retrieves the file type of the specified file.
        /// </summary>
        /// <param name="hFile">A handle to the file.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        private static extern FileType GetFileType(IntPtr hFile);

        /// <summary>
        /// Retrieves a handle to the specified standard device (standard input, standard output,
        /// or standard error).
        /// </summary>
        /// <param name="nStdHandle">The standard device.</param>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(StdHandle nStdHandle);

        /// <summary>
        /// Retrieves the window handle used by the console associated with the calling process.
        /// </summary>
        /// <returns></returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// Determines the visibility state of the specified window.
        /// </summary>
        /// <param name="hWnd">A handle to the window to be tested.</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        private static extern bool IsWindowVisible(IntPtr hWnd);

        #endregion Native interop

        /// <summary>
        /// Gets a value indicating whether the current application has an interactive console and
        /// is able to interact with the user through it.
        /// </summary>
        public static bool IsInteractiveAndVisible
        {
            get
            {
                IntPtr consoleWnd = GetConsoleWindow();
                return Environment.UserInteractive &&
                    consoleWnd != IntPtr.Zero &&
                    IsWindowVisible(consoleWnd) &&
                    !IsInputRedirected &&
                    !IsOutputRedirected &&
                    !IsErrorRedirected;
            }
        }

        private static bool? isInputRedirected;
        private static bool? isOutputRedirected;
        private static bool? isErrorRedirected;

        /// <summary>
        /// Gets a value that indicates whether input has been redirected from the standard input
        /// stream.
        /// </summary>
        /// <remarks>
        /// The value is cached after the first access.
        /// </remarks>
        public static bool IsInputRedirected
        {
            get
            {
                if (isInputRedirected == null)
                {
                    isInputRedirected = GetFileType(GetStdHandle(StdHandle.Input)) != FileType.FileTypeChar;
                }
                return isInputRedirected == true;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether output has been redirected from the standard output
        /// stream.
        /// </summary>
        /// <remarks>
        /// The value is cached after the first access.
        /// </remarks>
        public static bool IsOutputRedirected
        {
            get
            {
                if (isOutputRedirected == null)
                {
                    isOutputRedirected = GetFileType(GetStdHandle(StdHandle.Output)) != FileType.FileTypeChar;
                }
                return isOutputRedirected == true;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the error output stream has been redirected from the
        /// standard error stream.
        /// </summary>
        /// <remarks>
        /// The value is cached after the first access.
        /// </remarks>
        public static bool IsErrorRedirected
        {
            get
            {
                if (isErrorRedirected == null)
                {
                    isErrorRedirected = GetFileType(GetStdHandle(StdHandle.Error)) != FileType.FileTypeChar;
                }
                return isErrorRedirected == true;
            }
        }

        #endregion Environment

        #region Cursor

        /// <summary>
        /// Moves the cursor in the current line.
        /// </summary>
        /// <param name="count">The number of characters to move the cursor. Positive values move to the right, negative to the left.</param>
        public static void MoveCursor(int count)
        {
            if (IsOutputRedirected == false)
            {
                int x = Console.CursorLeft + count;
                if (x < 0)
                {
                    x = 0;
                }
                if (x >= Console.BufferWidth)
                {
                    x = Console.BufferWidth - 1;
                }

                Console.CursorLeft = x;
            }
        }

        /// <summary>
        /// Clears the current line and moves the cursor to the first column.
        /// </summary>
        public static void ClearLine()
        {
            if (!IsOutputRedirected)
            {
                Console.CursorLeft = 0;
                Console.Write(new string(' ', Console.BufferWidth - 1));
                Console.CursorLeft = 0;
            }
            else
            {
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Clears the current line and moves the cursor to the first column.
        /// </summary>
        public static void ClearScreen()
        {
            Console.Clear();
            Console.CursorLeft = 0;
        }

        #endregion Cursor

        #region Color output
        public static void Write()
        {
            Console.Write(string.Empty);
        }

        /// <summary>
        /// Writes a text in a different color. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void Write(string text, ConsoleColor color = ConsoleColor.White)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Writes a text in a different color. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textColor"></param>
        /// <param name="backColor"></param>
        public static void Write(string text, ConsoleColor textColor, ConsoleColor backColor)
        {
            var oldTextColor = Console.ForegroundColor;
            var oldBackColor = Console.BackgroundColor;
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backColor;
            Console.Write(text);
            Console.ForegroundColor = oldTextColor;
            Console.BackgroundColor = oldBackColor;
        }

        public static void NewLine()
        {
            Console.WriteLine($"\n");
        }

        public static void WriteLine()
        {
            Console.WriteLine(string.Empty);
        }

        /// <summary>
        /// Writes a text in a different color. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void WriteLine(string text, ConsoleColor color = ConsoleColor.White)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Writes a text in a different color. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="textColor"></param>
        /// <param name="backColor"></param>
        public static void WriteLine(object text, ConsoleColor textColor, ConsoleColor backColor)
        {
            var oldTextColor = Console.ForegroundColor;
            var oldBackColor = Console.BackgroundColor;
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backColor;
            Console.WriteLine(text);
            Console.ForegroundColor = oldTextColor;
            Console.BackgroundColor = oldBackColor;
        }

        /// <summary>
        /// Writes as Error a text in a red color. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void WriteErrorLine(object text)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Writes as Succes a text in a red color. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void WriteSuccessLine(object text)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Writes as Info a text in a red color. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public static void WriteInfoLine(object text)
        {
            var oldColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(text);
            Console.ForegroundColor = oldColor;
        }

        /// <summary>
        /// Writes a text with custom format control characters. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="formatter">A function that can set the console color depending on the input
        ///   character. Return false to hide the character.</param>
        public static void WriteLineFormatted(string text, Func<char, bool> formatter)
        {
            WriteFormatted(text, formatter);
            Console.WriteLine();
        }

        /// <summary>
        /// Writes a text with custom format control characters. The previous color is restored.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="formatter">A function that can set the console color depending on the input
        ///   character. Return false to hide the character.</param>
        public static void WriteFormatted(string text, Func<char, bool> formatter)
        {
            var oldTextColor = Console.ForegroundColor;
            var oldBackColor = Console.BackgroundColor;
            foreach (char ch in text)
            {
                if (formatter(ch))
                {
                    Console.Write(ch);
                }
            }
            Console.ForegroundColor = oldTextColor;
            Console.BackgroundColor = oldBackColor;
        }

        /*
        public static void Line(string text, int count, ConsoleColor textColor, ConsoleColor backColor = ConsoleColor.Black)
        {
            ConsoleColor oldTextColor = Console.ForegroundColor;
            ConsoleColor oldBackColor = Console.BackgroundColor;
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backColor;
            Console.WriteLine(text.Repeat(count));
            Console.ForegroundColor = oldTextColor;
            Console.BackgroundColor = oldBackColor;
        }

        public static void Line(string text, int count = 40, ConsoleColor textColor = ConsoleColor.White)
        {
            ConsoleColor oldTextColor = Console.ForegroundColor;
            ConsoleColor oldBackColor = Console.BackgroundColor;
            Console.ForegroundColor = textColor;
            Console.WriteLine(text.Repeat(count));
            Console.ForegroundColor = oldTextColor;
            Console.BackgroundColor = oldBackColor;
        }
        */

        public static void RepeatLine(string text, int count, ConsoleColor textColor, ConsoleColor backColor = ConsoleColor.Black)
        {
            var oldTextColor = Console.ForegroundColor;
            var oldBackColor = Console.BackgroundColor;
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backColor;
            Console.WriteLine(text.Repeat(count));
            Console.ForegroundColor = oldTextColor;
            Console.BackgroundColor = oldBackColor;
        }

        public static void RepeatLine(string text, int count,ConsoleColor textColor = ConsoleColor.White)
        {
            RepeatLine(text, count, textColor, ConsoleColor.Black);
        }

        public static void RepeatLine(string text, ConsoleColor textColor = ConsoleColor.White)
        {
            RepeatLine(text, 40, textColor, ConsoleColor.Black);
        }

        public static void RepeatLine(string text = "=")
        {
            RepeatLine(text, 40, ConsoleColor.White, ConsoleColor.Black);
        }

        #endregion Color output

        #region Progress bar

        private static string progressTitle;
        private static ConsoleColor progressTitleColor = ConsoleColor.White;
        private static ConsoleColor progressBackColor = ConsoleColor.DarkGreen;
        private static int progressValue;
        private static int progressTotal;
        private static bool progressHasWarning;
        private static bool progressHasError;

        public static ConsoleColor ProgressBackColor
        {
            get { return progressBackColor; }
            set
            {
                if (value != progressBackColor)
                {
                    progressBackColor = value;
                    WriteProgress(value);
                }
            }
        }

        /// <summary>
        /// Gets or sets the progress title, and updates the displayed progress bar accordingly.
        /// </summary>
        /// <remarks>
        /// A progress bar is only displayed if <see cref="ProgressTotal"/> is greater than zero.
        /// </remarks>
        public static ConsoleColor ProgressTitleColor
        {
            get { return progressTitleColor; }
            set
            {
                if (value != progressTitleColor)
                {
                    progressTitleColor = value;
                    WriteProgress();
                }
            }
        }

        public static string ProgressTitle
        {
            get { return progressTitle; }
            set
            {
                if (value != progressTitle)
                {
                    progressTitle = value;
                    WriteProgress();
                }
            }
        }
        /// <summary>
        /// Gets or sets the current value of the progress, and updates the displayed progress bar
        /// accordingly.
        /// </summary>
        /// <remarks>
        /// A progress bar is only displayed if <see cref="ProgressTotal"/> is greater than zero.
        /// </remarks>
        public static int ProgressValue
        {
            get { return progressValue; }
            set
            {
                if (value != progressValue)
                {
                    progressValue = value;
                    WriteProgress(ProgressBackColor);
                }
            }
        }

        /// <summary>
        /// Gets or sets the total value of the progress, and updates the displayed progress bar
        /// accordingly. Setting a value of zero or less clears the progress bar and resets its
        /// state.
        /// </summary>
        /// <remarks>
        /// A progress bar is only displayed if <see cref="ProgressTotal"/> is greater than zero.
        /// </remarks>
        public static int ProgressTotal
        {
            get { return progressTotal; }
            set
            {
                if (value != progressTotal)
                {
                    progressTotal = value;
                    WriteProgress(progressBackColor);
                    if (progressTotal <= 0)
                    {
                        // Reset progress
                        progressValue = 0;
                        progressHasWarning = false;
                        progressHasError = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether a warning occured during processing, and
        /// updates the displayed progress bar accordingly.
        /// </summary>
        /// <remarks>
        /// A progress bar is only displayed if <see cref="ProgressTotal"/> is greater than zero.
        /// </remarks>
        public static bool ProgressHasWarning
        {
            get { return progressHasWarning; }
            set
            {
                if (value != progressHasWarning)
                {
                    progressHasWarning = value;
                    WriteProgress();
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether an error occured during processing, and updates
        /// the displayed progress bar accordingly.
        /// </summary>
        /// <remarks>
        /// A progress bar is only displayed if <see cref="ProgressTotal"/> is greater than zero.
        /// </remarks>
        public static bool ProgressHasError
        {
            get { return progressHasError; }
            set
            {
                if (value != progressHasError)
                {
                    progressHasError = value;
                    WriteProgress();
                }
            }
        }

        /// <summary>
        /// Writes the progress info in the current line, replacing the current line.
        /// </summary>
        private static void WriteProgress(ConsoleColor graphColor = ConsoleColor.DarkGreen)
        {
            // Replace the current line with the new progress
            ClearLine();
            if (progressTotal > 0)
            {
                // Range checking
                int value = progressValue;
                if (value < 0)
                {
                    value = 0;
                }

                if (value > progressTotal)
                {
                    value = progressTotal;
                }

                MConsole.Write($"{ProgressTitle} {value.ToString().PadLeft(progressTotal.ToString().Length)}/{progressTotal} ", ProgressTitleColor);

                // Use almost the entire remaining visible space for the progress bar
                int graphLength = 80;
                if (!IsOutputRedirected)
                {
                    graphLength = Console.WindowWidth - Console.CursorLeft - 4;
                }

                int graphPart = progressTotal > 0 ? (int)Math.Round((double)value / progressTotal * graphLength) : 0;

                if (progressHasError == true)
                {
                    graphColor = ConsoleColor.DarkRed;
                }
                else if (progressHasWarning == true)
                {
                    graphColor = ConsoleColor.DarkYellow;
                }

                Write(new string('█', graphPart), graphColor);
                Write(new string('░', graphLength - graphPart), ConsoleColor.DarkGray);
            }
        }

        #endregion Progress bar

        #region Line wrapping

        /// <summary>
        /// Writes a string in multiple lines, limited to the console window width, wrapping at
        /// spaces whenever possible and keeping the first line indenting for wrapped lines.
        /// </summary>
        /// <param name="text">The text to write to the console.</param>
        /// <param name="tableMode">Indents to the last occurence of two spaces; otherwise indents to leading spaces.</param>
        public static void WriteWrapped(string text, bool tableMode = false)
        {
            int width = !IsOutputRedirected ? Console.WindowWidth : 80;
            foreach (string line in text.Split('\n'))
            {
                Console.Write(FormatWrapped(line.TrimEnd(), width, tableMode));
            }
        }

        /// <summary>
        /// Writes a string with custom format control characters in multiple lines, limited to the
        /// console window width, wrapping at spaces whenever possible and keeping the first line
        /// indenting for wrapped lines. The previous color is restored.
        /// </summary>
        /// <param name="text">The text to write to the console.</param>
        /// <param name="formatter">A function that can set the console color depending on the input
        ///   character. Return false to hide the character.</param>
        /// <param name="tableMode">Indents to the last occurence of two spaces; otherwise indents to leading spaces.</param>
        public static void WriteWrappedFormatted(string text, Func<char, bool> formatter, bool tableMode = false)
        {
            int width = !IsOutputRedirected ? Console.WindowWidth : 80;
            foreach (string line in text.Split('\n'))
            {
                WriteFormatted(FormatWrapped(line.TrimEnd(), width, tableMode), formatter);
            }
        }

        /// <summary>
        /// Formats a string to multiple lines, limited to the specified width, wrapping at spaces
        /// whenever possible and keeping the first line indenting for wrapped lines.
        /// </summary>
        /// <param name="input">The input string to format.</param>
        /// <param name="width">The available width for wrapping.</param>
        /// <param name="tableMode">Indents to the last occurence of two spaces; otherwise indents to leading spaces.</param>
        /// <returns>The formatted string with line breaks and indenting in every line.</returns>
        public static string FormatWrapped(string input, int width, bool tableMode)
        {
            if (input.TrimEnd() == string.Empty)
                return Environment.NewLine;

            // Detect by how many spaces the text is indented. This amount will be used for every
            // following wrapped line.
            int indent = 0;
            if (tableMode)
            {
                indent = input.LastIndexOf("  ");
                if (indent != -1)
                {
                    indent += 2;
                }
                else
                {
                    indent = 0;
                }
            }
            else
            {
                while (input[indent] == ' ')
                {
                    indent++;
                }
            }
            string indentStr = string.Empty;
            if (indent > 0)
            {
                indentStr = new string(' ', indent);
            }

            string output = string.Empty;
            bool haveReducedWidth = false;
            do
            {
                int pos = width - 1;
                if (pos >= input.Length)
                {
                    pos = input.Length;
                }
                else
                {
                    while (pos > 0 && input[pos] != ' ')
                    {
                        pos--;
                    }

                    // If the line cannot be wrapped at a space, write it to the full width
                    if (pos == 0)
                    {
                        pos = width - 1;
                    }
                }
                if (output != string.Empty)
                {
                    // Prepend indenting spaces for the following lines
                    output += indentStr;
                }
                output += input.Substring(0, pos) + Environment.NewLine;
                if (pos + 1 < input.Length)
                {
                    input = input.Substring(pos + 1);
                    // Reduce the available width by the indenting for the following lines
                    if (!haveReducedWidth)
                    {
                        width -= indent;
                        haveReducedWidth = true;
                    }
                }
                else
                {
                    input = string.Empty;
                }
            }

            while (input.Length > 0);
            return output;
        }

        #endregion Line wrapping

        #region Interaction

        public static void Clear(ConsoleColor textColor = ConsoleColor.White, ConsoleColor backColor = ConsoleColor.Black)
        {
            Console.Clear();
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backColor;
            Console.CursorLeft = 0;
        }

        /// <summary>
        /// Clears the key input buffer. Any keys that have been pressed but not yet processed
        /// before will be dropped.
        /// </summary>
        public static void ClearKeyBuffer()
        {
            if (!IsInputRedirected)
            {
                while (Console.KeyAvailable)
                {
                    Console.ReadKey(true);
                }
            }
        }

        /// <summary>
        /// Determines whether the specified key is an input key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool IsInputKey(ConsoleKey key)
        {
            int[] ignore = new[]
            {
                16,    // Shift (left or right)
                17,    // Ctrl (left or right)
                18,    // Alt (left or right)
                19,    // Pause
                20,    // Caps lock
                42,    // Print
                44,    // Print screen
                91,    // Windows key (left)
                92,    // Windows key (right)
                93,    // Menu key
                144,   // Num lock
                145,   // Scroll lock
                166,   // Back
                167,   // Forward
                168,   // Refresh
                169,   // Stop
                170,   // Search
                171,   // Favorites
                172,   // Start/Home
                173,   // Mute
                174,   // Volume Down
                175,   // Volume Up
                176,   // Next Track
                177,   // Previous Track
                178,   // Stop Media
                179,   // Play
                180,   // Mail
                181,   // Select Media
                182,   // Application 1
                183    // Application 2
            };
            return ignore.Contains((int)key) == false;
        }

        /// <summary>
        /// Waits for the user to press any key if in interactive mode and input is not redirected.
        /// </summary>
        /// <param name="message">The message to display. If null, a standard message is displayed.</param>
        /// <param name="timeout">The time in seconds until the method returns even if no key was pressed. If -1, the timeout is infinite.</param>
        /// <param name="showDots">true to show a dot for every second of the timeout, removing one dot each second.</param>
        public static ConsoleKey Wait(string message, int timeout, bool showDots, ConsoleColor textColor)
        {
            ConsoleKey lastKey = ConsoleKey.NoName;

            if (Environment.UserInteractive && IsInputRedirected == false)
            {
                if (message == null)
                {
                    if (CultureInfo.CurrentCulture.Name == "de-DE")
                    {
                        message = "Drücken Sie eine Taste für weiter...";
                    }
                    else
                    {
                        message = "Press any key to continue...";
                    }
                }

                if (message != string.Empty)
                {
                    ClearLine();
                    Write(message, textColor);
                }

                if (timeout < 0)
                {
                    ClearKeyBuffer();
                    // Wait for a real input key
                    lastKey = Console.ReadKey(true).Key;
                    while (IsInputKey(lastKey) == false)
                    {
                    }
                }
                else
                {
                    int counter;
                    counter = timeout;
                    while (counter > 0)
                    {
                        counter--;
                        if (showDots == true)
                        {
                            Console.Write(".");
                        }
                    }

                    timeout *= 1000;   // Convert to milliseconds
                    counter = 0;
                    int step = 100;   // Sleeping duration
                    int nextSecond = 1000;
                    ClearKeyBuffer();
                    while (!(Console.KeyAvailable && IsInputKey(Console.ReadKey(true).Key)) && counter < timeout)
                    {
                        Thread.Sleep(step);
                        counter += step;
                        if (showDots && counter > nextSecond)
                        {
                            nextSecond += 1000;
                            if (showDots == true)
                            {
                                MoveCursor(-1);
                                Console.Write(" ");
                                MoveCursor(-1);
                            }
                        }
                    }

                    ClearKeyBuffer();

                    if (message != string.Empty)
                    {
                        Console.WriteLine();
                    }
                }
            }

            return lastKey;
        }

        public static ConsoleKey Wait(string message, int timeout = -1, bool showDots = false)
        {
            ConsoleKey lastKey = Wait(message, timeout, showDots, Console.ForegroundColor);
            return lastKey;
        }

        public static ConsoleKey Wait(string message, ConsoleColor textColor = ConsoleColor.White)
        {
            ConsoleKey lastKey = Wait(message, -1, false, textColor);
            return lastKey;
        }

        public static ConsoleKey Wait(string message = null)
        {
            ConsoleKey lastKey;

            if (string.IsNullOrEmpty(message) == true)
            {
                lastKey = Wait($"\nEine Taste drücken für weiter!", -1, false, Console.ForegroundColor);
            }
            else
            {
                lastKey = Wait($"{message}\n\nEine Taste drücken für weiter!", -1, false, Console.ForegroundColor);
            }

            return lastKey;
        }

        public static ConsoleKey Wait(int timeout,bool showDots = false)
        {
            ConsoleKey lastKey = Wait(null, timeout, showDots, Console.ForegroundColor);
            return lastKey;
        }

        public static ConsoleKey Wait(ConsoleColor textColor)
        {
            ConsoleKey lastKey = Wait("Eine Taste drücken für weiter!", -1, false, textColor);
            return lastKey;
        }

        public static ConsoleKey Wait(ConsoleColor textColor, int timeout)
        {
            ConsoleKey lastKey = Wait("Eine Taste drücken für weiter!", timeout, false, textColor);
            return lastKey;
        }

        /// <summary>
        /// Waits for the user to press any key if in debugging mode.
        /// </summary>
        /// <remarks>
        /// Visual Studio will wait after the program terminates only if not debugging. When the
        /// program was started with debugging, the console window is closed immediately. This
        /// method can be called at the end of the program to always wait once and be able to
        /// evaluate the last console output.
        /// </remarks>
        public static void WaitIfDebug()
        {
            if (Debugger.IsAttached)
            {
                Wait("Press any key to quit...");
            }
        }

        /// <summary>
        /// Writes an error message in red color, waits for a key and returns the specified exit
        /// code for passing it directly to the return statement.
        /// </summary>
        /// <param name="message">The error message to write.</param>
        /// <param name="exitCode">The exit code to return.</param>
        /// <returns></returns>
        public static int ExitError(string message, int exitCode)
        {
            ClearLine();
            using (new ConsoleColorScope(ConsoleColor.Red))
            {
                Console.Error.WriteLine(message);
            }

            WaitIfDebug();

            return exitCode;
        }

        public static string Input()
        {
            WriteLine("Geben Sie einen Wert ein:");
            return Console.ReadLine();
        }

        public static TResult Input<TResult>(string inputDescription)
        {
            WriteLine($"{inputDescription}:");

            object result = Console.ReadLine(); 

            if (result != null)
            {
                try
                {
                    return result.To<TResult>();
                }
                catch (Exception ex)
                {
                    string errorText = ex.Message;
                    throw;
                }
            }

            return default;
        }

        #endregion Interaction

        #region Clear Screen/Area
        public static void ClearToEndOfCurrentLine(int row, ConsoleColor backColor = ConsoleColor.Black)
        {
            var saveBackColor = Console.BackgroundColor;
            Console.BackgroundColor = backColor;
            var savePos = Console.GetCursorPosition();
            Console.CursorVisible = false;

            int currentLeft = Console.CursorLeft;
            for (int i = row; i < Console.BufferHeight; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write(new string(' ', Console.WindowWidth - currentLeft));
            }

            Console.SetCursorPosition(savePos.Left, savePos.Top);

            Console.BackgroundColor = saveBackColor;
        }

        public static void ClearArea(int startRow, int startColumn, int width, int height)
        {
            var savePos = Console.GetCursorPosition();

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Console.SetCursorPosition(startColumn + col, startRow + row);
                    Console.Write(' ');
                }
            }

            Console.SetCursorPosition(savePos.Left, savePos.Top);
        }

        public static void ClearArea(int startRow, int startColumn, int width, int height, ConsoleColor backColor = ConsoleColor.Black)
        {
            var savePos = Console.GetCursorPosition();
            var saveBackColor = Console.BackgroundColor;
            Console.BackgroundColor = backColor;

            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    Console.SetCursorPosition(startColumn + col, startRow + row);
                    Console.Write(' ');
                }
            }

            Console.BackgroundColor = saveBackColor;
            Console.SetCursorPosition(savePos.Left, savePos.Top);
        }
        #endregion

        #region Say/Get
        /// <summary>
        /// Stellt einen Text an der festgelegten Position dar
        /// </summary>
        /// <param name="row">Zeile</param>
        /// <param name="col">Spalte</param>
        /// <param name="say">text</param>
        /// <param name="inputLength">Länge der Eingabe</param>
        /// <example>
        /// ConsoleMenu.Say(0, 1, "Vorname:");
        /// </example>
        public static void Say(int row, int col, string say, int inputLength)
        {
            var savePos = Console.GetCursorPosition();
            var saveBackColor = Console.BackgroundColor;
            var saveForegroundColor = Console.ForegroundColor;

            Console.SetCursorPosition(col, row);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(say);
            Console.SetCursorPosition(col + say.Length, row);

            if (inputLength > 0)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.ForegroundColor = ConsoleColor.Black;

                for (int i = 0; i < inputLength; i++)
                {
                    Console.Write(' ');
                }
            }

            Console.ForegroundColor = saveForegroundColor;
            Console.BackgroundColor = saveBackColor;
            Console.SetCursorPosition(savePos.Left, savePos.Top);
        }

        /// <summary>
        /// Stellt einen Text an der festgelegten Position dar
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="say"></param>
        public static void Say(int row, int col, string say)
        {
            var savePos = Console.GetCursorPosition();
            var saveBackColor = Console.BackgroundColor;
            var saveForegroundColor = Console.ForegroundColor;

            Console.SetCursorPosition(col, row);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write(say);
            Console.SetCursorPosition(col + say.Length, row);

            Console.ForegroundColor = saveForegroundColor;
            Console.BackgroundColor = saveBackColor;
            Console.SetCursorPosition(savePos.Left, savePos.Top);
        }

        /// <summary>
        /// Eingabe eines Textes an angegebener Position
        /// </summary>
        /// <param name="row">Zeile</param>
        /// <param name="col">Spalte</param>
        /// <param name="inputLength">Länge der Eingabe</param>
        /// <returns>Eingegebener Text</returns>
        /// <example>
        /// string v1 = ConsoleMenu.Get(0, 9);
        /// </example>
        public static string Get(int row, int col, int inputLength = 10)
        {
            string result = string.Empty;

            var savePos = Console.GetCursorPosition();
            var saveBackColor = Console.BackgroundColor;
            var saveForegroundColor = Console.ForegroundColor;
            var saveCursorSize = Console.CursorSize;

            Console.BackgroundColor = ConsoleColor.White;
            Console.ForegroundColor = ConsoleColor.Black;

            Console.SetCursorPosition(col, row);
            Console.CursorSize = 100;

            List<char> keys = new List<char>();
            Console.CursorVisible = true;
            do
            {
                char key = Console.ReadKey(false).KeyChar;
                if (key == '\r')
                {
                    break;
                }
                else if (key == '\b')
                {
                    if (keys.Count > 0)
                    {
                        Console.Write(' ');
                        keys.RemoveAt(keys.Count - 1);
                        var cpos = Console.GetCursorPosition();
                        Console.SetCursorPosition(cpos.Left - 1, cpos.Top);
                    }
                    else
                    {
                        Console.SetCursorPosition(col, row);
                    }
                }
                else
                {
                    keys.Add(key);
                }
            } while (keys.Count < inputLength);

            Console.CursorVisible = false;
            result = string.Join(string.Empty, keys);

            Console.CursorSize = saveCursorSize;
            Console.ForegroundColor = saveForegroundColor;
            Console.BackgroundColor = saveBackColor;
            Console.SetCursorPosition(savePos.Left, savePos.Top);

            return result;
        }        
        #endregion Say/Get
    }

    #region ConsoleColorScope helper class

    /// <summary>
    /// Changes the console foreground color and changes it back again.
    /// </summary>
    public class ConsoleColorScope : IDisposable
    {
        private readonly ConsoleColor previousColor;

        /// <summary>
        /// Changes the console foreground color.
        /// </summary>
        /// <param name="color">The new foreground color.</param>
        public ConsoleColorScope(ConsoleColor color)
        {
            this.previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
        }

        /// <summary>
        /// Changes the foreground color to its previous value.
        /// </summary>
        public void Dispose()
        {
            Console.ForegroundColor = previousColor;
        }
    }

    #endregion ConsoleColorScope helper class

    #region ConsoleSpinner
    public class ConsoleSpinner : IDisposable
    {
        private string[,] sequence = new string[,] {
                { "/", "-", "\\", "|" },
                { ".", "o", "0", "o" },
                { "+", "x","+","x" },
                { "V", "<", "^", ">" },
                { ".   ", "..  ", "... ", "...." },
                { "=>   ", "==>  ", "===> ", "====>" }
        };

        private int counter = 0;
        private readonly int left;
        private readonly int top;
        private readonly int delay;
        private bool active;
        private readonly Thread thread;
        private int animationTyp;
        private ConsoleColor saveForegroundColor;

        public ConsoleSpinner(int left, int top, int delay = 100, int animationTyp = 0)
        {
            this.left = left;
            this.top = top;
            this.delay = delay;
            this.animationTyp = animationTyp;
            this.thread = new Thread(Spin);
            this.saveForegroundColor = Console.ForegroundColor;
        }

        public void Start()
        {
            active = true;
            if (!thread.IsAlive)
            {
                thread.Start();
                Console.CursorVisible = false;
                this.saveForegroundColor = Console.ForegroundColor;
            }
        }

        public void Stop()
        {
            active = false;
            int len = sequence[this.animationTyp, 0].Length;
            Draw(new string(' ', len));
            Console.CursorVisible = true;
            Console.ForegroundColor = saveForegroundColor;
        }

        private void Spin()
        {
            while (active)
            {
                Turn();
                Thread.Sleep(delay);
            }
        }

        private void Draw(string animation)
        {
            Console.SetCursorPosition(left, top);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(animation);
        }

        private void Turn()
        {
            counter++;
            int counterValue = counter % 4;
            string animation = sequence[this.animationTyp, counterValue];
            Draw(animation);
        }

        public void Dispose()
        {
            Stop();
        }
    }
    #endregion ConsoleSpinner
}