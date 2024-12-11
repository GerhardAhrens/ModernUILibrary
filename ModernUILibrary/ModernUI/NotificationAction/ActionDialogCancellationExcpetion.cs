//-----------------------------------------------------------------------
// <copyright file="ActionDialogCancellationExcpetion.cs" company="lifeprojects.de">
//     Class: ActionDialogCancellationExcpetion
//     Copyright © lifeprojects.de GmbH 2021
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>20.07.2021</date>
//
// <summary>
// Class with Exception from ActionDialog
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    internal class ActionDialogCancellationExcpetion : Exception
    {
        public ActionDialogCancellationExcpetion() : base()
        {
        }

        public ActionDialogCancellationExcpetion(string message) : base(message)
        {
        }

        public ActionDialogCancellationExcpetion(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ActionDialogCancellationExcpetion(string format, params string[] arg) : base(string.Format(format, arg))
        {
        }
    }
}