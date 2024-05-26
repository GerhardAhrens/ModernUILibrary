//-----------------------------------------------------------------------
// <copyright file="ScreenCapturerTo.cs" company="Lifeprojects.de">
//     Class: ScreenCapturerTo
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>05.07.2017</date>
//
// <summary>Definition of ScreenCapturerTo Class</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Base
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;

    [SupportedOSPlatform("windows")]
    public class ScreenCapturerTo : IDisposable
    {
        private static string assemblyName = null;
        private bool classIsDisposed = false;

        public ScreenCapturerTo()
        {
            if (UnitTestDetector.IsInUnitTest == true)
            {
                assemblyName =Path.GetFileNameWithoutExtension( Assembly.GetCallingAssembly().Location);
            }
            else
            {
                assemblyName = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
            }
        }

        public Point CursorPosition
        {
            get;
            protected set;
        }

        public string Capture()
        {
            return Capture(ScreenCaptureMode.Window, ScreenCaptureOutput.ToClipboard);
        }

        public string Capture(ScreenCaptureMode screenCaptureMode)
        {
            return Capture(screenCaptureMode, ScreenCaptureOutput.ToClipboard);
        }

        public string Capture(ScreenCaptureMode screenCaptureMode, ScreenCaptureOutput screenCaptureOutput)
        {
            Rectangle bounds;
            string fileName = string.Empty;

            if (screenCaptureMode == ScreenCaptureMode.Screen)
            {
                bounds = Screen.GetBounds(Point.Empty);
                this.CursorPosition = Cursor.Position;
            }
            else
            {
                var foregroundWindowsHandle = GetForegroundWindow();
                var rect = new Rect();
                GetWindowRect(foregroundWindowsHandle, ref rect);
                bounds = new Rectangle(rect.Left, rect.Top, rect.Right - rect.Left, rect.Bottom - rect.Top);
                this.CursorPosition = new Point(Cursor.Position.X - rect.Left, Cursor.Position.Y - rect.Top);
            }

            Bitmap result = new Bitmap(bounds.Width, bounds.Height);

            using (var g = Graphics.FromImage(result))
            {
                g.CopyFromScreen(new Point(bounds.Left, bounds.Top), Point.Empty, bounds.Size);
            }

            if (screenCaptureOutput == ScreenCaptureOutput.ToClipboard)
            {
                Clipboard.SetImage(result);
                fileName = $"Bitmap: {result.Height}x{result.Width}";
            }
            else
            {
                int lastNumber = this.LastFileNumber();
                string appName = Assembly.GetExecutingAssembly().Location;
                fileName = Path.Combine(Path.GetDirectoryName(appName), string.Format("{0}{1}.png", assemblyName, lastNumber));
                result.Save(fileName, ImageFormat.Png);

                if (File.Exists(fileName) == true)
                {
                    string argument = "/select, \"" + fileName + "\"";
                    System.Diagnostics.Process.Start("explorer.exe", argument);
                }
            }


            return fileName;
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool classDisposing = false)
        {
            if (this.classIsDisposed == false)
            {
                if (classDisposing)
                {
                }
            }

            this.classIsDisposed = true;
        }

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern IntPtr GetWindowRect(IntPtr windowHandle, ref Rect rect);


        private int LastFileNumber()
        {
            int result = -1;

            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            DirectoryInfo di = new DirectoryInfo(assemblyPath);
            IEnumerable<FileInfo> files = di.EnumerateFiles(string.Format("{0}*.*", assemblyName), SearchOption.TopDirectoryOnly);

            FileInfo lastFile = files.OrderBy(x => x.FullName, new OrdinalStringComparer()).LastOrDefault();
            if (lastFile != null)
            {
                string resultString = Regex.Match(lastFile.FullName, @"\d+").Value;

                if (int.TryParse(resultString, out result) == false)
                {
                    result = 0;
                }
                else
                {
                    result = result + 1;
                }
            }
            else
            {
                result = 0;
            }

            return result;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct Rect
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
    }
}