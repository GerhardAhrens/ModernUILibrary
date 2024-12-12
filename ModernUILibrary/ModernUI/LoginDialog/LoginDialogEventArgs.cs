//-----------------------------------------------------------------------
// <copyright file="LoginDialogEventArgs.cs" company="lifeprojects.de">
//     Class: LoginDialogEventArgs
//     Copyright © lifeprojects.de GmbH 2023
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>29.05.2023</date>
//
// <summary>
//  Class with LoginDialogEventArgs Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;
    using System.Data;

    public class LoginDialogEventArgs : CancelEventArgs
    {
        public LoginDialogValid Result { get; set; }

        public object ResultOut { get; set; }

        public Exception Error { get; set; }
    }
}
