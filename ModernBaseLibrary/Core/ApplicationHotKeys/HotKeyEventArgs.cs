//-----------------------------------------------------------------------
// <copyright file="HotKeyEventArgs.cs" company="Lifeprojects.de">
//     Class: HotKeyEventArgs
//     Copyright © Lifeprojects.de 2022
// </copyright>
//
// <Framework>4.8</Framework>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>25.12.2022 12:51:03</date>
//
// <summary>
// EventArgs Class for 
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Globalization;

    public sealed class HotKeyEventArgs : EventArgs
    {
        public HotKey HotKey { get; private set; }

        public HotKeyEventArgs(HotKey hotKey)
        {
            this.HotKey = hotKey;
        }
    }
}
