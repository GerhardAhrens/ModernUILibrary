namespace ModernTest.ModernBaseLibrary
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Runtime.Loader;
    using System.Text;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public static class AssemblyHelper
    {
        public static Func<Assembly> GetEntryAssembly = () => Assembly.GetEntryAssembly();
    }

    [TestClass]
    public abstract class BaseTest
    {
        private readonly Assembly testAssembly = null;

        public BaseTest()
        {
            /* Preparing test start */
            this.testAssembly = Assembly.GetEntryAssembly();
            this.TestAssembly = this.testAssembly;

            Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
        }

        public CultureInfo GetCurrentCulture
        {
            get { return Thread.CurrentThread.CurrentCulture; }
        }

        public DirectoryInfo GetAssemblyPath
        {
            get
            {
                string assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                return new DirectoryInfo(assemblyPath);
            }
        }

        public string GetAssemblyFullname
        {
            get
            {
                return Assembly.GetEntryAssembly().Location;
            }
        }

        public string GetAssemblyName
        {
            get
            {
                return Path.GetFileName(Assembly.GetEntryAssembly().Location);
            }
        }

        public Assembly TestAssembly { get; private set; }

        public Assembly BaseAssembly { get; private set; }

        public TestContext TestContext { get; set; }

        public string Class
        {
            get { return this.TestContext.FullyQualifiedTestClassName; }
        }

        public string Method
        {
            get { return this.TestContext.TestName; }
        }

        protected virtual void Trace(object message, params object[] args)
        {
            System.Diagnostics.Trace.WriteLine(string.Format(message.ToString(), args));
        }

        protected virtual void Trace(object message)
        {
            System.Diagnostics.Trace.WriteLine(message);
        }

        protected virtual void Trace(string message)
        {
            System.Diagnostics.Trace.WriteLine(message);
        }

        protected virtual void Trace(int message)
        {
            System.Diagnostics.Trace.WriteLine(message);
        }
    }

    public class ReaderLoadContext : AssemblyLoadContext
    {
        private AssemblyDependencyResolver _resolver;

        public ReaderLoadContext(string readerLocation)
        {
            _resolver = new AssemblyDependencyResolver(readerLocation);
        }

        protected override Assembly Load(AssemblyName assemblyName)
        {
            string assemblyPath = _resolver.ResolveAssemblyToPath(assemblyName);
            if (assemblyPath != null)
            {
                return LoadFromAssemblyPath(assemblyPath);
            }

            return null;
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            string libraryPath = _resolver.ResolveUnmanagedDllToPath(unmanagedDllName);

            if (libraryPath != null)
            {
                return LoadUnmanagedDllFromPath(libraryPath);
            }

            return IntPtr.Zero;
        }
    }

    public class DebugTextWriter : TextWriter
    {
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        public override void Write(char value)
        {
            Debug.Write(value);
        }

        public override void Write(string value)
        {
            Debug.Write(value);
        }

        public override void WriteLine(string value)
        {
            Debug.WriteLine(value);
        }
    }

    public class OutputDebugStringTextWriter : TextWriter
    {
        [DllImport("kernel32.dll")]
        static extern void OutputDebugString(string lpOutputString);

        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }

        public override void Write(char value)
        {
            OutputDebugString(value.ToString());
        }

        public override void Write(string value)
        {
            OutputDebugString(value);
        }

        public override void WriteLine(string value)
        {
            OutputDebugString(value);
        }
    }
}
