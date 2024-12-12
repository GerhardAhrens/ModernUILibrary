//-----------------------------------------------------------------------
// <copyright file="LoginDialogResult.cs" company="lifeprojects.de">
//     Class: LoginDialogResult
//     Copyright © lifeprojects.de GmbH 2023
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>29.05.2023</date>
//
// <summary>
//  Class with LoginDialogResult Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Data;

    public class LoginDialogResult
    {
        public LoginDialogResult(LoginDialogEventArgs e)
        {
            if (e.Cancel == true)
            {
                this.Cancelled = true;
            }
            else if (e.Error != null)
            {
                this.Error = e.Error;
            }
            else
            {
                this.Result = e.Result;
                this.ResultOut = e.ResultOut;
            }
        }

        public LoginDialogValid Result { get; private set; } = LoginDialogValid.None;

        public object ResultOut { get; set; }

        public bool Cancelled { get; private set; }

        public Exception Error { get; private set; }

        public bool OperationFailed
        {
            get { return Error != null; }
        }
    }
}
