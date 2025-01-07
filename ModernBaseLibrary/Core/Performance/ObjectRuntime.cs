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

    using ModernBaseLibrary.Extension;

    public class ObjectRuntime : DisposableCoreBase
    {
        private Stopwatch sw = null;

        public ObjectRuntime()
        {
            this.sw = new Stopwatch();
            this.sw.Start();
        }

        public string Result()
        {
            this.sw.Stop();

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

            return this.sw.ElapsedMilliseconds;
        }
    }
}
