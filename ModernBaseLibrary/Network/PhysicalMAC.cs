//-----------------------------------------------------------------------
// <copyright file="PhysicalMAC.cs" company="www.lifeprojects.de">
//     Class: PhysicalMAC
//     Copyright © www.lifeprojects.de 2023
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>17.05.2023 14:35:50</date>
//
// <summary>
// Die Klasse gibt die physische MAC Adresse zurück
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Network
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.NetworkInformation;

    public static class PhysicalMAC
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicalMAC"/> class.
        /// </summary>
        static PhysicalMAC()
        {
        }

        public static string Get()
        {
            string formattedAddress = string.Empty;
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            if (networkInterfaces.Length > 0)
            {
                IEnumerable<PhysicalAddress> addresses = networkInterfaces
                    .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
                    .Select(nic => nic.GetPhysicalAddress());
                if (addresses.Count() > 0)
                {
                    foreach (var address in addresses)
                    {
                        byte[] bytes = address.GetAddressBytes();

                        for (int i = 0; i < bytes.Length; i++)
                        {
                            formattedAddress += bytes[i].ToString("X2");
                            if (i != bytes.Length - 1)
                            {
                                formattedAddress += "-";
                            }
                        }
                    }
                }
            }

            return formattedAddress;
        }

        public static List<string> GetList()
        {
            List<string> macAddress = null;

            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
            if (networkInterfaces != null && networkInterfaces.Length > 0)
            {
                //Get all the physical network adapter addresses
                //Which support IPV4, NOT virtual, has physical address
                //And ordered by device index
                IEnumerable<PhysicalAddress> addresses = networkInterfaces
                    .Where(adapter =>
                            adapter.Supports(NetworkInterfaceComponent.IPv4) == true &&
                            adapter.GetIPProperties().GetIPv4Properties() != null &&
                            adapter.Description.Contains("Virtual") != true &&
                            adapter.GetPhysicalAddress().ToString() != "")
                    .OrderBy(adapter => adapter.GetIPProperties().GetIPv4Properties().Index)
                    .Select(nic => nic.GetPhysicalAddress());

                if (addresses != null && addresses.Count() > 0)
                {
                    macAddress = new List<string>();

                    addresses.ToList().ForEach(
                                            address =>
                                            {
                                                string formattedAddress = string.Empty;
                                                byte[] bytes = address.GetAddressBytes();
                                                for (int i = 0; i < bytes.Length; i++)
                                                {
                                                    formattedAddress += bytes[i].ToString("X2");
                                                    if (i != bytes.Length - 1)
                                                    {
                                                        formattedAddress += "-";
                                                    }
                                                }

                                                macAddress.Add(formattedAddress);
                                            });
                }

            }

            return macAddress;
        }

        public static Dictionary<string, string> NetworkInterfaceInfo()
        {
            Dictionary<string, string> interfaceInfo = new Dictionary<string, string>();

            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            if (computerProperties != null)
            {
                interfaceInfo.Add("Interface information", $"{computerProperties.HostName}.{computerProperties.DomainName}");
            }

            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            if (nics == null || nics.Length < 1)
            {
                interfaceInfo.Add("No network interfaces found.", string.Empty);
                return interfaceInfo;
            }

            interfaceInfo.Add("Number of interfaces", nics.Length.ToString());
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                interfaceInfo.Add(adapter.Description, adapter.NetworkInterfaceType.ToString());
                PhysicalAddress address = adapter.GetPhysicalAddress();

                string formattedAddress = string.Empty;
                byte[] bytes = address.GetAddressBytes();
                for (int i = 0; i < bytes.Length; i++)
                {
                    formattedAddress += bytes[i].ToString("X2");
                    if (i != bytes.Length - 1)
                    {
                        formattedAddress += "-";
                    }
                }

                interfaceInfo.Add($"Physical MAC address for #{adapter.Name}", $"{formattedAddress}");
                interfaceInfo.Add(adapter.Id, string.Empty);
            }

            return interfaceInfo;
        }
    }
}
