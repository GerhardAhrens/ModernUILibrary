//-----------------------------------------------------------------------
// <copyright file="HelpWindowResult.cs" company="lifeprojects.de">
//     Class: HelpWindowResult
//     Copyright © lifeprojects.de GmbH 2025
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>10.03.2025</date>
//
// <summary>
//  Class with HelpWindowResult Definition
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Data;

    public class HelpWindowResult
    {
        public HelpWindowResult(HelpWindowEventArgs e)
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

        public bool Result { get; private set; }

        public bool Cancelled { get; private set; }

        public Exception Error { get; private set; }

        public bool OperationFailed
        {
            get { return Error != null; }
        }
    }
}
