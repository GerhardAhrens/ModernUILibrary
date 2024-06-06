//-----------------------------------------------------------------------
// <copyright file="CursorTypeChanged.cs" company="Lifeprojects.de">
//     Class: CursorTypeChanged
//     Copyright © Lifeprojects.de 2024
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>06.06.2024 13:42:41</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------

namespace ModernUILibrary.ModernUIBase
{
    using System.Windows;
    using System.Windows.Input;

    public static class CursorTypeChanged
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CursorTypeChanged"/> class.
        /// </summary>
        static CursorTypeChanged()
        {
        }

        public static void SetCursor(FrameworkElement control, string cursorName = "None")
        {
            ArgumentException.ThrowIfNullOrEmpty(cursorName, nameof(cursorName));

            switch (cursorName)
            {
                case "AppStarting":
                    control.Cursor = Cursors.AppStarting;
                    break;
                case "ArrowCD":
                    control.Cursor = Cursors.ArrowCD;
                    break;
                case "Arrow":
                    control.Cursor = Cursors.Arrow;
                    break;
                case "Cross":
                    control.Cursor = Cursors.Cross;
                    break;
                case "HandCursor":
                    control.Cursor = Cursors.Hand;
                    break;
                case "Help":
                    control.Cursor = Cursors.Help;
                    break;
                case "IBeam":
                    control.Cursor = Cursors.IBeam;
                    break;
                case "No":
                    control.Cursor = Cursors.No;
                    break;
                case "None":
                    control.Cursor = Cursors.None;
                    break;
                case "Pen":
                    control.Cursor = Cursors.Pen;
                    break;
                case "ScrollSE":
                    control.Cursor = Cursors.ScrollSE;
                    break;
                case "ScrollWE":
                    control.Cursor = Cursors.ScrollWE;
                    break;
                case "SizeAll":
                    control.Cursor = Cursors.SizeAll;
                    break;
                case "SizeNESW":
                    control.Cursor = Cursors.SizeNESW;
                    break;
                case "SizeNS":
                    control.Cursor = Cursors.SizeNS;
                    break;
                case "SizeNWSE":
                    control.Cursor = Cursors.SizeNWSE;
                    break;
                case "SizeWE":
                    control.Cursor = Cursors.SizeWE;
                    break;
                case "UpArrow":
                    control.Cursor = Cursors.UpArrow;
                    break;
                case "WaitCursor":
                    control.Cursor = Cursors.Wait;
                    break;
                case "Custom":
                    /*control.Cursor = CustomCursor;*/
                    break;
                default:
                    break;
            }

            Mouse.OverrideCursor = control.Cursor;
        }
    }
}
