//-----------------------------------------------------------------------
// <copyright file="UnitTestDetector.cs" company="Lifeprojects.de">
//     Class: UnitTestDetector
//     Copyright © Gerhard Ahrens, 2017
// </copyright>
//
// <author>Gerhard Ahrens - Lifeprojects.de</author>
// <email>development@lifeprojects.de</email>
// <date>12.03.2015</date>
//
// <summary>Class with UnitTestDetector Definition</summary>
//-----------------------------------------------------------------------

namespace ModernIU.Base
{
    using System;
    using System.Linq;

    public static class UnitTestDetector
    {
        static UnitTestDetector()
        {
            const string TestAssemblyName = "Microsoft.VisualStudio.QualityTools.UnitTestFramework";
            const string MsText = "Microsoft.VisualStudio.TestPlatform";

            UnitTestDetector.IsInUnitTest = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName.StartsWith(TestAssemblyName) || a.FullName.StartsWith(MsText));
            UnitTestDetector.FriendlyName = AppDomain.CurrentDomain.FriendlyName;
            UnitTestDetector.BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        }

        public static bool IsInUnitTest { get; private set; }

        public static string FriendlyName { get; private set; }

        public static string BaseDirectory { get; private set; }
    }
}
