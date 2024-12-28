namespace ModernBaseLibrary.Core
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Runtime.InteropServices;
    using System.Runtime.Versioning;

    using ModernBaseLibrary.Extension;

    /// <summary>
    /// Computer Information.
    /// </summary>
    [SupportedOSPlatform("windows")]
    public sealed class ComputerInfo
    {
        /// <summary>
        /// Gets the computer culture.
        /// </summary>
        /// <value>The computer culture.</value>
        public string ComputerCulture { get; private set; } = CultureInfo.CurrentCulture.ThreeLetterISOLanguageName;

        /// <summary>
        /// Gets the computer UI culture.
        /// </summary>
        /// <value>The computer UI culture.</value>
        public string ComputerUICulture { get; private set; } = CultureInfo.CurrentUICulture.ThreeLetterISOLanguageName;

        /// <summary>
        /// Gets the current managed tread identifier.
        /// </summary>
        /// <value>The current managed tread identifier.</value>
        public int CurrentManagedTreadId { get; private set; } = Environment.CurrentManagedThreadId;

        /// <summary>
        /// Gets the current stack trace information.
        /// </summary>
        /// <value>The current stack trace.</value>
        public string CurrentStackTrace { get; private set; } = Environment.StackTrace;

        /// <summary>
        /// Gets the current system tick count.
        /// </summary>
        /// <value>The current system tick count.</value>
        public int CurrentSystemTickCount { get; private set; } = Environment.TickCount;

        /// <summary>
        /// Gets the current working directory.
        /// </summary>
        /// <value>The current directory.</value>
        public string CurrentWorkingDirectory { get; private set; } = Environment.CurrentDirectory;

        /// <summary>
        /// Gets the framework description.
        /// </summary>
        /// <value>The framework description.</value>
        public string FrameworkDescription { get; private set; } = RuntimeInformation.FrameworkDescription;

        /// <summary>
        /// Gets the framework version.
        /// </summary>
        /// <value>The framework version.</value>
        public Version FrameworkVersion { get; private set; } = Environment.Version;

        /// <summary>
        /// Gets a value indicating whether this instance has shutdown started.
        /// </summary>
        /// <value><c>true</c> if this instance has shutdown started; otherwise, <c>false</c>.</value>
        public bool HasShutdownStarted { get; private set; } = Environment.HasShutdownStarted;

        /// <summary>
        /// Gets the ip addresses.
        /// </summary>
        /// <value>The ip address.</value>
        public string IPAddress { get; private set; } = Dns.GetHostAddresses(Dns.GetHostName()).Where(p => p.AddressFamily == AddressFamily.InterNetwork).ToDelimitedString(char.Parse(",").ToString());

        /// <summary>
        /// Gets a value indicating whether [is64 bit operating system].
        /// </summary>
        /// <value><c>true</c> if [is64 bit operating system]; otherwise, <c>false</c>.</value>
        public bool Is64BitOperatingSystem { get; private set; } = Environment.Is64BitOperatingSystem;

        /// <summary>
        /// Gets a value indicating whether [is64 bit process].
        /// </summary>
        /// <value><c>true</c> if [is64 bit process]; otherwise, <c>false</c>.</value>
        public bool Is64BitProcess { get; private set; } = Environment.Is64BitProcess;

        /// <summary>
        /// Gets a value indicating whether the user is interactive.
        /// </summary>
        /// <value><c>true</c> if this instance is user interactive; otherwise, <c>false</c>.</value>
        public bool IsUserInteractive { get; private set; } = Environment.UserInteractive;

        /// <summary>
        /// Gets the logical drives.
        /// </summary>
        /// <value>The logical drives.</value>
        public IEnumerable<string> LogicalDrives { get; private set; } = Environment.GetLogicalDrives().AsEnumerable();

        /// <summary>
        /// Gets the name of the machine.
        /// </summary>
        /// <value>The name of the machine.</value>
        public string MachineName { get; private set; } = Environment.MachineName;

        /// <summary>
        /// Gets the os architecture.
        /// </summary>
        /// <value>The os architecture.</value>
        public Architecture OSArchitecture { get; private set; } = RuntimeInformation.OSArchitecture;

        /// <summary>
        /// Gets the os version.
        /// </summary>
        /// <value>The os version.</value>
        public string OSDescription { get; private set; } = RuntimeInformation.OSDescription;

        /// <summary>
        /// Gets the size of the os memory page.
        /// </summary>
        /// <value>The size of the os memory page.</value>
        public int OsMemoryPageSize { get; private set; } = Environment.SystemPageSize;

        /// <summary>
        /// Gets the physical memory in use.
        /// </summary>
        /// <value>The physical memory in use.</value>
        public long PhysicalMemoryInUse { get; private set; } = Environment.WorkingSet;

        /// <summary>
        /// Gets the process architecture.
        /// </summary>
        /// <value>The process architecture.</value>
        public Architecture ProcessArchitecture { get; private set; } = RuntimeInformation.ProcessArchitecture;

        /// <summary>
        /// Gets the processor count.
        /// </summary>
        /// <value>The processor count.</value>
        public int ProcessorCount { get; private set; } = Environment.ProcessorCount;

        /// <summary>
        /// Gets the system directory.
        /// </summary>
        /// <value>The system directory.</value>
        public string SystemDirectory { get; private set; } = Environment.SystemDirectory;

        /// <summary>
        /// Gets the size of the system page.
        /// </summary>
        /// <value>The size of the system page.</value>
        public int SystemPageSize { get; private set; } = Environment.SystemPageSize;

        /// <summary>
        /// Gets the tick count.
        /// </summary>
        /// <value>The tick count.</value>
        public int TickCount { get; private set; } = Environment.TickCount;

        /// <summary>
        /// Gets the tick count as long.
        /// </summary>
        /// <value>The tick count as long.</value>
        public long TickCount64 { get; private set; } = Environment.TickCount64;

        /// <summary>
        /// Gets the name of the user domain.
        /// </summary>
        /// <value>The name of the user domain.</value>
        public string UserDomainName { get; private set; } = Environment.UserDomainName;

        /// <summary>
        /// Gets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; private set; } = Environment.UserName;
    }
}
