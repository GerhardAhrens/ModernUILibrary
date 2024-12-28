//-----------------------------------------------------------------------
// <copyright file="UsbStateChangedEventHandler.cs" company="Lifeprojects.de">
//     Class: UsbStateChangedEventHandler
//     Copyright © Lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>18.02.2023</date>
//
// <summary>
// Klasse für 
// </summary>
//-----------------------------------------------------------------------


namespace ModernBaseLibrary.Core.IO
{
    using System;

    public delegate void UsbStateChangedEventHandler (UsbStateChangedEventArgs e);

    
    public class UsbStateChangedEventArgs : EventArgs
    {
        public UsbStateChangedEventArgs (UsbStateChange state, UsbDisk disk)
        {
            this.State = state;
            this.Disk = disk;
        }

        public UsbDisk Disk
        {
            get;
            private set;
        }

        public UsbStateChange State
        {
            get;
            private set;
        }
    }
}
