//-----------------------------------------------------------------------
// <copyright file="WpfScreen.cs" company="Lifeprojects.de">
//     Class: WpfScreen
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>15.04.2025 13:49:07</date>
//
// <summary>
// Die Klasse ermittelt alle über Windows Verfügbaren Screens
// </summary>
//-----------------------------------------------------------------------

namespace ModernUILibrary.WPF.Base
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Interop;

    public class WpfScreen
    {
        private readonly Screen screen;

        public static IEnumerable<WpfScreen> AllScreens()
        {
            foreach (Screen screen in System.Windows.Forms.Screen.AllScreens)
            {
                yield return new WpfScreen(screen);
            }
        }

        public static WpfScreen GetScreenFrom(Window window)
        {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            Screen screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            WpfScreen wpfScreen = new WpfScreen(screen);
            return wpfScreen;
        }

        public static WpfScreen GetScreenFrom(Point point)
        {
            int x = (int)Math.Round(point.X);
            int y = (int)Math.Round(point.Y);

            // are x,y device-independent-pixels ??
            System.Drawing.Point drawingPoint = new System.Drawing.Point(x, y);
            Screen screen = System.Windows.Forms.Screen.FromPoint(drawingPoint);
            WpfScreen wpfScreen = new WpfScreen(screen);

            return wpfScreen;
        }

        public static WpfScreen Primary
        {
            get { return new WpfScreen(System.Windows.Forms.Screen.PrimaryScreen); }
        }

        public static int CountScreens
        {
            get { return System.Windows.Forms.Screen.AllScreens.Count(); }
        }

        public static Size PrimarySize
        {
            get { return new WpfScreen(System.Windows.Forms.Screen.PrimaryScreen).WorkingArea.Size; }
        }

        internal WpfScreen(System.Windows.Forms.Screen screen)
        {
            this.screen = screen;
        }

        public System.Windows.Rect DeviceBounds
        {
            get { return this.GetRect(this.screen.Bounds); }
        }

        public System.Windows.Rect WorkingArea
        {
            get { return this.GetRect(this.screen.WorkingArea); }
        }

        private System.Windows.Rect GetRect(Rectangle value)
        {
            // should x, y, width, height be device-independent-pixels ??
            return new System.Windows.Rect
            {
                X = value.X,
                Y = value.Y,
                Width = value.Width,
                Height = value.Height
            };
        }

        public bool IsPrimary
        {
            get { return this.screen.Primary; }
        }

        public string DeviceName
        {
            get { return this.screen.DeviceName; }
        }
    }
}
