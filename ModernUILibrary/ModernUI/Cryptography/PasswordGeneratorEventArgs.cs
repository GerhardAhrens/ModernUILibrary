//-----------------------------------------------------------------------
// <copyright file="PasswordGeneratorEventArgs.cs" company="lifeprojects.de">
//     Class: PasswordGeneratorEventArgs
//     Copyright © lifeprojects.de GmbH 2023
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>6.06.2023</date>
//
// <summary>
//  Class with PasswordGeneratorEventArgs Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.ComponentModel;
    using System.Data;

    public class PasswordGeneratorEventArgs : CancelEventArgs
    {
        public string Result { get; set; }

        public Exception Error { get; set; }
    }
}
