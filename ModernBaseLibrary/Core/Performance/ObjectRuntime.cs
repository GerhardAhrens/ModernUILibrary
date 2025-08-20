//-----------------------------------------------------------------------
// <copyright file="ObjectRuntime.cs" company="Lifeprojects.de">
//     Class: ObjectRuntime
//     Copyright © Lifeprojects.de 2020
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>17.11.2020</date>
//
// <summary>
// Die Klasse stellt ein Stopwatch Objekt zur Verfügung
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System.Diagnostics;
    using System.Windows.Input;

    using ModernBaseLibrary.Extension;

    public class ObjectRuntime : DisposableCoreBase
    {
        private readonly Cursor previousCursor;

        private Stopwatch sw = null;

        public ObjectRuntime()
        {
            this.sw = new Stopwatch();
            this.sw.Start();

            this.previousCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        public ObjectRuntime(Cursor cursorTyp)
        {
            this.sw = new Stopwatch();
            this.sw.Start();

            this.previousCursor = cursorTyp;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        public string Result()
        {
            this.sw.Stop();
            Mouse.OverrideCursor = this.previousCursor;
            return this.sw.GetTimeString();
        }

        public long ResultTicks()
        {
            this.sw.Stop();

            return this.sw.ElapsedTicks;
        }

        public long ResultMilliseconds()
        {
            this.sw.Stop();
            Mouse.OverrideCursor = this.previousCursor;
            return this.sw.ElapsedMilliseconds;
        }

        public static void SetNormal()
        {
            Mouse.OverrideCursor = null;
        }

        public static void SetWait()
        {
            Mouse.OverrideCursor = Cursors.Wait;
        }

        public override void DisposeManagedResources()
        {
            Mouse.OverrideCursor = this.previousCursor;
        }
    }
}
