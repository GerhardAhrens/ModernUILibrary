//-----------------------------------------------------------------------
// <copyright file="DetectVirtualMachine.cs" company="Lifeprojects.de">
//     Class: DetectVirtualMachine
//     Copyright © Lifeprojects.de 2025
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>Gerhard Ahrens@Lifeprojects.de</email>
// <date>15.07.2025 06:55:13</date>
//
// <summary>
// Die Klasse prüft, ob der Source auf einer VM oder auf einem physischem Gerät ausgeführt wird.
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core.IO
{
    using System;
    using System.Management;

    public class DetectVirtualMachine
    {
        public static VirtualMachine IsVirtualMachine()
        {
            const string MICROSOFTCORPORATION = "microsoft corporation";
            const string VMWARE = "vmware";
            const string VIRTUALBOX = "virtualbox";
            const string pattern = "SELECT * from Win32_ComputerSystem WHERE (Manufacturer LIKE '%microsoft corporation%' AND Model LIKE '%virtual%') OR Manufacturer LIKE '%vmware%' OR Model LIKE '%VirtualBox%'";

            VirtualMachine vm = new VirtualMachine();

            try
            {
                foreach (var item in new ManagementObjectSearcher(pattern).Get())
                {
                    string manufacturer = item["Manufacturer"].ToString().ToLower();
                    // Check the Manufacturer (eg: vmware, inc)
                    if (manufacturer.Contains(MICROSOFTCORPORATION) == true || manufacturer.Contains(VMWARE) == true || manufacturer.Contains(VIRTUALBOX) == true)
                    {
                        vm.Name = item["Manufacturer"].ToString();
                        if (item["Model"] != null)
                        {
                            vm.Version = item["Model"].ToString();
                        }

                        vm.IsVM = true;
                        return vm;
                    }

                    // Also, check the model (eg: VMware Virtual Platform)
                    if (item["Model"] != null)
                    {
                        string model = item["Model"].ToString().ToLower();
                        if (model.Contains(MICROSOFTCORPORATION) == true || model.Contains(VMWARE) == true || manufacturer.Contains(VIRTUALBOX) == true)
                        {
                            vm.Name = item["Manufacturer"].ToString();
                            vm.Version = item["Model"].ToString().Replace(",", ".");
                            vm.IsVM = true;
                            return vm;
                        }
                    }
                }

                vm.Name = string.Empty;
                vm.Version = string.Empty;
                vm.IsVM = false;
                return vm;
            }
            catch (Exception ex)
            {
                string errorText = ex.Message;
                throw;
            }
        }
    }

    public class VirtualMachine
    {
        public bool IsVM { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }
    }
}
