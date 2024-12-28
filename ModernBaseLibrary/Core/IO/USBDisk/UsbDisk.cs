//-----------------------------------------------------------------------
// <copyright file="UsbDisk.cs" company="Lifeprojects.de">
//     Class: UsbDisk
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
    using System.Text;

    public class UsbDisk
    {
        private const int KB = 1024;
        private const int MB = KB * 1000;
        private const int GB = MB * 1000;


        internal UsbDisk (string name)
        {
            this.Name = name;
            this.Model = String.Empty;
            this.Volume = String.Empty;
            this.FreeSpace = 0;
            this.Size = 0;
        }

        public ulong FreeSpace
        {
            get;
            internal set;
        }

        public string Model
        {
            get;
            internal set;
        }

        public string Name
        {
            get;
            private set;
        }


        public ulong Size
        {
            get;
            internal set;
        }

        public string Volume
        {
            get;
            internal set;
        }

        public override string ToString ()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(Name);
            builder.Append(" ");
            builder.Append(Volume);
            builder.Append(" (");
            builder.Append(Model);
            builder.Append(") ");
            builder.Append(FormatByteCount(FreeSpace));
            builder.Append(" free of ");
            builder.Append(FormatByteCount(Size));

            return builder.ToString();
        }


        private string FormatByteCount (ulong bytes)
        {
            string format = null;

            if (bytes < KB)
            {
                format = String.Format("{0} Bytes", bytes);
            }
            else if (bytes < MB)
            {
                bytes = bytes / KB;
                format = String.Format("{0} KB", bytes.ToString("N"));
            }
            else if (bytes < GB)
            {
                double dree = bytes / MB;
                format = String.Format("{0} MB", dree.ToString("N1"));
            }
            else
            {
                double gree = bytes / GB;
                format = String.Format("{0} GB", gree.ToString("N1"));
            }

            return format;
        }
    }
}
