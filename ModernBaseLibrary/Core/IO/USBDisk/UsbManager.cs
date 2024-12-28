//-----------------------------------------------------------------------
// <copyright file="UsbManager.cs" company="Lifeprojects.de">
//     Class: UsbManager
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
    using System.Management;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;
    using System.Windows.Forms;

    using ModernBaseLibrary.Extension;

    [SupportedOSPlatform("windows")]
    public class UsbManager : IDisposable
    {
        #region DriverWindow

        private class DriverWindow : NativeWindow, IDisposable
        {
            [StructLayout(LayoutKind.Sequential)]
            public struct DEV_BROADCAST_VOLUME
            {
                public int dbcv_size;           // size of the struct
                public int dbcv_devicetype;     // DBT_DEVTYP_VOLUME
                public int dbcv_reserved;       // reserved; do not use
                public int dbcv_unitmask;       // Bit 0=A, bit 1=B, and so on (bitmask)
                public short dbcv_flags;        // DBTF_MEDIA=0x01, DBTF_NET=0x02 (bitmask)
            }


            private const int WM_DEVICECHANGE = 0x0219;             // device state change
            private const int DBT_DEVICEARRIVAL = 0x8000;           // detected a new device
            private const int DBT_DEVICEQUERYREMOVE = 0x8001;       // preparing to remove
            private const int DBT_DEVICEREMOVECOMPLETE = 0x8004;    // removed 
            private const int DBT_DEVTYP_VOLUME = 0x00000002;       // logical volume


            public DriverWindow()
            {
                base.CreateHandle(new CreateParams());
            }


            public void Dispose()
            {
                base.DestroyHandle();
                GC.SuppressFinalize(this);
            }


            public event UsbStateChangedEventHandler StateChanged;


            protected override void WndProc(ref Message message)
            {
                base.WndProc(ref message);

                if ((message.Msg == WM_DEVICECHANGE) && (message.LParam != IntPtr.Zero))
                {
                    DEV_BROADCAST_VOLUME volume = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(
                        message.LParam, typeof(DEV_BROADCAST_VOLUME));

                    if (volume.dbcv_devicetype == DBT_DEVTYP_VOLUME)
                    {
                        switch (message.WParam.ToInt32())
                        {
                            case DBT_DEVICEARRIVAL:
                                SignalDeviceChange(UsbStateChange.Added, volume);
                                break;

                            case DBT_DEVICEQUERYREMOVE:
                                // can intercept
                                break;

                            case DBT_DEVICEREMOVECOMPLETE:
                                SignalDeviceChange(UsbStateChange.Removed, volume);
                                break;
                        }
                    }
                }
            }


            private void SignalDeviceChange(UsbStateChange state, DEV_BROADCAST_VOLUME volume)
            {
                string name = ToUnitName(volume.dbcv_unitmask);

                if (StateChanged != null)
                {
                    UsbDisk disk = new UsbDisk(name);
                    StateChanged(new UsbStateChangedEventArgs(state, disk));
                }
            }

            private string ToUnitName(int mask)
            {
                int offset = 0;
                while ((offset < 26) && ((mask & 0x00000001) == 0))
                {
                    mask = mask >> 1;
                    offset++;
                }

                if (offset < 26)
                {
                    return String.Format("{0}:", Convert.ToChar(Convert.ToInt32('A') + offset));
                }

                return "?:";
            }
        }

        #endregion WndProc Driver


        private delegate void GetDiskInformationDelegate(UsbDisk disk);


        private DriverWindow window;
        private UsbStateChangedEventHandler handler;
        private bool isDisposed;

        public UsbManager()
        {
            this.window = null;
            this.handler = null;
            this.isDisposed = false;
        }


        #region Lifecycle

        ~UsbManager()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                if (window != null)
                {
                    window.StateChanged -= new UsbStateChangedEventHandler(DoStateChanged);
                    window.Dispose();
                    window = null;
                }

                isDisposed = true;

                GC.SuppressFinalize(this);
            }
        }

        #endregion Lifecycle

        public event UsbStateChangedEventHandler StateChanged
        {
            add
            {
                if (window == null)
                {
                    // create the driver window once a consumer registers for notifications
                    window = new DriverWindow();
                    window.StateChanged += new UsbStateChangedEventHandler(DoStateChanged);
                }

                handler = (UsbStateChangedEventHandler)Delegate.Combine(handler, value);
            }

            remove
            {
                handler = (UsbStateChangedEventHandler)Delegate.Remove(handler, value);

                if (handler == null)
                {
                    window.StateChanged -= new UsbStateChangedEventHandler(DoStateChanged);
                    window.Dispose();
                    window = null;
                }
            }
        }

        public UsbDiskCollection GetAvailableDisks()
        {
            const string DISKDRIVEUSB = "select * from Win32_DiskDrive where InterfaceType='USB'";
            const string DEVICEID = "associators of {{Win32_DiskDrive.DeviceID='{0}'}} where AssocClass = Win32_DiskDriveToDiskPartition";

            UsbDiskCollection disks = new UsbDiskCollection();

            foreach (ManagementObject drive in new ManagementObjectSearcher(DISKDRIVEUSB).Get())
            {
                ManagementObject partition = new ManagementObjectSearcher(String.Format(DEVICEID,drive["DeviceID"])).First();

                if (partition != null)
                {
                    ManagementObject logical = new ManagementObjectSearcher(String.Format("associators of {{Win32_DiskPartition.DeviceID='{0}'}} where AssocClass = Win32_LogicalDiskToPartition", partition["DeviceID"])).First();

                    if (logical != null)
                    {
                        ManagementObject volume = new ManagementObjectSearcher(String.Format("select * from Win32_LogicalDisk where Name='{0}'",logical["Name"])).First();

                        UsbDisk disk = new UsbDisk(logical["Name"].ToString());

                        var d1 = drive["InstallDate"];
                        var d2 = drive["Caption"];
                        var d3 = drive["CreationClassName"];
                        var d4 = drive["DeviceID"];
                        var d5 = drive["Name"];
                        var d6 = drive["Manufacturer"];
                        var d7 = drive["SerialNumber"];
                        var d8 = drive["Status"];
                        var d9 = drive["FirmwareRevision"];
                        var d10 = drive["MediaType"];
                        var d11 = drive["Signature"];

                        disk.Model = drive["Model"].ToString();

                        var a1 = volume["Caption"].ToString();
                        var a2 = volume["Description"].ToString();
                        var a3 = volume["DeviceID"].ToString();
                        var a4 = volume["FileSystem"].ToString();
                        var a5 = volume["InstallDate"];
                        var a6 = volume["PNPDeviceID"];
                        var a7 = volume["ProviderName"];
                        var a8 = volume["VolumeSerialNumber"];

                        disk.Volume = volume["VolumeName"].ToString();
                        disk.FreeSpace = (ulong)volume["FreeSpace"];
                        disk.Size = (ulong)volume["Size"];

                        disks.Add(disk);
                    }
                }
            }

            return disks;
        }


        /// <summary>
        /// Internally handle state changes and notify listeners.
        /// </summary>
        /// <param name="e"></param>

        private void DoStateChanged(UsbStateChangedEventArgs e)
        {
            if (handler != null)
            {
                UsbDisk disk = e.Disk;

                if ((e.State == UsbStateChange.Added) && (e.Disk.Name[0] != '?'))
                {
                    GetDiskInformationDelegate gdi = new GetDiskInformationDelegate(GetDiskInformation);
                    IAsyncResult result = gdi.BeginInvoke(e.Disk, null, null);
                    gdi.EndInvoke(result);
                }

                handler(e);
            }
        }

        private void GetDiskInformation(UsbDisk disk)
        {
            ManagementObject partition = new ManagementObjectSearcher(String.Format(
                "associators of {{Win32_LogicalDisk.DeviceID='{0}'}} where AssocClass = Win32_LogicalDiskToPartition",
                disk.Name)).First();

            if (partition != null)
            {
                ManagementObject drive = new ManagementObjectSearcher(String.Format(
                    "associators of {{Win32_DiskPartition.DeviceID='{0}'}}  where resultClass = Win32_DiskDrive",
                    partition["DeviceID"])).First();

                if (drive != null)
                {
                    disk.Model = drive["Model"].ToString();
                }

                ManagementObject volume = new ManagementObjectSearcher(String.Format(
                    "select FreeSpace, Size, VolumeName from Win32_LogicalDisk where Name='{0}'",
                    disk.Name)).First();

                if (volume != null)
                {
                    disk.Volume = volume["VolumeName"].ToString();
                    disk.FreeSpace = (ulong)volume["FreeSpace"];
                    disk.Size = (ulong)volume["Size"];
                }
            }
        }
    }
}
