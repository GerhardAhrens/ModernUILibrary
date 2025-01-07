//-----------------------------------------------------------------------
// <copyright file="ProcessHelper.cs" company="Lifeprojects.de">
//     Class: ProcessHelper
//     Copyright © Lifeprojects.de 2019
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>developer@lifeprojects.de</email>
// <date>06.09.2019</date>
//
// <summary>
//      Über die Klasse kann ein angegebene Applikation aus der Prozessliste entfernt werden.
// </summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Management;
    using System.Runtime.Versioning;

    [SupportedOSPlatform("windows")]
    public class ProcessHelper : DisposableCoreBase
    {
        private Process[] internalProcessList = null;

        public ProcessHelper()
        {
            this.internalProcessList = Process.GetProcesses();
        }

        public IEnumerable<Process> GetProcesses()
        {
            return this.internalProcessList.Cast<Process>();
        }

        public Process GetProcess(string applicationName)
        {
            applicationName.IsArgumentNullOrEmpty(applicationName);
            return this.internalProcessList.Cast<Process>().FirstOrDefault(f => f.ProcessName.ToLower().Contains(applicationName.ToLower()));
        }

        public Process GetProcess(int processId)
        {
            processId.IsArgumentOutOfRange("processId", processId > 1, "the 'processId' must greater 0");
            return this.internalProcessList.Cast<Process>().FirstOrDefault(f => f.Id == processId);
        }

        public string GetProcessOwner(int processId)
        {
            processId.IsArgumentOutOfRange("processId", processId > 1, "the 'processId' must greater 0");

            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    return $"{argList[1]}\\{argList[0]}"; 
                }
            }

            return "NoOwner";
        }

        public int GetProcessId(string applicationName)
        {
            applicationName.IsArgumentNullOrEmpty(applicationName);

            int result = -1;

            string processName = string.Empty;

            try
            {
                foreach (var item in this.internalProcessList.Where(w => w.ProcessName == applicationName))
                {
                    processName = $"ProcessName: {item.ProcessName}; Id: {item.Id}";
                    result = item.Id;
                    break;
                }
            }
            catch (System.Exception ex)
            {
                ex.Data.Add("ProcessName", processName);
                throw;
            }

            return result;
        }

        public void KillByName(string applicationName)
        {
            applicationName.IsArgumentNullOrEmpty(applicationName);

            string processName = string.Empty;

            try
            {
                foreach (var item in this.internalProcessList.Where(w => w.ProcessName == applicationName))
                {
                    processName = $"ProcessName: {item.ProcessName}; Id: {item.Id}";
                    item.Kill();
                }
            }
            catch (System.Exception ex)
            {
                ex.Data.Add("ProcessName", processName);
                throw;
            }
        }

        public void KillById(int processId)
        {
            processId.IsArgumentOutOfRange("processId", processId > 1, "the 'processId' must greater 0");

            string processName = string.Empty;
            try
            {
                foreach (var item in this.internalProcessList.Where(w => w.Id == processId))
                {
                    processName = $"ProcessName: {item.ProcessName}; Id: {item.Id}";
                    item.Kill();
                }
            }
            catch (System.Exception ex)
            {
                ex.Data.Add("ProcessName", processName);
                throw;
            }
        }

        public Dictionary<int,string> KillProcessByNameAndUser(string processName, string processUserName, int hasStartedForMinutes)
        {
            Dictionary<int, string> result = new Dictionary<int, string>();
            Process[] foundProcesses = Process.GetProcessesByName(processName);
            Console.WriteLine(foundProcesses.Length.ToString() + " processes found.");

            string strMessage = string.Empty;
            foreach (Process process in foundProcesses)
            {
                string userName = this.GetProcessOwner(process.Id);
                strMessage = $"Process Name: {process.ProcessName} | Process ID: {process.Id} | User Name : {userName} | StartTime {process.StartTime}";
                /*Console.WriteLine(strMessage);*/
                bool timeExpired = (process.StartTime.AddMinutes(hasStartedForMinutes) < DateTime.Now) || hasStartedForMinutes == 0;
                bool prcoessUserNameIsMatched = userName.Equals(processUserName);

                if ((processUserName.ToLower() == "all" && timeExpired) || (prcoessUserNameIsMatched && timeExpired))
                {
                    process.Kill();
                    result.Add(process.Id, process.ProcessName);
                }
            }

            return result;
        }

        public override void DisposeManagedResources()
        {
            this.internalProcessList = null;
        }
    }
}
