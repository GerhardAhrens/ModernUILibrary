//-----------------------------------------------------------------------
// <copyright file="ProgressBarDialogCancellationExcpetion.cs" company="lifeprojects.de">
//     Class: ProgressBarDialogCancellationExcpetion
//     Copyright © lifeprojects.de GmbH 2019
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>08.02.2019</date>
//
// <summary>
//  Class with Exception from ProgressBarDialog
// </summary>
//-----------------------------------------------------------------------

namespace ModernIU.Controls
{
    using System;
    using System.Runtime.Serialization;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    [Serializable]
    internal class ProgressBarDialogCancellationExcpetion : Exception
    {
        public ProgressBarDialogCancellationExcpetion()
            : base()
        {
        }

        public ProgressBarDialogCancellationExcpetion(string message)
            : base(message)
        {
        }

        public ProgressBarDialogCancellationExcpetion(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ProgressBarDialogCancellationExcpetion(string format, params string[] arg)
            : base(string.Format(format, arg))
        {
        }
        #pragma warning disable SYSLIB0051
        protected ProgressBarDialogCancellationExcpetion(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
        #pragma warning restore SYSLIB0051
    }
}