//-----------------------------------------------------------------------
// <copyright file="ProcessExtensions.cs" company="Lifeprojects.de">
//     Class: ProcessExtensions
//     Copyright © Lifeprojects.de 2018
// </copyright>
//
// <author>Gerhard Ahrens</author>
// <email>developer@lifeprojects.de</email>
// <date>.09.2018</date>
//
// <summary>Extension Class for ProcessExtensions</summary>
//-----------------------------------------------------------------------

namespace ModernBaseLibrary.Extension
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Runtime.InteropServices;

    using ModernBaseLibrary.Win32API;

    public static class ProcessExtensions
    {
        [DllImport("kernel32.dll")]
        private static extern bool Process32First(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        [DllImport("kernel32.dll")]
        private static extern bool Process32Next(IntPtr hSnapshot, ref PROCESSENTRY32 lppe);

        public static Process ParentProcess(this Process process)
        {
            int parentPid = 0;
            int processPid = process.Id;

            IntPtr hSnapshot = Win32API.Instance.CreateToolHelp32Snapshot();
            if (hSnapshot == IntPtr.Zero)
            {
                return null;
            }

            PROCESSENTRY32 procInfo = new PROCESSENTRY32();
            procInfo.dwSize = (uint)Marshal.SizeOf(typeof(PROCESSENTRY32));

            if (Process32First(hSnapshot, ref procInfo) == false)
            {
                return null;
            }

            do
            {
                if (processPid == procInfo.th32ProcessID)
                {
                    parentPid = (int)procInfo.th32ParentProcessID;
                }
            }

            while (parentPid == 0 && Process32Next(hSnapshot, ref procInfo));

            if (parentPid > 0)
            {
                return Process.GetProcessById(parentPid);
            }
            else
            {
                return null;
            }
        }

        public static bool IsWordRunning(this Process[] @this, string windowTitle = "")
        {
            bool result = false;

            if (string.IsNullOrEmpty(windowTitle) == true)
            {
                int countProcess = @this.Count(p => p.ProcessName.ToLower().Contains("winword"));
                if (countProcess > 0)
                {
                    result = true;
                }
            }
            else
            {
                int countProcess = @this.Count(p => p.MainWindowTitle.ToLower().Contains(windowTitle.ToLower()));
                if (countProcess > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool IsWordRunning(this IEnumerable<Process> @this, string windowTitle = "")
        {
            bool result = false;

            if (string.IsNullOrEmpty(windowTitle) == true)
            {
                int countProcess = @this.Count(p => p.ProcessName.ToLower().Contains("winword"));
                if (countProcess > 0)
                {
                    result = true;
                }
            }
            else
            {
                int countProcess = @this.Count(p => p.MainWindowTitle.ToLower().Contains(windowTitle.ToLower()));
                if (countProcess > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool IsExcelRunning(this Process[] @this, string windowTitle = "")
        {
            bool result = false;

            if (string.IsNullOrEmpty(windowTitle) == true)
            {
                int countProcess = @this.Count(p => p.ProcessName.ToLower().Contains("excel"));
                if (countProcess > 0)
                {
                    result = true;
                }
            }
            else
            {
                int countProcess = @this.Count(p => p.MainWindowTitle.ToLower().Contains(windowTitle.ToLower()));
                if (countProcess > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        public static bool IsExcelRunning(this IEnumerable<Process> @this, string windowTitle = "")
        {
            bool result = false;

            if (string.IsNullOrEmpty(windowTitle) == true)
            {
                int countProcess = @this.Count(p => p.ProcessName.ToLower().Contains("excel"));
                if (countProcess > 0)
                {
                    result = true;
                }
            }
            else
            {
                int countProcess = @this.Count(p => p.MainWindowTitle.ToLower().Contains(windowTitle.ToLower()));
                if (countProcess > 0)
                {
                    result = true;
                }
            }

            return result;
        }

        /// <summary>
        /// Wartet asynchron auf die Beendigung des Prozesses.
        /// </summary>
        /// <param name="process">Der Prozess zum Warten auf die Beendigung.</param>
        /// <returns>Ein Task, der auf das Ende des Prozesses wartet.</returns>
        public static Task WaitForExitAsync(this Process process)
        {
            return WaitForExitAsync(process, CancellationToken.None);
        }

        /// <summary>
        /// Wartet asynchron auf die Beendigung des Prozesses.
        /// </summary>
        /// <param name="process">Der Prozess zum Warten auf die Beendigung.</param>
        /// <param name="cancellationToken">Ein Abbruch-Token. Wenn es aufgerufen wird, wird die Aufgabe sofort als abgebrochen zurückgegeben.</param>
        /// <returns>Ein Task, der auf das Ende des Prozesses wartet.</returns>
        public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken)
        {
            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null);
            if (cancellationToken != CancellationToken.None)
                cancellationToken.Register(() => tcs.TrySetCanceled());
            return tcs.Task;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESSENTRY32
        {
            public uint dwSize;
            public uint cntUsage;
            public uint th32ProcessID;
            public IntPtr th32DefaultHeapID;
            public uint th32ModuleID;
            public uint cntThreads;
            public uint th32ParentProcessID;
            public int pcPriClassBase;
            public uint dwFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szExeFile;
        }
    }
}
