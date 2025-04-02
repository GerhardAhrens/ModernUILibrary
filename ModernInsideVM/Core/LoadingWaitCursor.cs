//-----------------------------------------------------------------------
// <copyright file="WaitCursor.cs" company="PTA GmbH">
//     Class: WaitCursor
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - PTA GmbH</author>
// <email>gerhard.ahrens@pta.de</email>
// <date>25.07.2017</date>
//
// <summary>
// Class with WaitCursor Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernInsideVM.Core
{
    using System;
    using System.Windows.Input;

    public class LoadingWaitCursor : IDisposable
    {
        private readonly Cursor previousCursor;

        public LoadingWaitCursor()
        {
            this.previousCursor = Mouse.OverrideCursor;

            Mouse.OverrideCursor = Cursors.Wait;
        }

        public LoadingWaitCursor(Cursor cursorTyp)
        {
            this.previousCursor = cursorTyp;

            Mouse.OverrideCursor = Cursors.Wait;
        }

        public Cursor CurrentCursor
        {
            get { return Mouse.OverrideCursor; }
        }

        public static void SetNormal()
        {
            Mouse.OverrideCursor = null;
        }

        public static void SetWait()
        {
            Mouse.OverrideCursor = Cursors.Wait;
        }

        public void Dispose()
        {
            Mouse.OverrideCursor = this.previousCursor;
        }
    }
}