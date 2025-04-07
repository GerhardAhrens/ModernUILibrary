namespace ModernUI.MVVM.Base
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