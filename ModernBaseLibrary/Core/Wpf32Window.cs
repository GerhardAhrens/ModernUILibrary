//-----------------------------------------------------------------------
// <copyright file="Wpf32Window.cs" company="Lifeprojects.de">
//     Class: Wpf32Window
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>18.09.2018</date>
//
// <summary>
// Die Klasse gibt einen IntPtr von einem Window zurück
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Windows;
    using System.Windows.Interop;
    using WinForms = System.Windows.Forms;

    public class Wpf32Window : WinForms.IWin32Window
    {
        public IntPtr Handle { get; private set; }

        public Wpf32Window(Window wpfWindow)
        {
            this.Handle = new WindowInteropHelper(wpfWindow).Handle;
        }
    }
}
