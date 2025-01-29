//-----------------------------------------------------------------------
// <copyright file="CursorServices.cs" company="Lifeprojects.de">
//     Class: CursorServices
//     Copyright © Lifeprojects.de 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>31.07.2017</date>
//
// <summary>
// Helper class for UI CursorServices
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.WPF.Base
{
    using System;
    using System.Windows;
    using System.Windows.Input;
    using System.Windows.Threading;

    public static class CursorServices
    {
        private static bool isBusy;

        public static void SetBusyState()
        {
            SetBusyState(true);
        }

        private static void SetBusyState(bool busy)
        {
            if (busy != isBusy)
            {
                isBusy = busy;
                Mouse.OverrideCursor = busy ? Cursors.Wait : null;

                if (isBusy)
                {
                    new DispatcherTimer(TimeSpan.FromSeconds(0), DispatcherPriority.ApplicationIdle, DispatcherTimer_Tick, Application.Current.Dispatcher);
                }
            }
        }

        private static void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            var dispatcherTimer = sender as DispatcherTimer;
            if (dispatcherTimer != null)
            {
                SetBusyState(false);
                dispatcherTimer.Stop();
            }
        }
    }
}
