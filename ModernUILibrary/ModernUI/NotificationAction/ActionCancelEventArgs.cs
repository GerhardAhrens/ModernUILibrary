//-----------------------------------------------------------------------
// <copyright file="ActionCancelEventArgs.cs" company="lifeprojects.de">
//     Class: ActionCancelEventArgs
//     Copyright © lifeprojects.de GmbH 2021
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>20.07.2021</date>
//
// <summary>
//  Class with ActionCancelEventArgs Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;

    public class ActionCancelEventArgs : CancelEventArgs
    {
        public object Result { get; set; }

        public Exception Error { get; set; }
    }
}
