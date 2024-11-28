//-----------------------------------------------------------------------
// <copyright file="PasswordGeneratorResult.cs" company="lifeprojects.de">
//     Class: PasswordGeneratorResult
//     Copyright © lifeprojects.de GmbH 2023
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>03.06.2023</date>
//
// <summary>
//  Class with PasswordGeneratorResult Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Data;

    public class PasswordGeneratorResult
    {
        public PasswordGeneratorResult(PasswordGeneratorEventArgs e)
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
            }
        }

        public string Result { get; private set; }

        public bool Cancelled { get; private set; }

        public Exception Error { get; private set; }

        public bool OperationFailed
        {
            get { return Error != null; }
        }
    }
}
