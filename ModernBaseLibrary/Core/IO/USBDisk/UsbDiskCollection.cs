//-----------------------------------------------------------------------
// <copyright file="UsbDiskCollection.cs" company="Lifeprojects.de">
//     Class: UsbDiskCollection
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
    using System.Collections.ObjectModel;
    using System.Linq;


    public class UsbDiskCollection : ObservableCollection<UsbDisk>
    {

        public bool Contains (string name)
        {
            return this.AsQueryable<UsbDisk>().Any(d => d.Name == name) == true;
        }

        public bool Remove (string name)
        {
            UsbDisk disk = 
                (this.AsQueryable<UsbDisk>()
                .Where(d => d.Name == name)
                .Select(d => d)).FirstOrDefault<UsbDisk>();

            if (disk != null)
            {
                return this.Remove(disk);
            }

            return false;
        }
    }
}
