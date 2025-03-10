//-----------------------------------------------------------------------
// <copyright file="HelpWindowEventArgs.cs" company="lifeprojects.de">
//     Class: HelpWindowEventArgs
//     Copyright © lifeprojects.de GmbH 2025
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>10.03.2025</date>
//
// <summary>
//  Class with HelpWindowEventArgs Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;
    using System.Data;

    public class HelpWindowEventArgs : CancelEventArgs
    {
        public bool Result { get; set; }

        public Exception Error { get; set; }
    }
}
