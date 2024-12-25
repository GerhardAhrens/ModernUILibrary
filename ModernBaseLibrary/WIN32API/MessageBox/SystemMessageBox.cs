
//-----------------------------------------------------------------------
// <copyright file="SystemMessageBox.cs" company="Lifeprojects.de">
//     Class: SystemMessageBox
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>11.01.2022</date>
//
// <summary>
// Klasse zum anzeigen einer MessageBox die über die Windows API erzeugt wird
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.Win32API
{
    using System;
    using System.Runtime.InteropServices;
    using System.Windows;

    public class SystemMessageBox
    {
        private delegate int MessageBoxDelegate(IntPtr hWnd, string pText, string pCaption, uint pType);
        private static MessageBoxDelegate internalMessageBox;

        static SystemMessageBox()
        {
            try
            {
                IntPtr hWnd;
                internalMessageBox = DLLFunctionLoader.LoadFunction<MessageBoxDelegate>("user32.dll", "MessageBoxA", out hWnd);
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                throw;
            }
        }

        public static int Show(string pText, string pCaption, uint pType = 0)
        {
            int result = -1;

            try
            {
                result = internalMessageBox(new IntPtr(0), pText, pCaption, pType);
            }
            catch (Exception ex)
            {
                string errText = ex.Message;
                throw;
            }

            return result;
        }
    }
}
