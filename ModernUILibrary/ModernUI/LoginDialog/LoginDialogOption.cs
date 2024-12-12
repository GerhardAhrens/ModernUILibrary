//-----------------------------------------------------------------------
// <copyright file="ListSelectDialogOption.cs" company="lifeprojects.de">
//     Class: ListSelectDialogOption
//     Copyright © lifeprojects.de GmbH 2021
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>20.07.2019</date>
//
// <summary>
//  Class with ListSelectDialogOption Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Net;
    using System.Runtime.Versioning;
    using System.Security;

    using Microsoft.VisualBasic.ApplicationServices;

    [SupportedOSPlatform("windows")]
    public class LoginDialogOption
    {
        public LoginDialogOption()
        {
            this.UserPasswordHash = null;
            this.User = null;
            this.Password = null;
        }
        public LoginDialogOption(string userPasswordHash)
        {
            this.UserPasswordHash = userPasswordHash.ToSecureString();
            this.User = null;
            this.Password = null;
        }

        public LoginDialogOption(string user, string password)
        {
            this.User = user.ToSecureString();
            this.Password = password.ToSecureString();
            this.UserPasswordHash = null;
        }

        public string Title { get; set; }

        public string HeaderText { get; set; }

        public string InstructionText { get; set; }

        public bool PasswordRepeat { get; set; }

        public int AutoCloseDialog { get; set; } = -1;

        public SecureString UserPasswordHash { get; private set; }

        public SecureString User { get; private set; }

        public SecureString Password { get; private set; }
    }
}
